using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MissileRack : AbstractEnemyController
{
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

    private void checkAttackPatterns()
    {
        List<string> invalidAPs = new List<string>();
        foreach (AttackPattern ap in attackPatternsValues)
        {
            if (ap.isConstantAttack)
            { 
                invalidAPs.Add(ap.name);
            }
        }
        if (invalidAPs.Count > 0)
        {
            Debug.LogError($"Invalid Attack Patterns - can not be constant: {invalidAPs.ToString()}");
        }
        
    }

    void Start()
    {
        time = 0f;
        hp = 60;
        //UpdateGUI();
        targetType = TargetTypes.Player;

        HomingMovement mvController = this.GetComponent<HomingMovement>();
        GameObject target = AuxiliaryMethods.FindTarget(targetType.ToString(), this.transform.position);
        mvController.target = target;

        initAttackPatterns();
        checkAttackPatterns();
        initFirepoints();
        initOnDeathEvent();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (HP < 0)
        {
            isAlive = false;
            Destroy(gameObject);
        }
        if (isTimeBased)
        {
            if (time > explosionDelay)
            {
                FireAll();
                Destroy(gameObject);
            }
            return;
        }
        if (isAimedAtPoint)
        {
            if (Vector2.Distance(new Vector2(this.transform.position.x, this.transform.position.y), targetPoint) < distanceToTarget)
            {
                FireAll();
                Destroy(gameObject);
            }
            return;
        }
    }

}
