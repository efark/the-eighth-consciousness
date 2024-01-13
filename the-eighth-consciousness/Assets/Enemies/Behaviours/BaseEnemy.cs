using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BaseEnemy : AbstractEnemyController
{
    
    // private GameObject[] players = new GameObject[2];
    // private GameObject targetPlayer;
    private bool isAlive = true;

    // public TMP_Text statsText;

    public override int HP
    {
        get
        {
            return hp;
        }
        set
        {
            hp += value;
            UpdateGUI();
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

    void Start()
    {
        time = 0f;
        hp = 500;
        UpdateGUI();
        targetType = TargetTypes.Player;

        initAttackPatterns();
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

    void Update()
    {
        if (isAlive)
        {
            //Debug.Log($"time: {time} x = {Mathf.Sin(time)} - y = {Mathf.Cos(time)}");
            loopOneShotAttacks();
            loopConstantAttacks();
        }
        time += Time.fixedDeltaTime;
    }

}
