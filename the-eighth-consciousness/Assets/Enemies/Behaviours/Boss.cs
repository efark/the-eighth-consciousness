using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : AbstractEnemyController
{
    private bool isAlive = true;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
