﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;

// Font 'Futura' downloaded from https://ttfonts.net/.
public class GameController : MonoBehaviour
{
    public AudioMixer audioMixer;

    public AudioSource deathSFX;
    public AudioSource musicTrack;
    public AudioSource gameOverTrack;

    public TMP_Text statsText1;
    public TMP_Text statsText2;
    public TMP_Text gameOverText;
    public PlayerStats statsPlayer1;
    public PlayerStats statsPlayer2;
    public GameObject playerPrefab1;
    public GameObject playerPrefab2;
    public Vector3 InitialPosition1;
    public Vector3 InitialPosition2;

    private float sfxVolume;
    private float musicVolume;

    private void LoadPreferences()
    {
        sfxVolume = PlayerPrefs.HasKey("sfxVolume") ? PlayerPrefs.GetFloat("sfxVolume") : 1.0f;
        musicVolume = PlayerPrefs.HasKey("musicVolume") ? PlayerPrefs.GetFloat("musicVolume") : 1.0f;
    }

    private void SetAudioVolume()
    {
        audioMixer.SetFloat("sfxVolume", Mathf.Log(sfxVolume) * 20);
        audioMixer.SetFloat("musicVolume", Mathf.Log(musicVolume) * 20);
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerStats.OnPlayerGUIChange += UpdatePlayerStats;
        PlayerStats.OnPlayerDeath += Respawn;
        PlayerStats.OnGameOver += GameOver;

        statsPlayer1.Init();

        UpdatePlayerStats(1);
        UpdatePlayerStats(2);

        LoadPreferences();
        SetAudioVolume();

        musicTrack.Play();
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
        musicTrack.Stop();
        gameOverTrack.Play();
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
                statsText1.text += $"Lives: {statsPlayer1.CurrentLives}\n";
                statsText1.text += $"Bombs: {statsPlayer1.CurrentBombs}\n";
                statsText1.text += $"ECD: {statsPlayer1.CurrentECDstatus}";
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
                statsText2.text += $"Bombs: {statsPlayer2.CurrentBombs}\n";
                statsText2.text += $"ECD: {statsPlayer2.CurrentECDstatus}";
                return;
            }
            statsText2.text = "Game Over";
        }
    }
}
