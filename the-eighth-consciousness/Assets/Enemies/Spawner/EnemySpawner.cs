using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public SpawnerSettings settings;
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
        EnemyFactory ef = new EnemyFactory(settings.prefab);
        this.spread = AuxiliaryMethods.InitSpread(ef, settings.spreadSettings);
        nextWave = settings.startDelay;
        waveNumber = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (nextWave <= 0)
        {
            if (!settings.autoTarget)
            {
                players = GameObject.FindGameObjectsWithTag(TargetTypes.Player.ToString());
                targetPlayer = GetClosestPlayer();
                if (targetPlayer != null)
                {
                    Vector3 dir3 = targetPlayer.transform.position - this.transform.position;
                    direction.x = dir3.x;
                    direction.y = dir3.y;
                }
                
            }
            // Create(Vector3 startPosition, Quaternion rotation, Vector2 direction
            Debug.Log($"waveNumber: {waveNumber}");
            this.spread.Create(this.transform.position, this.transform.rotation, direction);
            nextWave = settings.cooldown;
            waveNumber++;
        }
        nextWave -= Time.deltaTime;
        if (settings.waves == waveNumber)
        {
            Destroy(gameObject);
        }
    }
}
