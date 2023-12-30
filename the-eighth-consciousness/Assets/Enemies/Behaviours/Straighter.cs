using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Straighter : AbstractEnemyController
{

    private TargetTypes targetType;
    public List<AttackPattern> attackPatternsValues = new List<AttackPattern>();
    private List<AttackPattern> attackPatterns = new List<AttackPattern>();
    private Vector2 lastDirection;
    private bool isAlive = true;

    // public TMP_Text statsText;
    private float time;
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

    private IEnumerator constantAttack(int index)
    {
        AttackPattern ap = attackPatterns[index];
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
                    if (targetPlayer != null)
                    {
                        Vector3 targetDir = (targetPlayer.transform.position - this.transform.position).normalized;
                        Vector2 targetDirection = new Vector2(targetDir.x, targetDir.y);
                        if (ap.isOpposite)
                        {
                            targetDirection *= -1;
                        }
                        lastDirection = targetDirection;
                    }

                    ap.spread.Create(transform.position, transform.rotation, lastDirection);
                }
            }
            // yield return new WaitForSeconds(ap.cooldown);
            ap.UpdateIsRunning(false);
            attackPatterns[index] = ap;
        }
    }

    private void loopConstantAttacks()
    {
        for (int i = 0; i < attackPatterns.Count; i++)
        {
            StartCoroutine(constantAttack(i));
        }
    }

    void Start()
    {
        time = 0f;
        hp = 500;
        UpdateGUI();
        targetType = TargetTypes.Player;

        for (int i = 0; i < attackPatternsValues.Count; i++)
        {
            AttackPattern ap = attackPatternsValues[i];
            AttackPattern clone = Instantiate(ap);
            clone.Init(targetType);
            attackPatterns.Add(clone);
        }

    }

    void Update()
    {
        if (isAlive)
        {
            loopConstantAttacks();
        }
        time += Time.fixedDeltaTime;
    }

}
