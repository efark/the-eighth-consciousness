using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Font 'Futura' downloaded from https://ttfonts.net/.
public class GameController : MonoBehaviour
{
    public PlayerStats statsPlayer1;
    public PlayerStats statsPlayer2;
    public GameObject playerPrefab1;
    public GameObject playerPrefab2;
    public Vector3 InitialPosition1;
    public Vector3 InitialPosition2;
    private bool mustRespawn1;
    private bool mustRespawn2;
    /*
    public void Init(bool hasPlayer2)
    {
        statsPlayer1.Init();
        if (hasPlayer2)
        {
            statsPlayer2.Init();
        }

    }
    */
    public void Start()
    {
        mustRespawn1 = false;
        mustRespawn2 = false;
        PlayerStats.OnPlayerDeath += FlagPlayerRespawn;
        // PlayerStats.OnGameOver += PlayerGameOver;
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
            // triggeredRespawn1 = false;
        }
        if (playerId == 2)
        {
            _playerSpawn(playerPrefab2, InitialPosition2, statsPlayer2);
            // triggeredRespawn2 = false;
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
        // GameObject pgo = 
        Instantiate(prefab, initialPosition, Quaternion.identity);

    }

}
