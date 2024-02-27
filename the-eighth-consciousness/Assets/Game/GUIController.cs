using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GUIController : MonoBehaviour
{
    public AudioMixer audioMixer;
    public AudioSource musicTrack;
    public AudioSource levelCompleteSFX;

    [Header("Stats")]
    public PlayerStats statsPlayer1;
    public PlayerStats statsPlayer2;

    [Header("Text")]
    public TMP_Text statsText1;
    public TMP_Text statsText2;
    public TMP_Text gameOverText;

    [Header("Bars")]
    [SerializeField] private GameObject _healthBar1;
    [SerializeField] private GameObject _healthBar2;
    [SerializeField] private GameObject _ECDBar1;
    [SerializeField] private GameObject _ECDBar2;
    [SerializeField] private Image _healthbarSprite1;
    [SerializeField] private Image _healthbarSprite2;
    [SerializeField] private GameObject _bossBar;
    [SerializeField] private Image _bossHealthbarSprite;
    private int maxHP1;
    private int maxHP2;

    private float sfxVolume;
    private float musicVolume;

    // Start is called before the first frame update
    void Start()
    {
        PlayerStats.OnPlayerGUIChange += UpdatePlayerStats;
        PlayerStats.OnGameOver += GameOver;
        LevelController.BossAppears += startBossGUI;
        Boss.UpdateHPBar += updateBossGUI;
        maxHP1 = statsPlayer1._maxHP;
        maxHP2 = statsPlayer2._maxHP;

        UpdatePlayerStats(1);
        if (statsPlayer2.IsActive)
        {
            UpdatePlayerStats(2);
        }
        else {
            disableUI(statsText2, _healthBar2, _ECDBar2);
        }

        LoadPreferences();
        SetAudioVolume();
    }

    private void startBossGUI()
    {
        _bossBar.SetActive(true);
    }
    
    private void updateBossGUI(float percentage)
    {
        Debug.Log($"Percentage: {percentage}");
        updateBossHealthBar(_bossHealthbarSprite, percentage);
        if (percentage <= 0)
        {
            levelComplete();
        }
    }

    public void DisableGUI()
    {
        disableUI(statsText1, _healthBar1, _ECDBar1);
        disableUI(statsText2, _healthBar2, _ECDBar2);
    }

    void OnDestroy()
    {
        PlayerStats.OnPlayerGUIChange -= UpdatePlayerStats;
        PlayerStats.OnGameOver -= GameOver;
        Boss.UpdateHPBar -= updateBossGUI;
        LevelController.BossAppears -= startBossGUI;
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
            updateHealthBar(_healthbarSprite1, maxHP1, statsPlayer1.CurrentHP);
        }
        if (playerId == 2)
        {
            _updatePlayerStats(statsText2, statsPlayer2);
            updateHealthBar(_healthbarSprite2, maxHP2, statsPlayer2.CurrentHP);
        }
    }

    private void updateHealthBar(Image hbSprite, int maxHP, int currentHP)
    {
        hbSprite.fillAmount = (float)currentHP / (float)maxHP;
    }

    private void updateBossHealthBar(Image hbSprite, float fillPercentage)
    {
        hbSprite.fillAmount = fillPercentage;
    }

    private void _updatePlayerStats(TMP_Text statsText, PlayerStats pStats)
    {
        statsText.text = $"HP:\n";
        statsText.text += $"Lives: {pStats.CurrentLives}\n";
        statsText.text += $"Bombs: {pStats.CurrentBombs}\n";
        statsText.text += $"ECD:\n";
        statsText.text += $"Score: {pStats.CurrentScore}";
        return;
    }

    public void GameOver(int playerId)
    {
        if (playerId == 1)
        {
            _gameOver(statsText1, _healthBar1, _ECDBar1);
        }
        if (playerId == 2)
        {
            _gameOver(statsText2, _healthBar2, _ECDBar2);
        }
        if (!statsPlayer1.IsActive && !statsPlayer2.IsActive)
        {
            FinishGame();
        }
    }

    private void _gameOver(TMP_Text statsText, GameObject HPBar, GameObject ECDBar)
    {
        statsText.text = "Game Over";
        HPBar.SetActive(false);
        ECDBar.SetActive(false);
    }

    private void disableUI(TMP_Text statsText, GameObject HPBar, GameObject ECDBar)
    {
        statsText.text = "";
        HPBar.SetActive(false);
        ECDBar.SetActive(false);
    }

    public void FinishGame()
    {
        musicTrack.Stop();
        disableUI(statsText1, _healthBar1, _ECDBar1);
        disableUI(statsText2, _healthBar2, _ECDBar2);
        gameOverText.text = "Game Over!";
        StartCoroutine(loadScene());
    }

    IEnumerator loadScene()
    {
        yield return new WaitForSecondsRealtime(10.0f);
        SceneManager.LoadScene("MainMenu");
    }

    private void levelComplete()
    {
        musicTrack.Stop();
        levelCompleteSFX.Play();
        disableUI(statsText1, _healthBar1, _ECDBar1);
        disableUI(statsText2, _healthBar2, _ECDBar2);
        gameOverText.text = "Level Complete!\nThank you for playing!";
        StartCoroutine(loadScene());
    }
}
