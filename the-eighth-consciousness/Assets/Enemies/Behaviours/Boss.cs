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
        HP = 2000;
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

        foreach (KeyValuePair<int, (AbstractMovement, AttackPattern)> kvp in actionsMap)
        {
            Debug.Log(string.Format("Key = {0}, Item1 = {1}, Item2 = {2}", kvp.Key, kvp.Value.Item1, kvp.Value.Item2));
        }
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
        Debug.Log($"Triggering action: {actIndex}");
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

        /*----------------------------------------------
        S0 - Main Gun.

        M0 - Move forward.
        S1 - Lateral shots to the sides.

        M1 - Teleport.

        M2 - Move to the initial point.

        S2 - Shoot circling.
 
        S3 - Shoot forward.

        M3 - Move to the center of the screen.

        S4 - Lateral Shot 2x 3radial90 missiles.
        -----------------------------------------------*/
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
        //StartFire(ap, i);
        StartCoroutine(Burst(ap));
    }

    private IEnumerator Burst(AttackPattern ap) //, int index)
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
                //Debug.Log($"fPoints: {fPoints}, firepointType: {ap.firepointType}");
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

}
