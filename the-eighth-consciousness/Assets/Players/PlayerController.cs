using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject bomb;

    [Header("Movement")]
    public float speed = 12;
    public float ECDSpeed = 16;

    [Header("Fire")]
    public float fireRate = 4f;
    public float ECDFireRate = 6f;
    private float activeFireRate;
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
    private float bombCooldown;
    private bool bombIsActive;

    public static event Action<bool> OnTriggerECD;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = transform.GetComponent<Rigidbody2D>();
        activeFireRate = fireRate;
        nextFire = 1 / activeFireRate;
        activeECDCooldown = ECDCooldown;
        activeSpeed = speed;
        ECDready = true;
        ECDenabled = false;

        PlayerStats.OnPlayerDeath += Death;

        //Debug.Log($"stats.CurrentFirePower: {stats.CurrentFirePower}");

        bFactory = new BulletFactory(bulletSettings, TargetTypes.Enemy, 1, 0f, 1f + stats.CurrentFirePower * 0.2f);
        spread = AuxiliaryMethods.InitSpread(bFactory, spreadSettings);

    }

    public void UpdateFirePower(int i)
    {
        stats.UpdateFirePower(i);
        bFactory.UpdateFactor(1f + stats.CurrentFirePower * 0.2f);
    }

    public void Death(int playerId)
    {
        PlayerStats.OnPlayerDeath -= Death;
        // Trigger some sound.
        // Trigger visual effect.
        // Update some values in state.
        Destroy(gameObject);
    }

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
        bool bombButton = Input.GetButtonDown("Fire2");
        bool slowMoButton = Input.GetButtonDown("Fire3");

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
                nextFire = 1 / activeFireRate;
                for (int i = 0; i < firepoints.Count; i++)
                {
                    spread.Create(transform.position, transform.rotation, Vector2.up);
                }
            }
        }
        if (bombButton)
        {
            if (stats.CurrentBombs > 0 && !bombIsActive)
            {
                bombIsActive = true;
                stats.UpdateBombs(-1);
                Instantiate(bomb, transform.position, Quaternion.identity);
                StartCoroutine(waitBombCooldown(bombCooldown));
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
        activeFireRate = ECDFireRate;
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
        activeFireRate = fireRate;
        activeECDCooldown = ECDCooldown;
        OnTriggerECD?.Invoke(false);
    }

    private IEnumerator waitBombCooldown(float timeout)
    {
        yield return new WaitForSecondsRealtime(timeout);
        bombIsActive = false;
    }

}
