using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public PlayerStats statsPlayer1;
    public PlayerStats statsPlayer2;
    public bool isActivePlayer1;
    public bool isActivePlayer2;

    // Start is called before the first frame update
    void Start()
    {
        PlayerStats.OnPlayerDeath += Respawn;
        PlayerStats.OnGameOver += GameOver;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Respawn(int playerId)
    { 
        // Instantiate prefab.
        // Get PlayerController component.
        // Assign playerStats.
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
}
