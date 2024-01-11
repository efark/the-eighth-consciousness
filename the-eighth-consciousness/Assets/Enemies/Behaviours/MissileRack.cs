using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MissileRack : AbstractEnemyController
{
    private bool isAlive = true;

    // public TMP_Text statsText;
    public bool isAimedAtPoint;
    public Vector2 targetPoint;
    public float distanceToTarget;
    public bool isTimeBased;
    public float explosionDelay;

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

    void fire()
    {
        foreach(AttackPattern ap in attackPatterns)
        {
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

    void Start()
    {
        time = 0f;
        hp = 500;
        UpdateGUI();
        targetType = TargetTypes.Player;

        HomingMovement mvController = this.GetComponent<HomingMovement>();
        GameObject target = AuxiliaryMethods.FindTarget(targetType.ToString(), this.transform.position);
        mvController.target = target;

        for (int i = 0; i < attackPatternsValues.Count; i++)
        {
            AttackPattern ap = attackPatternsValues[i];
            AttackPattern clone = Instantiate(ap);
            clone.Init(targetType);
            attackPatterns.Add(clone);
        }
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (isTimeBased)
        {
            if (time > explosionDelay)
            {
                fire();
                Destroy(gameObject);
            }
            return;
        }
        if (isAimedAtPoint)
        {
            if (Vector2.Distance(new Vector2(this.transform.position.x, this.transform.position.y), targetPoint) < distanceToTarget)
            {
                fire();
                Destroy(gameObject);
            }
            return;
        }
    }

}
