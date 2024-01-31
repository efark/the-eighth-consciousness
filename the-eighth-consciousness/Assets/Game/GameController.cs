using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

// Font 'Futura' downloaded from https://ttfonts.net/.
public class GameController : MonoBehaviour
{
    public PlayerStats statsPlayer1;
    public PlayerStats statsPlayer2;
    public GameObject playerPrefab1;
    public GameObject playerPrefab2;
    public Vector3 InitialPosition1;
    public Vector3 InitialPosition2;
    public AudioSource deathSFX;
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
        PlayerStats.OnPlayerDeath += Respawn;
        if (statsPlayer1.IsActive)
        {
            Instantiate(playerPrefab1, InitialPosition1, Quaternion.identity);
        }
        if (statsPlayer2.IsActive)
        {
            Instantiate(playerPrefab2, InitialPosition2, Quaternion.identity);
        }
    }

    public void OnDestroy()
    {
        PlayerStats.OnPlayerDeath -= Respawn;
    }

    public void Respawn(int playerId)
    {
        StartCoroutine(playerDeath(playerId));
    }

    private IEnumerator playerDeath(int playerId)
    {
        deathSFX.Play();
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
        if (stats.IsActive)
        {
            // Instantiate prefab.
            GameObject pgo = Instantiate(prefab, initialPosition, Quaternion.identity) as GameObject;
            // Get PlayerController component.
            // PlayerController pc = pgo.GetComponent<PlayerController>();
            // Assign playerStats.
            stats.SetFullHP();
        }
    }

}
