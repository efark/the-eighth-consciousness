using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ECDStatus
{
    Charging,
    Active,
    Ready,
    Cooldown
}

public class PlayerController : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject bomb;
    public int _playerId;
    public GameObject explosion;
    public GameObject ECDParticlesGO;
    private ParticleSystem ECDParticles;

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

    [Header("Blinking VFX")]
    public float spriteBlinkingTimer = 0.0f;
    public float spriteBlinkingMiniDuration = 0.1f;
    public float spriteBlinkingTotalTimer = 0.0f;
    public float spriteBlinkingTotalDuration = 1.0f;
    public bool isBlinking = false;

    [Header("Bars")]
    private ECDStatus _ECDStatus;
    [SerializeField] private Image ECDSprite;
    private Color ECDReady;
    private Color ECDActive;
    private Color ECDCharging;
    private float ECDruntime;

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

    private float iFrameCooldown;

    private string xAxisName;
    private string yAxisName;
    private string fire1Name;
    private string fire2Name;
    private string fire3Name;

    public static event Func<bool> CheckECD;
    public static event Action<bool> OnTriggerECD;
    public static event Action<int> OnECD;
    public static event Action<int> OnMovement;
    public static event Action<int> OnShoot;
    public static event Action<int> OnBombUse;


    public void SetECDSprite(Image _sprite)
    { 
        if (ECDSprite == null)
        {
            ECDSprite = _sprite;
            return;
        }
    }

    private void mapButtons()
    {
        xAxisName = $"Horizontal{_playerId}";
        yAxisName = $"Vertical{_playerId}";
        fire1Name = $"Fire{_playerId}1";
        fire2Name = $"Fire{_playerId}2";
        fire3Name = $"Fire{_playerId}3";
    }

    private void initECDColors()
    {
        ECDReady = Color.blue;
        ECDActive = Color.cyan;
        ECDCharging = Color.yellow;
    }

    // Start is called before the first frame update
    void Start()
    {
        mapButtons();
        initECDColors();

        rigidBody = transform.GetComponent<Rigidbody2D>();
        activeFireRate = fireRate;
        nextFire = 1 / activeFireRate;
        activeECDCooldown = ECDCooldown;
        activeSpeed = speed;
        ECDready = true;
        ECDenabled = false;
        _ECDStatus = ECDStatus.Ready;
        spriteBlinkingTotalDuration = stats.IFrameDuration;
        ECDParticles = ECDParticlesGO.transform.GetComponent<ParticleSystem>();

        PlayerStats.OnPlayerDeath += Death;
        PlayerStats.OnGameOver += Death;
        PlayerStats.OnPlayerHit += PlayerHit;

        bFactory = new BulletFactory(bulletSettings, TargetTypes.Enemy, _playerId, 0f, 1f + stats.CurrentFirePower * 0.2f);
        spread = AuxiliaryMethods.InitSpread(bFactory, spreadSettings);

        /*-------------------------------------------------------------------------------------
        The logic to calculate the screen borders was taken from Unity's documentation:
        https://docs.unity3d.com/ScriptReference/Camera.ScreenToWorldPoint.html
        -------------------------------------------------------------------------------------*/
        cam = Camera.main;
        bottomLeft = cam.ScreenToWorldPoint(Vector3.zero);
        topRight = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, cam.pixelHeight));

        cameraRect = new Rect(
            bottomLeft.x,
            bottomLeft.y,
            topRight.x - bottomLeft.x,
            topRight.y - bottomLeft.y);

        iFrameCooldown = stats.IFrameDuration;
        isBlinking = true;
    }

    void OnDestroy()
    {
        PlayerStats.OnPlayerDeath -= Death;
        PlayerStats.OnGameOver -= Death;
        PlayerStats.OnPlayerHit -= PlayerHit;
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
            iFrameCooldown = stats.IFrameDuration;
            isBlinking = true;
            hitSFX.PlayOneShot(hitSFX.clip);
        }
    }
    /*---------------------------------------
     Code adapted from post in Unity Forum:
     https://discussions.unity.com/t/sprite-blinking-effect-when-player-hit/158164/4
     Accessed: 2024-02-14
    ---------------------------------------*/
    private void SpriteBlinkingEffect()
    {
        spriteBlinkingTotalTimer += Time.deltaTime;
        if (spriteBlinkingTotalTimer >= spriteBlinkingTotalDuration)
        {
            isBlinking = false;
            spriteBlinkingTotalTimer = 0.0f;
            this.gameObject.GetComponent<SpriteRenderer>().enabled = true;   // according to 
                                                                             //your sprite
            return;
        }

        spriteBlinkingTimer += Time.deltaTime;
        if (spriteBlinkingTimer >= spriteBlinkingMiniDuration)
        {
            spriteBlinkingTimer = 0.0f;
            if (this.gameObject.GetComponent<SpriteRenderer>().enabled == true)
            {
                this.gameObject.GetComponent<SpriteRenderer>().enabled = false;  //make changes
            }
            else
            {
                this.gameObject.GetComponent<SpriteRenderer>().enabled = true;   //make changes
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
            PlayerStats.OnPlayerDeath -= Death;
            PlayerStats.OnGameOver -= Death;
            PlayerStats.OnPlayerHit -= PlayerHit;
            // Trigger some sound.
            deathSFX.Play();
            // Trigger visual effect.
            Instantiate(explosion, this.transform.position, this.transform.rotation);
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
        if (movement.magnitude != 0)
        {
            OnMovement?.Invoke(_playerId);
        }
        rigidBody.velocity = movement * activeSpeed;
    }

    void Update()
    {
        nextFire = Mathf.Max(nextFire - Time.deltaTime, 0);
        bool fireButton = Input.GetButton(fire1Name);
        bool bombButton = Input.GetButtonDown(fire2Name);
        bool slowMoButton = Input.GetButtonDown(fire3Name);

        iFrameCooldown = Mathf.Max(iFrameCooldown - Time.deltaTime, 0);
        if (iFrameCooldown == 0)
        {
            stats.UpdateIFrameActive(false);
        }

        if (isBlinking)
        {
            SpriteBlinkingEffect();
        }

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
                    OnShoot?.Invoke(_playerId);
                    spread.Create(firepoints[i].transform.position, firepoints[i].transform.rotation, Vector2.up);
                }
            }
        }
        if (bombButton)
        {
            if (stats.CurrentBombs > 0 && !bombIsActive)
            {
                OnBombUse?.Invoke(_playerId);
                bombSFX.Play();
                bombIsActive = true;
                stats.UpdateBombs(-1);
                GameObject b = Instantiate(bomb, transform.position, Quaternion.identity) as GameObject;
                b.transform.GetComponent<BombController>().playerId = _playerId;
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
            if (activeECDCooldown == 0 && _checkECD())
            {
                ECDready = true;
                _ECDStatus = ECDStatus.Ready;
            }
        }
        if (ECDready)
        {
            if (!_checkECD())
            {
                _ECDStatus = ECDStatus.Cooldown;
            }
            else
            {
                _ECDStatus = ECDStatus.Ready;
            }
            
        }

        updateECDGUI();
    }

    private bool _checkECD()
    {
        bool? nullableBool = CheckECD?.Invoke();
        return nullableBool.HasValue ? nullableBool.Value : false;
    }

    private void triggerSlowMo()
    {
        if (!(_checkECD()))
        {
            return;
        }
        ECDenabled = true;
        ECDready = false;
        activeSpeed = ECDSpeed;
        activeFireRate = ECDFireRate;
        _ECDStatus = ECDStatus.Active;
        if (!ECDParticles.isPlaying)
        {
            ECDParticles.Play();
        }
        ecdStartSFX.Play();
        OnTriggerECD?.Invoke(true);
        StartCoroutine(waitAndEndSlowMo(ECDDuration));
    }

    private IEnumerator waitAndEndSlowMo(float timeout)
    {
        //Wait until timeout seconds pass.
        ECDruntime = 0f;
        while (ECDruntime <= timeout)
        {
            yield return new WaitForFixedUpdate();
            ECDruntime += Time.deltaTime;
        }
        endSlowMo();
    }

    private void endSlowMo()
    {
        OnECD?.Invoke(_playerId);
        ECDenabled = false;
        _ECDStatus = ECDStatus.Charging;
        if (ECDParticles.isPlaying)
        {
            ECDParticles.Stop();
        }
        activeSpeed = speed;
        activeFireRate = fireRate;
        activeECDCooldown = ECDCooldown;
        OnTriggerECD?.Invoke(false);
    }

    private void updateECDGUI()
    {
        if (_ECDStatus == ECDStatus.Cooldown)
        {
            ECDSprite.color = ECDCharging;
            return;
        }
        if (_ECDStatus == ECDStatus.Active)
        {
            ECDSprite.color = ECDActive;
            ECDSprite.fillAmount = 1 - ((float)ECDruntime / (float)ECDDuration);
            return;
        }
        if (_ECDStatus == ECDStatus.Charging)
        {
            ECDSprite.color = ECDCharging;
            ECDSprite.fillAmount = 1 - ((float)activeECDCooldown / (float)ECDCooldown);
            return;
        }
        if (_ECDStatus == ECDStatus.Ready)
        {
            ECDSprite.color = ECDReady;
            ECDSprite.fillAmount = 1;
            return;
        }
    }

    private IEnumerator waitBombCooldown(float timeout)
    {
        yield return new WaitForSecondsRealtime(timeout);
        bombIsActive = false;
    }

}
