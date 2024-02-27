using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using TMPro;

public class Boss : AbstractEnemyController
{
    // public TMP_Text statsText;
    // public healthBar;

    private List<AbstractMovement> mvControllers = new List<AbstractMovement>();
    private Dictionary<int, (AbstractMovement, AttackPattern)> actionsMap = new Dictionary<int, (AbstractMovement, AttackPattern)>();
    private AbstractMovement currentMovement;
    private AttackPattern currentAttack;
    private bool mvDone, attackDone;
    private int actIndex;
    private int maxAction;
    private bool actTriggered, actDone;
    private int maxHP;

    public static event Action<float> UpdateHPBar;

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

    new public void Hit(int playerId, int damage)
    {
        if (hp > 0 && hp + damage <= 0)
        {
            OnDeath?.Invoke(this.GetInstanceID(), playerId, points);
        }
        hp += damage;
        UpdateHPBar?.Invoke((float)hp / (float)maxHP);
        if (hp <= 0)
        {
            if (explosion != null)
            {
                Instantiate(explosion, this.transform.position, this.transform.rotation);
            }
            Destroy(gameObject);
        }
    }

    private void initMovementControllers()
    {
        AbstractMovement[] myResults = GetComponents<AbstractMovement>();
        foreach (AbstractMovement am in myResults)
        {
            mvControllers.Add(am);
        }
    }

    private void mapActions()
    {
        for (int i = 0; i <= maxAction; i++)
        {
            AbstractMovement tempAM = null;
            AttackPattern tempAP = null;
            foreach (AbstractMovement am in mvControllers)
            {
                if (am.order == i)
                {
                    tempAM = am;
                    break;
                }
            }
            foreach (AttackPattern ap in attackPatterns)
            {
                if (ap.order == i)
                {
                    tempAP = ap;
                    break;
                }
            }
            actionsMap[i] = (tempAM, tempAP);
        }
    }

    void getMaxOrders()
    {
        int maxMvOrder, maxAtkOrder;
        maxMvOrder = 0;
        maxAtkOrder = 0;
        foreach (AbstractMovement am in mvControllers)
        { 
            maxMvOrder = maxMvOrder < am.order ? am.order : maxMvOrder;
        }

        foreach (AttackPattern ap in attackPatterns)
        {
            maxAtkOrder = maxAtkOrder < ap.order ? ap.order : maxAtkOrder;
        }
        maxAction = Mathf.Max(maxMvOrder, maxAtkOrder);
    }

    void Start()
    {
        hp = 2500;
        maxHP = 2500;
        actIndex = 0;
        actDone = false;
        targetType = TargetTypes.Player;

        actTriggered = false;
        actDone = false;

        initMovementControllers();
        initAttackPatterns();
        getMaxOrders();
        mapActions();
        initFirepoints();
        initScreenLimit();
        initOnDeathEvent();
    }

    private void checkActionStatus()
    {
        actDone = mvDone && attackDone;
    }

    private void triggerMovement()
    {
        currentMovement.IsActive = true;
        currentMovement.enabled = true;
    }

    private void stopMovement()
    {
        if (mvDone)
        {
            return;
        }
        mvDone = (Vector2.Distance(currentMovement.destination, new Vector2(transform.position.x, transform.position.y)) < 0.5f);
        if (mvDone)
        {
            currentMovement.IsActive = false;
        }
    }

    private void triggerAttack()
    {
        oneShotAttack(currentAttack);
    }


    private void triggerAction()
    {
        (AbstractMovement, AttackPattern) actionTuple = actionsMap[actIndex];
        currentMovement = actionTuple.Item1;
        currentAttack = actionTuple.Item2;
        mvDone = true;
        attackDone = true;
        if (currentMovement != null)
        {
            mvDone = false;
            triggerMovement();
        }
        if (currentAttack != null)
        {
            attackDone = false;
            triggerAttack();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (HP < 0)
        {
            isAlive = false;
            Destroy(gameObject);
        }

        if (actTriggered && actDone) // move to the next.
        {
            actIndex = actIndex < maxAction ? actIndex + 1 : 0;
            actTriggered = false;
            actDone = false;
        }
        if (!actTriggered && !actDone) // trigger
        {
            // Trigger the action.
            triggerAction();
            actTriggered = true;
            actDone = false;
        }

        stopMovement();
        checkActionStatus();

    }

    private void oneShotAttack(AttackPattern ap)
    {
        if (ap.IsRunning)
        {
            return;
        }
        ap.UpdateIsRunning(true);
        StartCoroutine(Burst(ap));
    }

    private IEnumerator Burst(AttackPattern ap)
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
                Vector2 targetDirection = new Vector2(0, -1);
                if (ap.hasFixedDirection)
                { 
                    targetDirection = ap.fixedDirection;
                }
                if (!ap.hasFixedDirection)
                {
                    targetPlayer = GetClosestPlayer();
                    if (targetPlayer != null)
                    {
                        Vector3 targetDir = (targetPlayer.transform.position - this.transform.position).normalized;
                        targetDirection = new Vector2(targetDir.x, targetDir.y);
                    }
                }
                if (ap.isOpposite)
                {
                    targetDirection *= -1;
                }
                List<Vector3> fPoints = GetFirepoints(ap.firepointType);
                shotFX.PlayOneShot(shotFX.clip);
                foreach (Vector3 fp in fPoints)
                {
                    if (ap.firepointType == FirepointTypes.Lateral)
                    {
                        if (fp.x > 0)
                        {
                            targetDirection *= -1;
                        }
                    }
                    ap.spread.Create(transform.position + fp, transform.rotation, targetDirection);
                }
            }
        }
        yield return new WaitForSeconds(ap.cooldown);
        ap.UpdateIsRunning(false);
        attackDone = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.ToLower() == "player")
        {
            other.transform.GetComponent<PlayerController>().stats.UpdateHP(-50);
            return;
        }
    }
}
