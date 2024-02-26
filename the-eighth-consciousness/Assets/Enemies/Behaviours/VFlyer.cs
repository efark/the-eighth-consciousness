using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFlyer : AbstractEnemyController
{
    public float thresholdY = 0f;
    public float thresholdRange = 1.5f;

    private bool hasFired = false;
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
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        hp = 30;
        targetType = TargetTypes.Player;
        thresholdY += Random.Range(-thresholdRange, thresholdRange);

        initMovementController();
        initAttackPatterns();
        initFirepoints();
        initScreenLimit();
        initWorldLimit();
        initOnDeathEvent();
        transform.rotation = Quaternion.LookRotation(Vector3.forward, mvController.direction);
    }

    void Update()
    {
        CheckEnteredScreen();
        CheckOutOfWorld();
        if (HP < 0)
        {
            isAlive = false;
            Destroy(gameObject);
        }
        canFire = screenLimit.Contains(transform.position);

        if (canFire && Mathf.Abs(this.transform.position.y - thresholdY) < 1 && !hasFired)
        {
            hasFired = true;
            FireAll();
        }
        if (this.transform.position.y <= thresholdY && !hasTurned)
        {
            mvController.direction.y *= -1;
            hasTurned = true;
            transform.rotation = Quaternion.LookRotation(Vector3.forward, mvController.direction);
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
                shotFX.PlayOneShot(shotFX.clip);
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
            ap.UpdateIsRunning(false);
            constantAttackPatterns[index] = ap;
        }
    }
}
