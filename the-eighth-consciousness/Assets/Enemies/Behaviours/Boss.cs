using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : AbstractEnemyController
{
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

    void Start()
    {
        initFirepoints();
        initAttackPatterns();
    }

    // Update is called once per frame
    void Update()
    {
        if (HP < 0)
        {
            isAlive = false;
            Destroy(gameObject);
        }
    }
}
