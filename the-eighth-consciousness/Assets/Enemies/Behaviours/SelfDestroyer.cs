using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroyer : AbstractEnemyController
{
    public GameObject bomb;
    public float proximity = 0.5f;
    private Vector3 target;

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
        hp = 100;
        UpdateGUI();
        targetType = TargetTypes.Player;
        targetPlayer = GetClosestPlayer();


        // https://docs.unity3d.com/ScriptReference/Camera.ScreenToWorldPoint.html
        initMovementController();
        initScreenLimit();
        initAttackPatterns();
        initOnDeathEvent();

        if (targetPlayer != null)
        {
            target = targetPlayer.transform.position;
            Vector3 targetDir = (target - this.transform.position).normalized;
            mvController.direction = new Vector2(targetDir.x, targetDir.y);
        }
        
    }

    void Update()
    {
        if (HP < 0)
        {
            isAlive = false;
            Destroy(gameObject);
        }

        if (isAlive && Vector3.Distance(this.transform.position, target) < proximity)
        {
            GameObject b = Instantiate(bomb, transform.position, Quaternion.identity) as GameObject;
            b.transform.GetComponent<BombController>().playerId = 0;
            b.transform.GetComponent<BombController>().damage = 50;
            b.transform.GetComponent<BombController>().ttl = 1f;
            b.transform.GetComponent<BombController>().scaleFactor = 1;
            Destroy(gameObject);
        }
    }

}
