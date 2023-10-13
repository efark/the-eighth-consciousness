using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    [Header("Game Management")]
    public GameObject gameController;

    [Header("Prefabs")]
    public GameObject bullet;
    public GameObject bomb;

    [Header("Fire")]
    public int firePower = 1;
    public int maxFirePower = 5;
    public int minFirePower = 1;
    public float fireRate = 4;
    public float maxFireRate = 10;
    public float minFireRate = 4;
    public float ECDCooldown = 10;
    public float ECDDuration = 4;
    public List<Transform> firepoints = new List<Transform>();

    private float nextFire;
    private float activeECDCooldown;
    private bool ECDenabled;
    private bool ECDready;

    private GameObject[] players;
    private Collider[] playerColliders;

    // Start is called before the first frame update
    void Start()
    {
        nextFire = 5 / fireRate;
        activeECDCooldown = ECDCooldown;
        ECDready = true;
        ECDenabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        nextFire = Mathf.Max(nextFire - Time.deltaTime, 0);
        bool fireButton = Input.GetButton("Fire1");
        bool slowMoButton = Input.GetButton("Fire3");

        // This should be modified later.
        players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject p in players)
        {
            playerColliders = p.transform.GetComponentsInChildren<Collider>();
        }

        if (fireButton)
        {
            if (nextFire <= 0)
            {
                Debug.Log("Fire!");
                nextFire = 5 / fireRate;
                for (int i = 0; i < firepoints.Count; i++)
                {
                    GameObject bulletInst = Instantiate(bullet, firepoints[i].position, Quaternion.identity);

                    for (int j = 0; j < playerColliders.Length; j++)
                    {
                        Physics.IgnoreCollision(bulletInst.transform.GetComponent<Collider>(), playerColliders[j]);
                    }
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
        gameController.transform.GetComponent<TimeController>().SlowMotionEffect(true);
        StartCoroutine(endSlowMo(ECDDuration));
    }

    IEnumerator endSlowMo(float timeout)
    {
        //Wait until timeout seconds pass.
        yield return new WaitForSecondsRealtime(timeout);
        Debug.Log("ECD stop!");
        ECDenabled = false;
        activeECDCooldown = ECDCooldown;
        gameController.transform.GetComponent<TimeController>().SlowMotionEffect(false);
    }



}
