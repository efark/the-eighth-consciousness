using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : AbstractEnemyController
{
    private TargetTypes targetType;
    private bool isAlive = true;

    // public TMP_Text statsText;
    private Vector3 centralFirepoint;
    private List<Vector3> lateralFirepoints = new List<Vector3>();
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

    private List<Vector3> GetFirepoints(FirepointTypes ft)
    {
        List<Vector3> temp = new List<Vector3>();
        if (ft == FirepointTypes.All || ft == FirepointTypes.Lateral)
        {
            foreach (Vector3 fp in lateralFirepoints)
            {
                temp.Add(fp);
            }
        }
        if (ft == FirepointTypes.All || ft == FirepointTypes.Central)
        {
            temp.Add(centralFirepoint);
        }
        return temp;
    }

    void Start()
    {
        Transform firepoints = this.transform.Find("Firepoints");
        foreach (GameObject child in firepoints)
        {
            if (child.name.ToLower() == "central")
            {
                centralFirepoint = child.transform.position;
            }
            else
            {
                lateralFirepoints.Add(child.transform.position);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
