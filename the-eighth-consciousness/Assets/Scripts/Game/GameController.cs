using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Font 'Futura' downloaded from https://ttfonts.net/.
public class GameController : MonoBehaviour
{
    public TMP_Text statsText1;
    public TMP_Text statsText2;
    public TMP_Text gameOverText;
    public PlayerStats statsPlayer1;
    public PlayerStats statsPlayer2;
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
        UpdatePlayerStats(2);
    }

    public void Respawn(int playerId)
    {
        if (playerId == 1)
        {
            if (statsPlayer1.IsActive)
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
        {
            statsPlayer1.UpdateIsActive(false);
            UpdatePlayerStats(1);
        }
        if (playerId == 2)
        {
            statsPlayer2.UpdateIsActive(false);
            UpdatePlayerStats(2);
        }
        FinishGame();
    }

    public void FinishGame()
    {
        if (!statsPlayer1.IsActive && !statsPlayer2.IsActive)
        {
            gameOverText.text = "Game Over!";
        }
    }

    public void UpdatePlayerStats(int playerId)
    {
        if (playerId == 1)
        {
            if (statsPlayer1.IsActive)
            {
                statsText1.text = $"HP: {statsPlayer1.CurrentHP}\n";
                statsText1.text += $"Lives: {statsPlayer1.CurrentLives}";
                return;
            }
            statsText1.text = "Game Over";
        }
        if (playerId == 2)
        {
            if (statsPlayer2.IsActive)
            {
                statsText2.text = $"HP: {statsPlayer2.CurrentHP}\n";
                statsText2.text += $"Lives: {statsPlayer2.CurrentLives}";
                return;
            }
            statsText2.text = "Game Over";
        }
    }
}
