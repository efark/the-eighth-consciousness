using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Straighter : AbstractEnemyController
{

    private TargetTypes targetType;
    //private GameObject[] players = new GameObject[2];
    //private GameObject targetPlayer;
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateGUI()
    {
        if (hp <= 0)
        {
            statsText.text = "";
            Destroy(gameObject);
        }
        statsText.text = $"Enemy HP: {hp}";
    }
}
