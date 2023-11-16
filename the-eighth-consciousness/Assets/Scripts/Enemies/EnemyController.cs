using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : AbstractEnemyController
{
    //private Rigidbody2D rigidBody;

    public int hp = 100;
    private TargetTypes targetType = TargetTypes.Player;
    private GameObject[] players = new GameObject[2];
    private GameObject targetPlayer;
    private bool isAlive = true;

    public List<AttackPattern> attackPatterns = new List<AttackPattern>();
    private int currentOrder = 0;
    private int lastStarted = -1;
    private int maxOrder = 0;
    private int simultaneousOneShots = 0;

    private IEnumerator fire(AttackPattern ap)
    {
        int count = 0;
        while (ap.isActive)
        {
            Vector3 targetDirection = targetPlayer.transform.position - this.transform.position;
            //Debug.DrawRay(transform.position, targetDirection, Color.red, 10f);
            StartCoroutine(ap.burst.Fire(transform.position, Quaternion.identity, new Vector2(targetDirection.x, targetDirection.y)));
            yield return new WaitForSeconds(ap.cooldown);
            if (!ap.isConstantAttack)
            {
                simultaneousOneShots++;
                count++;
                if (count >= ap.numberOfBursts)
                {
                    ap.isActive = false;
                    ap.isRunning = false;
                    simultaneousOneShots--;
                    if (simultaneousOneShots == 0)
                    {
                        currentOrder++;
                    }
                }
            }
        }
    }

    private void StartFire(AttackPattern ap)
    {
        ap.isActive = true;
        ap.isRunning = true;
        StartCoroutine(fire(ap));
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

    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        targetPlayer = GetClosestPlayer();
        //rigidBody = transform.GetComponent<Rigidbody2D>();
        //nextFire = 5 / fireRate;

    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {
            foreach (AttackPattern ap in attackPatterns)
            {
                if (currentOrder > lastStarted)
                {
                    if (ap.order == currentOrder)
                    {
                        if (ap.isRunning)
                        {
                            continue;
                        }

                        ap.Init(targetType);
                        StartFire(ap);
                    }

                }
                if (ap.order > maxOrder)
                { 
                    maxOrder = ap.order;
                }
            }
            lastStarted = currentOrder;
        }
        if (currentOrder > maxOrder)
        {
            currentOrder = 0;
            lastStarted = -1;
        }

    }
}
