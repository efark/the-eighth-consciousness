﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject bomb;
    public int _playerId;

    [Header("Movement")]
    public float speed = 12;
    public float ECDSpeed = 18;

    [Header("Fire")]
    public float fireRate = 4f;
    public float ECDFireRate = 6f;
    private float activeFireRate;
    public float ECDCooldown = 10;
    public float ECDDuration = 4;
    public List<Transform> firepoints = new List<Transform>();

    [Header("Sound FX")]
    public AudioSource shootSFX;
    public AudioSource deathSFX;
    public AudioSource bombSFX;
    public AudioSource ecdStartSFX;
    public AudioSource hitSFX;

    public PlayerStats stats;
    private float nextFire;
    private float activeECDCooldown;
    private float activeSpeed;
    private bool ECDenabled;
    private bool ECDready;
    private Camera cam;
    private Vector3 bottomLeft;
    private Vector3 topRight;
    private Rect cameraRect;

    private Rigidbody2D rigidBody;
    private GameObject[] players;
    private Collider2D[] playerColliders;

    public BulletSettings bulletSettings;
    public SpreadSettings spreadSettings;
    private AbstractSpread spread;
    private BulletFactory bFactory;
    private float bombCooldown;
    private bool bombIsActive;

    private string xAxisName;
    private string yAxisName;
    private string fire1Name;
    private string fire2Name;
    private string fire3Name;

    public static event Action<bool> OnTriggerECD;

    private void mapButtons()
    {
        xAxisName = $"Horizontal{_playerId}";
        yAxisName = $"Vertical{_playerId}";
        fire1Name = $"Fire{_playerId}1";
        fire2Name = $"Fire{_playerId}2";
        fire3Name = $"Fire{_playerId}3";
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = transform.GetComponent<Rigidbody2D>();
        activeFireRate = fireRate;
        nextFire = 1 / activeFireRate;
        activeECDCooldown = ECDCooldown;
        activeSpeed = speed;
        ECDready = true;
        stats.UpdateECDStatus("Ready");
        ECDenabled = false;

        // PlayerStats.OnPlayerDeath += Death;
        PlayerStats.OnPlayerHit += PlayerHit;

        bFactory = new BulletFactory(bulletSettings, TargetTypes.Enemy, 1, 0f, 1f + stats.CurrentFirePower * 0.2f);
        spread = AuxiliaryMethods.InitSpread(bFactory, spreadSettings);

        mapButtons();

        // https://docs.unity3d.com/ScriptReference/Camera.ScreenToWorldPoint.html
        cam = Camera.main;
        bottomLeft = cam.ScreenToWorldPoint(Vector3.zero);
        topRight = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, cam.pixelHeight));

        cameraRect = new Rect(
            bottomLeft.x,
            bottomLeft.y,
            topRight.x - bottomLeft.x,
            topRight.y - bottomLeft.y);
    }

    public void UpdateFirePower(int i)
    {
        stats.UpdateFirePower(i);
        bFactory.UpdateFactor(1f + stats.CurrentFirePower * 0.2f);
    }

    public void PlayerHit(int playerId)
    {
        if (playerId == _playerId)
        {
            hitSFX.PlayOneShot(hitSFX.clip);
            if (stats.CurrentHP <= 0)
            {
                Death(playerId);
            }
        }
    }

    public void Death(int playerId)
    {
        if (playerId == _playerId)
        {
            if (ECDenabled)
            {
                endSlowMo();
            }
            // PlayerStats.OnPlayerDeath -= Death;
            PlayerStats.OnPlayerHit -= PlayerHit;
            // Trigger some sound.
            //deathSFX.Play();
            // Trigger visual effect.
            // Update some values in state.
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        // Character movement.
        float moveHorizontal = Input.GetAxis(xAxisName);
        float moveVertical = Input.GetAxis(yAxisName);
        
        // Reached Vertical limit of screen.
        if ((moveVertical > 0 && transform.position.y >= cameraRect.yMax) ||
            (moveVertical < 0 && transform.position.y <= cameraRect.yMin))
        {
            moveVertical = 0;
        }

        // Reached Horizontal limit of screen.
        if ((moveHorizontal < 0 && transform.position.x <= cameraRect.xMin) ||
            (moveHorizontal > 0 && transform.position.x >= cameraRect.xMax))
        {
            moveHorizontal = 0;
        }

        Vector3 movement = new Vector2(moveHorizontal, moveVertical);
        rigidBody.velocity = movement * activeSpeed;

    }

    void Update()
    {
        nextFire = Mathf.Max(nextFire - Time.deltaTime, 0);
        bool fireButton = Input.GetButton(fire1Name);
        bool bombButton = Input.GetButtonDown(fire2Name);
        bool slowMoButton = Input.GetButtonDown(fire3Name);

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
                shootSFX.PlayOneShot(shootSFX.clip);
                for (int i = 0; i < firepoints.Count; i++)
                {
                    spread.Create(firepoints[i].transform.position, firepoints[i].transform.rotation, Vector2.up);
                }
            }
        }
        if (bombButton)
        {
            if (stats.CurrentBombs > 0 && !bombIsActive)
            {
                bombSFX.Play();
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
                ECDready = true;
                stats.UpdateECDStatus("Ready");
            }
        }

    }

    private void triggerSlowMo()
    {
        ECDenabled = true;
        ECDready = false;
        activeSpeed = ECDSpeed;
        activeFireRate = ECDFireRate;
        stats.UpdateECDStatus("Active");
        ecdStartSFX.Play();
        OnTriggerECD?.Invoke(true);
        StartCoroutine(waitAndEndSlowMo(ECDDuration));
    }

    IEnumerator waitAndEndSlowMo(float timeout)
    {
        //Wait until timeout seconds pass.
        yield return new WaitForSecondsRealtime(timeout);
        if (ECDenabled)
        {
            endSlowMo();
        }
    }

    private void endSlowMo()
    {
        ECDenabled = false;
        stats.UpdateECDStatus("Charging");
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
