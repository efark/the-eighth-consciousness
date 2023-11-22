using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Font 'Futura' downloaded from https://ttfonts.net/.
public class GameController : MonoBehaviour
{
    public TMP_Text statsText1;
    public TMP_Text statsText2;
    public PlayerStats statsPlayer1;
    public PlayerStats statsPlayer2;
    public bool isActivePlayer1;
    public bool isActivePlayer2;
    public GameObject playerPrefab1;
    public GameObject playerPrefab2;
    public Vector3 InitialPosition1;
    public Vector3 InitialPosition2;

    // Start is called before the first frame update
    void Start()
    {
        PlayerStats.OnPlayerHPChange += UpdatePlayerStats;
        PlayerStats.OnPlayerDeath += Respawn;
        PlayerStats.OnGameOver += GameOver;

        statsPlayer1.Init();

        UpdatePlayerStats(1);
        //UpdatePlayerStats(2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Respawn(int playerId)
    {
        if (playerId == 1)
        {
            if (statsPlayer1.CurrentLives > 0)
            {
                // Instantiate prefab.
                GameObject pgo = Instantiate(playerPrefab1, InitialPosition1, Quaternion.identity) as GameObject;
                // Get PlayerController component.
                PlayerController pc = pgo.GetComponent<PlayerController>();
                // Assign playerStats.
                statsPlayer1.SetFullHP();
            }

        }
    }

    public void GameOver(int playerId)
    {
        if (playerId == 1)
        { isActivePlayer1 = false; }
        if (playerId == 2)
        { isActivePlayer2 = false; }
        FinishGame();
    }

    public void FinishGame()
    {
        if (!isActivePlayer1 && !isActivePlayer2)
        {
            Debug.Log("Game Over!");
        }
    }

    public void UpdatePlayerStats(int playerId)
    {
        if (playerId == 1)
        {
            statsText1.text = $"HP: {statsPlayer1.CurrentHP}\n";
            statsText1.text += $"Lives: {statsPlayer1.CurrentLives}";
        }
        if (playerId == 2)
        {
            statsText2.text = $"HP: {statsPlayer1.CurrentHP}";
        }
    }
}
