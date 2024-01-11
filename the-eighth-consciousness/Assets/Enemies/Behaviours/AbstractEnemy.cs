using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class AbstractEnemyController : MonoBehaviour
{
    protected int hp;
    protected GameObject[] players = new GameObject[2];
    protected GameObject targetPlayer;
    protected bool hasCentralFirepoint = false;
    protected Vector3 centralFirepoint;
    protected List<Vector3> lateralFirepoints = new List<Vector3>();

    protected TargetTypes targetType;
    public List<AttackPattern> attackPatternsValues = new List<AttackPattern>();
    protected List<AttackPattern> attackPatterns = new List<AttackPattern>();
    protected List<AttackPattern> constantAttackPatterns = new List<AttackPattern>();
    protected int currentOrder = 0;
    protected int maxOrder = 0;
    protected int simultaneousOneShots = 0;
    protected float time = 0;

    protected Rect screenLimit;

    public TMP_Text statsText;
    public abstract int HP
    {
        get;
        set;
    }

    protected void initScreenLimit()
    {
        Camera cam = Camera.main;
        Vector3 bottomLeft = cam.ScreenToWorldPoint(Vector3.zero);
        Vector3 topRight = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, cam.pixelHeight));

        screenLimit = new Rect(
            bottomLeft.x,
            bottomLeft.y,
            topRight.x - bottomLeft.x,
            topRight.y - bottomLeft.y);
    }

    protected void initFirepoints()
    {
        Transform firepoints = this.transform.Find("Firepoints");
        if (firepoints == null)
        {
            return;
        }
        foreach (Transform child in firepoints)
        {
            if (child.name.ToLower() == "central")
            {
                centralFirepoint = child.position;
                hasCentralFirepoint = true;
            }
            else
            {
                lateralFirepoints.Add(child.transform.position);
            }
        }
    }

    protected List<Vector3> GetFirepoints(FirepointTypes ft)
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

    protected void initAttackPatterns()
    {
        for (int i = 0; i < attackPatternsValues.Count; i++)
        {
            AttackPattern ap = attackPatternsValues[i];
            AttackPattern clone = Instantiate(ap);
            clone.Init(targetType);
            if (ap.isConstantAttack)
            {
                constantAttackPatterns.Add(clone);
            }
            if (!ap.isConstantAttack)
            {
                attackPatterns.Add(clone);
            }

            if (ap.order > maxOrder)
            {
                maxOrder = ap.order;
            }
        }
    }


    protected GameObject GetClosestPlayer()
    {
        if (players.Length == 1)
        {
            return players[0];
        }
        float min_distance = 0;
        GameObject closest = null;
        foreach (GameObject p in players)
        {
            float dist = Vector3.Distance(p.transform.position, gameObject.transform.position);
            if (min_distance == 0)
            {
                min_distance = dist;
                closest = p;
                continue;
            }
            if (dist < min_distance)
            {
                min_distance = dist;
                closest = p;
                continue;
            }
        }
        return closest;
    }

    public void UpdateGUI()
    {
        if (statsText == null)
        {
            return;
        }
        if (hp <= 0)
        {
            statsText.text = "";
            Destroy(gameObject);
        }
        statsText.text = $"Enemy HP: {hp}";
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
