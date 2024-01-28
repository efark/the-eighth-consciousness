using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Straighter : AbstractEnemyController
{
    private Vector2 lastDirection;

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
                    shotFX.Play();
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
        hp = 100;
        UpdateGUI();
        targetType = TargetTypes.Player;

        // https://docs.unity3d.com/ScriptReference/Camera.ScreenToWorldPoint.html
        canFire = false;

        initScreenLimit();
        initFirepoints();
        initAttackPatterns();
    }

    void Update()
    {
        if (HP < 0)
        {
            isAlive = false;
            Destroy(gameObject);
        }
        // Reached Vertical limit of screen.
        canFire = screenLimit.Contains(transform.position);

        if (isAlive && canFire)
        {
            loopConstantAttacks();
            time += Time.fixedDeltaTime;
        }
    }

}
