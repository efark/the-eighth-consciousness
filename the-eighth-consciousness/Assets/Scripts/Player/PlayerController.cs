using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject bullet;
    public GameObject bomb;

    [Header("Movement")]
    public float speed = 12;
    public float ECDSpeed = 16;

    [Header("Fire")]
    public int firePower = 1;
    public int maxFirePower = 5;
    public int minFirePower = 1;
    public float fireRate = 0.2f;
    public float ECDCooldown = 10;
    public float ECDDuration = 4;
    public List<Transform> firepoints = new List<Transform>();

    public PlayerStats stats;
    private float nextFire;
    private float activeECDCooldown;
    private float activeSpeed;
    private bool ECDenabled;
    private bool ECDready;

    private Rigidbody2D rigidBody;
    private GameObject[] players;
    private Collider2D[] playerColliders;

    public BulletSettings bulletSettings;
    public SpreadSettings spreadSettings;
    private AbstractSpread spread;
    private BulletFactory bFactory;

    public static event Action<bool> OnTriggerECD;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = transform.GetComponent<Rigidbody2D>();
        nextFire = 1 / fireRate;
        activeECDCooldown = ECDCooldown;
        activeSpeed = speed;
        ECDready = true;
        ECDenabled = false;

        PlayerStats.OnPlayerDeath += Death;

        bFactory = new BulletFactory(bulletSettings, TargetTypes.Enemy, 1, 0f);
        spread = ExtensionMethods.InitSpread(bFactory, spreadSettings);
    }

    public void Death(int playerId)
    {
        PlayerStats.OnPlayerDeath -= Death;
        // Trigger some sound.
        // Trigger visual effect.
        // Update some values in state.
        Destroy(gameObject);
    }

    // Code taken from Unity Reference: https://docs.unity3d.com/ScriptReference/GameObject.FindGameObjectsWithTag.html
    public GameObject FindClosestEnemy()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }
    // End of quoted code.

    void FixedUpdate()
    {
        // Character movement.
        float moveVertical = Input.GetAxis("Vertical");
        float moveHorizontal = Input.GetAxis("Horizontal");

        Vector3 movement = new Vector2(moveHorizontal, moveVertical);
        rigidBody.velocity = movement * activeSpeed;


    }

    void Update()
    {
        nextFire = Mathf.Max(nextFire - Time.deltaTime, 0);
        bool fireButton = Input.GetButton("Fire1");
        bool slowMoButton = Input.GetButton("Fire3");

        // This should be modified later.
        players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject p in players)
        {
            playerColliders = p.transform.GetComponentsInChildren<Collider2D>();
        }

        if (fireButton)
        {
            if (nextFire <= 0)
            {
                nextFire = 1 / fireRate;
                for (int i = 0; i < firepoints.Count; i++)
                {
                    spread.Create(transform.position, transform.rotation, Vector2.up);
                    //StartCoroutine(burst.Fire(transform.position, transform.rotation, Vector2.up));
                    //burst.Fire(transform.position, transform.rotation, Vector2.up);
                    //Fire(transform.position, transform.rotation, Vector2.up);
                }
            }
        }

        if (slowMoButton)
        {
            if (ECDready && !ECDenabled)
            {
                triggerSlowMo();
            }
        }

        if (!ECDenabled && !ECDready)
        {
            activeECDCooldown = Mathf.Max(activeECDCooldown - Time.deltaTime, 0);
            if (activeECDCooldown == 0)
            {
                Debug.Log("ECD Ready!");
                ECDready = true;
            }
        }

    }

    private void triggerSlowMo()
    {
        Debug.Log("ECD start!");
        ECDenabled = true;
        ECDready = false;
        activeSpeed = ECDSpeed;
        //gameController.transform.GetComponent<TimeController>().SlowMotionEffect(true);
        OnTriggerECD?.Invoke(true);
        StartCoroutine(endSlowMo(ECDDuration));
    }

    IEnumerator endSlowMo(float timeout)
    {
        //Wait until timeout seconds pass.
        yield return new WaitForSecondsRealtime(timeout);
        Debug.Log("ECD stop!");
        ECDenabled = false;
        activeSpeed = speed;
        activeECDCooldown = ECDCooldown;
        //gameController.transform.GetComponent<TimeController>().SlowMotionEffect(false);
        OnTriggerECD?.Invoke(false);
    }

}
