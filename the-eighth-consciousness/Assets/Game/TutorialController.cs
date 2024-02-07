using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    public PlayerStats statsPlayer1;
    public PlayerStats statsPlayer2;
    public GameObject playerPrefab1;
    public GameObject playerPrefab2;
    public Vector3 InitialPosition1;
    public Vector3 InitialPosition2;

    private bool hasMoved1;
    private bool hasMoved2;
    private bool hasFired1;
    private bool hasFired2;
    private bool hasBombed1;
    private bool hasBombed2;
    private bool hasECDed1;
    private bool hasECDed2;

    public void Start()
    {
        hasMoved1 = false;
        hasMoved2 = false;
        hasFired1 = false;
        hasFired2 = false;
        hasBombed1 = false;
        hasBombed2 = false;
        hasECDed1 = false;
        hasECDed2 = false;

        if (statsPlayer1.IsActive)
        {
            Instantiate(playerPrefab1, InitialPosition1, Quaternion.identity);
            statsPlayer1.UpdateIsAlive(true);
        }
        if (statsPlayer2.IsActive)
        {
            Instantiate(playerPrefab2, InitialPosition2, Quaternion.identity);
            statsPlayer2.UpdateIsAlive(true);
        }
    }

    void Update()
    {
        if (mustRespawn1)
        {
            mustRespawn1 = false;
            StartCoroutine(playerRespawn(1));
        }
        if (mustRespawn2)
        {
            mustRespawn2 = false;
            StartCoroutine(playerRespawn(2));
        }

        UpdateEnemyIds();
    }

    private Dictionary<int, GameObject> getEnemies()
    {
        GameObject[] currentEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        Dictionary<int, GameObject> mapEnemies = new Dictionary<int, GameObject>();
        foreach (GameObject i in currentEnemies)
        {
            mapEnemies.Add(i.transform.GetComponent<AbstractEnemyController>().GetInstanceID(), i);
        }
        return mapEnemies;
    }

    private void UpdateEnemyIds()
    {
        Dictionary<int, GameObject> currentEnemies = getEnemies();
        // Add new enemies.
        foreach (KeyValuePair<int, GameObject> cekv in currentEnemies)
        {
            bool found = false;
            foreach (KeyValuePair<int, GameObject> kv in enemies)
            {
                if (cekv.Key == kv.Key)
                {
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                enemies.Add(cekv.Key, cekv.Value);
                cekv.Value.transform.GetComponent<AbstractEnemyController>().OnDeath.AddListener(EnemyDeath);
            }
        }
    }

    private void EnemyDeath(int enemyId, int playerId, int points)
    {
        enemies[enemyId].transform.GetComponent<AbstractEnemyController>().OnDeath.RemoveListener(EnemyDeath);
        enemies.Remove(enemyId);
        if (playerId == 1)
        {
            statsPlayer1.UpdateScore(points);
        }
        if (playerId == 2)
        {
            statsPlayer2.UpdateScore(points);
        }
    }

    public void FlagPlayerRespawn(int playerId)
    {
        if (playerId == 1)
        { 
            mustRespawn1 = true;
        }
        if (playerId == 2)
        {
            mustRespawn2 = true;
        }
    }

    private IEnumerator playerRespawn(int playerId)
    {
        yield return new WaitForSecondsRealtime(1.0f);
        if (playerId == 1)
        {
            _playerSpawn(playerPrefab1, InitialPosition1, statsPlayer1);
        }
        if (playerId == 2)
        {
            _playerSpawn(playerPrefab2, InitialPosition2, statsPlayer2);
        }
    }

    private void _playerSpawn(GameObject prefab, Vector3 initialPosition, PlayerStats stats)
    {
        // Assign playerStats.
        stats.UpdateIsAlive(true);
        stats.UpdateIFrameActive(false);
        stats.UpdateLives(-1);
        stats.SetFullHP();
        // Instantiate prefab.
        Instantiate(prefab, initialPosition, Quaternion.identity);

    }

}
