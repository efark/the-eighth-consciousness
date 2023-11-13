using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Rigidbody2D rigidBody;

    public int hp;
    public int bombs;
    public int damage;
    public float fireRate;
    public int bulletSpeed;

    public float rotationSpeed = 1f;
    public GameObject bullet;
    private GameObject[] players = new GameObject[2];
    public List<Transform> firepoints = new List<Transform>();

    private float nextFire;
    private string targetType = "Player";

    private GameObject targetPlayer;

    public List<AttackPattern> constantAttacks = new List<AttackPattern>();
    public List<AttackPattern> oneShotAttacks = new List<AttackPattern>();

    private AttackPattern InitAttackPattern(AttackPattern ap)
    {
        ap.shotSpread = ExtensionMethods.InitShotSpread(ap.spreadSettings, targetType, 0);
        ap.shotBurst = new ShotBurst(ap.burstSettings.offset, ap.burstSettings.size, ap.burstSettings.fireRate, ap.shotSpread);
        return ap;
    }

    private IEnumerator constantFire(AttackPattern ap)
    {
        while (true)
        {
            Vector3 targetDirection = targetPlayer.transform.position - this.transform.position;
            //Debug.DrawRay(transform.position, targetDirection, Color.red, 10f);
            StartCoroutine(ap.shotBurst.Fire(transform.position, Quaternion.identity, new Vector2(targetDirection.x, targetDirection.y)));
            yield return new WaitForSeconds(ap.cooldown);
        }
    }

    private void StartConstantFire(AttackPattern ap)
    {
        StartCoroutine(constantFire(ap));
    }

    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        rigidBody = transform.GetComponent<Rigidbody2D>();
        nextFire = 5 / fireRate;
        targetPlayer = GetClosestPlayer();
        for(int i = 0; i < constantAttacks.Count; i++)
        {
            constantAttacks[i] = InitAttackPattern(constantAttacks[i]);
            StartConstantFire(constantAttacks[i]);
        }
    }


    private GameObject GetClosestPlayer()
    {
        if (players.Length == 1)
        {
            return players[0];
        }
        float min_distance = 0;
        GameObject closest = null;
        foreach (GameObject p in players)
        {
            float dist = Vector3.Distance(p.transform.position, gameObject.transform.position);
            if (min_distance == 0)
            {
                min_distance = dist;
                closest = p;
                continue;
            }
            if (dist < min_distance)
            {
                min_distance = dist;
                closest = p;
                continue;
            }
        }
        return closest;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
