using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : AbstractEnemyController
{

    public int hp = 100;
    private TargetTypes targetType = TargetTypes.Player;
    private GameObject[] players = new GameObject[2];
    private GameObject targetPlayer;
    private bool isAlive = true;

    public List<AttackPattern> attackPatternsValues = new List<AttackPattern>();
    private List<AttackPattern> attackPatterns = new List<AttackPattern>();
    private int currentOrder = 0;
    private int maxOrder = 0;
    private int simultaneousOneShots = 0;

    public IEnumerator Burst(AttackPattern ap, int index)
    {
        Debug.Log("Here0!");
        // Wait for offset
        yield return new WaitForSeconds(ap.burstOffset);
        for (int i = 0; (ap.isConstantAttack || i < ap.numberOfBursts); i++)
        {
            Debug.Log("Here1!");
            for (int j = 0; j < ap.burstSize; j++)
            {
                Debug.Log("Here2!");
                if (i > 0)
                {
                    Debug.Log("Here3!");
                    yield return new WaitForSeconds(ap.burstSpacing);
                }
                Vector3 targetDirection = targetPlayer.transform.position - this.transform.position;
                ap.spread.Create(transform.position, transform.rotation, new Vector2(targetDirection.x, targetDirection.y));
            }
        }
        yield return new WaitForSeconds(ap.cooldown);
        simultaneousOneShots--;
        attackPatterns[index].UpdateIsRunning(false);
        if (simultaneousOneShots == 0)
        {
            currentOrder++;
        }
    }

    private void StartFire(AttackPattern ap, int index)
    {
        ap.UpdateIsRunning(true);
        StartCoroutine(Burst(ap, index));
        Debug.Log("Here4!");
        if (!ap.isConstantAttack)
        {
            simultaneousOneShots++;
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

    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        targetPlayer = GetClosestPlayer();

        for (int i = 0; i < attackPatternsValues.Count; i++)
        {
            AttackPattern ap = attackPatternsValues[i];
            AttackPattern clone = Instantiate(ap);
            clone.Init(targetType);
            attackPatterns.Add(clone);
            if (ap.order > maxOrder)
            {
                maxOrder = currentOrder;
            }
            Debug.Log($"{clone}");
        }

    }

    void Update()
    {

        if (isAlive)
        {
            for(int i = 0; i < attackPatterns.Count; i++)
            {
                AttackPattern ap = attackPatterns[i];
                if (ap.order == currentOrder)
                {
                    Debug.Log("Here99!");
                    if (ap.IsRunning)
                    {
                        continue;
                    }
                    StartFire(ap, i);
                }

            }
        }
        if (currentOrder > maxOrder)
        {
            currentOrder = 0;
        }

    }
}
