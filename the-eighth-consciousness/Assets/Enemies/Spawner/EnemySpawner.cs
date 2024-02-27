using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [Header("Settings")]
    public GameObject prefab;
    public SpreadSettings spreadSettings;
    [Tooltip("Number of waves that the spawner will generate.")]
    public int waves;
    [Tooltip("Wait before the spawner created the first wave.")]
    public float startDelay;
    [Tooltip("Time between waves.")]
    public float cooldown;
    [Tooltip("If isAimingPlayer is on, then the spawned enemy will start pointing in the direction of the player.")]
    public bool isAimingPlayer;
    [Tooltip("If hasFixedDirection is on, then the spawner will send the enemy in the predetermined direction.")]
    public bool hasFixedDirection;
    public Vector2 fixedDirection;
    public Vector2 fixedDirectionRange;
    [Tooltip("If hasFixedDirection is on, then the spawner will send the enemy in the predetermined direction.")]
    public bool hasTargetPoint;
    public Vector2 targetPoint;
    public Vector2 targetPointRange;

    private AbstractSpread spread;
    private float nextWave;
    private int waveNumber;
    private Vector2 direction;
    private GameObject[] players = new GameObject[2];
    private GameObject targetPlayer;


    /*---------------------------------------------------
    SpawnerSettings
    public GameObject prefab;
    public SpreadSettings spreadSettings;
    public int waves;
    public float startDelay;
    public float cooldown;
    public bool autoTarget;
    ---------------------------------------------------*/

    private GameObject GetClosestPlayer()
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

    void Start()
    {
        EnemyFactory ef = new EnemyFactory(prefab);
        this.spread = AuxiliaryMethods.InitSpread(ef, spreadSettings);
        nextWave = startDelay;
        waveNumber = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (nextWave <= 0)
        {
            if (hasFixedDirection)
            {
                direction = fixedDirection;
                direction.x += Random.Range(-fixedDirectionRange.x, fixedDirectionRange.x);
                direction.y += Random.Range(-fixedDirectionRange.y, fixedDirectionRange.y);
            }
            if (hasTargetPoint)
            {
                direction = targetPoint - new Vector2(this.transform.position.x, this.transform.position.y);
                direction.x += Random.Range(-targetPointRange.x, targetPointRange.x);
                direction.y += Random.Range(-targetPointRange.y, targetPointRange.y);
            }
            if (isAimingPlayer)
            {
                players = GameObject.FindGameObjectsWithTag(TargetTypes.Player.ToString());
                targetPlayer = GetClosestPlayer();
                if (targetPlayer != null)
                {
                    Vector3 dir3 = targetPlayer.transform.position - this.transform.position;
                    direction.x = dir3.x;
                    direction.y = dir3.y;
                }
                else 
                {
                    direction.x = 0;
                    direction.y = 0;
                }
            }
            Quaternion rota = Quaternion.LookRotation(Vector3.forward, direction);
            this.spread.Create(this.transform.position, rota, direction.normalized);
            nextWave = cooldown;
            waveNumber++;
        }
        nextWave -= Time.deltaTime;
        if (waves == waveNumber)
        {
            Destroy(gameObject);
        }
    }
}
