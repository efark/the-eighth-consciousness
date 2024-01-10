using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : AbstractEnemyController
{
    private TargetTypes targetType;
    private bool isAlive = true;

    // public TMP_Text statsText;
    public List<AttackPattern> attackPatternsValues = new List<AttackPattern>();
    private List<AttackPattern> attackPatterns = new List<AttackPattern>();
    private List<AttackPattern> constantAttackPatterns = new List<AttackPattern>();
    private int currentOrder = 0;
    private int maxOrder = 0;
    private int simultaneousOneShots = 0;

    private float time;
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
