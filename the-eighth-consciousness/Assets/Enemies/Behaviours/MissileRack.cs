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

        initFirepoints();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (isTimeBased)
        {
            if (time > explosionDelay)
            {
                Debug.Log("Firing by time.");
                FireAll();
                Destroy(gameObject);
            }
            return;
        }
        if (isAimedAtPoint)
        {
            if (Vector2.Distance(new Vector2(this.transform.position.x, this.transform.position.y), targetPoint) < distanceToTarget)
            {
                Debug.Log("Firing by distance.");
                FireAll();
                Destroy(gameObject);
            }
            return;
        }
    }

}
