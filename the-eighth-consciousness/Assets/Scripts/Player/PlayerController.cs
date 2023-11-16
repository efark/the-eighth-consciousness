using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Game Management")]
    public GameObject gameController;
    public GameObject playerPrefab;
    public Vector3 startingPoint;

    [Header("Prefabs")]
    public GameObject bullet;
    public GameObject bomb;

    [Header("Stats")]
    public int HP = 100;
    public int MaxHP = 100;
    public int MinHP = 0;
    public int lives = 3;
    public int continues = 3;
    public int bombs = 3;

    [Header("Movement")]
    public float speed;
    public float maxSpeed;
    public float minSpeed;

    [Header("Fire")]
    public int firePower = 1;
    public int maxFirePower = 5;
    public int minFirePower = 1;
    public float fireRate = 0.2f;
    public float ECDCooldown = 10;
    public float ECDDuration = 4;
    public List<Transform> firepoints = new List<Transform>();

    private bool isAlive;
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
    public BurstSettings burstSettings;
    private AbstractSpread spread;
    private AbstractBurst burst;
    private BulletFactory bFactory;

    public int playerHP
    {
        get
        {
            return HP;
        }
        set
        {
            if (value != HP)
            {
                HP = Helpers.Clamp(value, MinHP, MaxHP);
                //Raise();
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = transform.GetComponent<Rigidbody2D>();
        nextFire = 1 / fireRate;
        activeECDCooldown = ECDCooldown;
        activeSpeed = speed;
        ECDready = true;
        ECDenabled = false;
        isAlive = true;

        //spread = new RadialSpread(bulletSettings, "Enemy", 1, 3, 90);
        bFactory = new BulletFactory(bulletSettings, TargetTypes.Enemy, 1);
        spread = ExtensionMethods.InitSpread(bFactory, spreadSettings);
        burst = new Burst(spread, burstSettings.offset, burstSettings.size, burstSettings.fireRate);
    }

    void InitNewLife()
    {
        isAlive = true;
        HP = 100;
        bombs = Mathf.Max(bombs - 1, 1);
        speed = Mathf.Max(speed - 1, minSpeed);
        firePower = Mathf.Max(firePower - 1, minFirePower);
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

        //rigidBody.rotation = Quaternion.Euler(Vector3.forward * moveHorizontal * tiltAngle);
        //rigidBody.rotation = Quaternion.Euler(Vector3.right * 90);

    }

    void Update()
    {
        //
        if (HP < MinHP)
        {
            // Death.
            playerDeath();
        }

        if (!isAlive)
        {
            return;
        }

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
        activeSpeed *= 1.25f;
        gameController.transform.GetComponent<TimeController>().SlowMotionEffect(true);
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
        gameController.transform.GetComponent<TimeController>().SlowMotionEffect(false);
    }

    IEnumerator playerDeath()
    {
        // Trigger Death Animation.
        isAlive = false;
        yield return new WaitForSecondsRealtime(2);

        // Reset values.
        InitNewLife();
        Instantiate(playerPrefab, transform.TransformPoint(startingPoint), Quaternion.identity);
    }

}
