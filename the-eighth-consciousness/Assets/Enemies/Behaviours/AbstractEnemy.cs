using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

// Custom Event that will take three parameters (enemy instance Id, player id and points).
public class EnemyDeathEvent : UnityEvent<int, int, int> { }

public abstract class AbstractEnemyController : MonoBehaviour
{
    protected int hp;
    protected GameObject[] players = new GameObject[2];
    protected GameObject targetPlayer;
    protected List<Vector3> centralFirepoints = new List<Vector3>();
    protected List<Vector3> lateralFirepoints = new List<Vector3>();
    protected List<Vector3> allFirepoints = new List<Vector3>();

    protected TargetTypes targetType;
    public List<AttackPattern> attackPatternsValues = new List<AttackPattern>();
    protected List<AttackPattern> attackPatterns = new List<AttackPattern>();
    protected List<AttackPattern> constantAttackPatterns = new List<AttackPattern>();
    protected int currentOrder = 0;
    protected int maxOrder = 0;
    protected int simultaneousOneShots = 0;
    protected float time = 0;
    protected bool isAlive = true;
    protected bool canFire = false;
    public AudioSource shotFX;
    public GameObject explosion;
    public int points;

    protected Rect screenLimit;

    public EnemyDeathEvent OnDeath;

    public TMP_Text statsText;
    public abstract int HP
    {
        get;
        set;
    }

    protected void initOnDeathEvent()
    {
        if (OnDeath == null)
        {
            OnDeath = new EnemyDeathEvent();
        }
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
                centralFirepoints.Add(child.localPosition);
            }
            else
            {
                lateralFirepoints.Add(child.localPosition);
            }
            allFirepoints.Add(child.localPosition);
        }
    }

    protected List<Vector3> GetFirepoints(FirepointTypes ft)
    {
        if (ft == FirepointTypes.All)
        {
            return allFirepoints;
        }
        if (ft == FirepointTypes.Central)
        {
            return centralFirepoints;
        }
        return lateralFirepoints;
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

    protected void FireAll()
    {
        StartCoroutine(fireAll());
    }

    protected IEnumerator fireAll()
    {
        foreach (AttackPattern ap in attackPatterns)
        {
            players = GameObject.FindGameObjectsWithTag(targetType.ToString());
            targetPlayer = GetClosestPlayer();
            List<Vector3>  fPoints = GetFirepoints(ap.firepointType);
            Vector3 targetDir = new Vector3(0, -1, 0);
            if (targetPlayer != null)
            {
                targetDir = (targetPlayer.transform.position - this.transform.position).normalized;
            }
            Vector2 targetDirection = new Vector2(targetDir.x, targetDir.y);
            if (ap.isOpposite)
            {
                targetDirection *= -1;
            }
            foreach (Vector3 fp in fPoints)
            {
                for (int i = 0; i < ap.numberOfBursts; i++)
                {
                    for (int j = 0; j < ap.burstSize; j++)
                    {
                        if (j > 0)
                        {
                            yield return new WaitForSeconds(ap.burstSpacing);
                        }
                        shotFX.PlayOneShot(shotFX.clip);
                        ap.spread.Create(transform.position + fp, transform.rotation, targetDirection);
                    }
                }

            }   
        }
    }

    public void Hit(int playerId, int damage)
    {
        if (hp > 0 && hp + damage < 0)
        {
            OnDeath?.Invoke(this.GetInstanceID(), playerId, points);
        }
        hp += damage;
        if (hp <= 0)
        {
            if (explosion != null)
            {
                Instantiate(explosion, this.transform.position, this.transform.rotation);
            }
            Destroy(gameObject);
        }
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

    void OnDestroy()
    {
        OnDeath?.Invoke(this.GetInstanceID(), 0, 0);
    }
}
