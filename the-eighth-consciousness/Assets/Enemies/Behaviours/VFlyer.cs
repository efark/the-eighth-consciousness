﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFlyer : AbstractEnemyController
{
    public float thresholdY = 0f;
    public float thresholdRange = 1.5f;

    public List<AttackPattern> attackPatternsValues = new List<AttackPattern>();
    private List<AttackPattern> attackPatterns = new List<AttackPattern>();
    private List<AttackPattern> constantAttackPatterns = new List<AttackPattern>();
    private int currentOrder = 0;
    private int maxOrder = 0;
    private int simultaneousOneShots = 0;

    private TargetTypes targetType;
    private GameObject[] players = new GameObject[2];
    private GameObject targetPlayer;
    private AbstractMovement mvController;
    private bool hasTurned = false;
    public override int HP
    {
        get
        {
            return hp;
        }
        set
        {
            hp += value;
            //UpdateGUI();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        hp = 500;
        targetType = TargetTypes.Player;
        mvController = this.GetComponent<AbstractMovement>();
        thresholdY += Random.Range(-thresholdRange, thresholdRange);

        for (int i = 0; i < attackPatternsValues.Count; i++)
        {
            AttackPattern ap = attackPatternsValues[i];
            AttackPattern clone = Instantiate(ap);
            clone.Init(targetType);
            if (ap.isConstantAttack)
            {
                constantAttackPatterns.Add(clone);
            }
            if (!ap.isConstantAttack)
            {
                attackPatterns.Add(clone);
            }
            if (ap.order > maxOrder)
            {
                maxOrder = ap.order;
            }
        }
        Debug.Log($"attackPatterns: {attackPatterns[0]}");
    }

    void Update()
    {
        loopConstantAttacks();
        if (this.transform.position.y <= thresholdY && !hasTurned)
        {
            mvController.direction.y *= -1;
            hasTurned = true;
            loopOneShotAttacks();
        }
            
    }



    private void loopOneShotAttacks()
    {
        for (int i = 0; i < attackPatterns.Count; i++)
        {
            AttackPattern ap = attackPatterns[i];
            if (ap.order == currentOrder)
            {
                if (ap.IsRunning)
                {
                    continue;
                }
                StartFire(ap, i);
            }

        }
    }

    private void loopConstantAttacks()
    {
        for (int i = 0; i < constantAttackPatterns.Count; i++)
        {
            StartCoroutine(constantAttack(i));
        }
    }

    public IEnumerator Burst(AttackPattern ap, int index)
    {
        // Wait for offset
        yield return new WaitForSeconds(ap.burstOffset);
        for (int i = 0; i < ap.numberOfBursts; i++)
        {
            for (int j = 0; j < ap.burstSize; j++)
            {
                if (j > 0)
                {
                    yield return new WaitForSeconds(ap.burstSpacing);
                }
                players = GameObject.FindGameObjectsWithTag(targetType.ToString());
                Vector2 targetDirection = new Vector2(0, 0);
                targetPlayer = GetClosestPlayer();
                if (targetPlayer != null)
                {
                    Vector3 targetDir = (targetPlayer.transform.position - this.transform.position).normalized;
                    targetDirection = new Vector2(targetDir.x, targetDir.y);
                    if (ap.isOpposite)
                    {
                        targetDirection *= -1;
                    }
                }
                ap.spread.Create(transform.position, transform.rotation, targetDirection);
            }
        }
        yield return new WaitForSeconds(ap.cooldown);
        simultaneousOneShots--;
        attackPatterns[index].UpdateIsRunning(false);
        if (simultaneousOneShots == 0)
        {
            currentOrder = currentOrder + 1 > maxOrder ? 0 : currentOrder + 1;
        }
    }

    private void StartFire(AttackPattern ap, int index)
    {
        ap.UpdateIsRunning(true);
        if (!ap.isConstantAttack)
        {
            simultaneousOneShots++;
            StartCoroutine(Burst(ap, index));
        }
    }

    private IEnumerator constantAttack(int index)
    {
        AttackPattern ap = constantAttackPatterns[index];
        ap.UpdateNextFire(-Time.deltaTime);
        if (ap.NextFire <= 0)
        {
            ap.UpdateNextFire(ap.cooldown);
            ap.UpdateIsRunning(true);

            yield return new WaitForSeconds(ap.burstOffset);
            for (int i = 0; i < ap.numberOfBursts; i++)
            {
                for (int j = 0; j < ap.burstSize; j++)
                {
                    if (j > 0)
                    {
                        yield return new WaitForSeconds(ap.burstSpacing);
                    }
                    players = GameObject.FindGameObjectsWithTag(targetType.ToString());
                    targetPlayer = GetClosestPlayer();
                    if (targetPlayer == null)
                    {
                        continue;
                    }
                    Vector3 targetDir = (targetPlayer.transform.position - this.transform.position).normalized;
                    Vector2 targetDirection = new Vector2(targetDir.x, targetDir.y);
                    if (ap.isOpposite)
                    {
                        targetDirection *= -1;
                    }
                    ap.spread.Create(transform.position, transform.rotation, targetDirection);
                }
            }
            // yield return new WaitForSeconds(ap.cooldown);
            ap.UpdateIsRunning(false);
            constantAttackPatterns[index] = ap;
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
}