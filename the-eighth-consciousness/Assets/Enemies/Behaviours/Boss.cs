using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using TMPro;

public class Boss : AbstractEnemyController
{
    // public TMP_Text statsText;
    // public healthBar;

    private List<AbstractMovement> mvControllers = new List<AbstractMovement>();
    private int mvIndex;
    private int mvLastIndex;
    private int fIndex;
    private int actIndex;
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
        mvLastIndex = myResults.Length;

        for (int i = 0; i < mvLastIndex; i++)
        {
            foreach (AbstractMovement am in myResults)
            {
                if (am.order == i)
                {
                    mvControllers.Add(am);
                }
            }
        }

    }

    void Start()
    {
        HP = 2000;
        mvIndex = 0;
        fIndex = 0;
        actIndex = 0;
        actDone = false;
        targetType = TargetTypes.Player;

        initMovementControllers();
        initAttackPatterns();
        initFirepoints();
        initScreenLimit();
        initOnDeathEvent();
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
        S0 - Main - .

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
            actIndex++;
            actTriggered = false;
            actDone = false;
        }
        if (actTriggered && !actDone) // wait
        {
            // nothing
        }
        if (!actTriggered && !actDone) // trigger
        {
            // Trigger the action.
            if (actIndex == 0)
            {
                oneShotAttack(attackPatterns[fIndex]);
            }
            actTriggered = true;
        }


        // check if target == position.
        /*
        if (mvControllers[mvIndex].target == transform.position)
        {
            mvControllers[mvIndex].isEnabled = false;
            mvIndex = mvIndex == mvLastIndex ? 0 : mvIndex + 1;
            mvControllers[mvIndex].isEnabled = true;
        }
        */
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
                List<Vector3> fPoints = GetFirepoints(ap.firepointType);
                shotFX.PlayOneShot(shotFX.clip);
                foreach (Vector3 fp in fPoints)
                {
                    ap.spread.Create(transform.position + fp, transform.rotation, targetDirection);
                }
            }
        }
        yield return new WaitForSeconds(ap.cooldown);
        ap.UpdateIsRunning(false);
    }

}
