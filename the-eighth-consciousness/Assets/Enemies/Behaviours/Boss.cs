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
        mvIndex = 0;
        fIndex = 0;

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
    }
}
