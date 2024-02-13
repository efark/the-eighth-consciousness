using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using TMPro;

public class GUIController : MonoBehaviour
{
    public AudioMixer audioMixer;
    public AudioSource musicTrack;
    public AudioSource gameOverTrack;

    [Header("Stats")]
    public PlayerStats statsPlayer1;
    public PlayerStats statsPlayer2;

    [Header("Text")]
    public TMP_Text statsText1;
    public TMP_Text statsText2;
    public TMP_Text gameOverText;

    private float sfxVolume;
    private float musicVolume;

    // Start is called before the first frame update
    void Start()
    {
        PlayerStats.OnPlayerGUIChange += UpdatePlayerStats;
        PlayerStats.OnGameOver += GameOver;

        UpdatePlayerStats(1);
        if (statsPlayer2.IsActive)
        {
            UpdatePlayerStats(2);
        }
        else {
            disableText(statsText2);
        }

        LoadPreferences();
        SetAudioVolume();
    }

    void OnDestroy()
    {
        PlayerStats.OnPlayerGUIChange -= UpdatePlayerStats;
        PlayerStats.OnGameOver -= GameOver;
    }

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

    public void UpdatePlayerStats(int playerId)
    {
        if (playerId == 1)
        {
            _updatePlayerStats(statsText1, statsPlayer1);
        }
        if (playerId == 2)
        {
            _updatePlayerStats(statsText2, statsPlayer2);
        }
    }

    private void _updatePlayerStats(TMP_Text statsText, PlayerStats pStats)
    {
        statsText.text = $"HP: {pStats.CurrentHP}\n";
        statsText.text += $"Lives: {pStats.CurrentLives}\n";
        statsText.text += $"Bombs: {pStats.CurrentBombs}\n";
        statsText.text += $"ECD: {pStats.CurrentECDstatus}\n";
        statsText.text += $"Score: {pStats.CurrentScore}";
        return;
    }

    public void GameOver(int playerId)
    {
        if (playerId == 1)
        {
            _gameOver(statsText1);
        }
        if (playerId == 2)
        {
            _gameOver(statsText2);
        }
        if (!statsPlayer1.IsActive && !statsPlayer2.IsActive)
        {
            FinishGame();
        }
    }

    private void _gameOver(TMP_Text statsText)
    {
        statsText.text = "Game Over";
    }

    private void disableText(TMP_Text statsText)
    {
        statsText.text = "";
    }

    public void FinishGame()
    {
        musicTrack.Stop();
        gameOverTrack.Play();
        disableText(statsText1);
        disableText(statsText2);
        gameOverText.text = "Game Over!";
        StartCoroutine(loadScene());
    }

    IEnumerator loadScene()
    {
        yield return new WaitForSecondsRealtime(10.0f);
        SceneManager.LoadScene("MainMenu");
    }
}
