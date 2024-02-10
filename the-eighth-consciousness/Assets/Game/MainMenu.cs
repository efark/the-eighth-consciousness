using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public PlayerStats statsPlayer1;
    public PlayerStats statsPlayer2;

    public GUISkin guiSkin;

    public Canvas canvas;
    public AudioMixer audioMixer;

    public AudioSource startFX;
    public AudioSource navigateMenuFX;
    public AudioSource testFX;
    public AudioSource BGMusic;

    private float initialSFXVolume;
    private float initialMusicVolume;
    private float currentSFXVolume;
    private float currentMusicVolume;

    public bool showStartMenu = false;
    public bool ShowStartMenu
    { 
        get { return showStartMenu; }
        set { showStartMenu = value; }
    }

    public bool showOptionsMenu = false;
    public bool ShowOptionsMenu
    { 
        get { return showOptionsMenu; }
        set { showOptionsMenu = value; }
    }

    public bool showControlsMenu = false;
    public bool ShowControlsMenu
    {
        get { return showControlsMenu; }
        set { showControlsMenu = value; }
    }

    Rect windowRect = new Rect(0, 0, 500, 360);
    float sfxSliderValue;
    float musicSliderValue;

    public void LoadPlayerPrefs()
    {
        initialSFXVolume = PlayerPrefs.HasKey("sfxVolume") ? PlayerPrefs.GetFloat("sfxVolume") : 1.0f;
        initialMusicVolume = PlayerPrefs.HasKey("musicVolume") ? PlayerPrefs.GetFloat("musicVolume") : 1.0f;
        currentSFXVolume = initialSFXVolume;
        currentMusicVolume = initialMusicVolume;
        sfxSliderValue = initialSFXVolume;
        musicSliderValue = initialMusicVolume;
    }

    private void setFXVolume(float volume)
    {
        audioMixer.SetFloat("sfxVolume", Mathf.Log(volume) * 20);
    }

    private void setMusicVolume(float volume)
    {
        audioMixer.SetFloat("musicVolume", Mathf.Log(volume) * 20);
    }

    void Start()
    {
        windowRect.x = (Screen.width - windowRect.width) / 2;
        windowRect.y = (Screen.height - windowRect.height) / 2;

        statsPlayer1.UpdateIsActive(false);
        statsPlayer2.UpdateIsActive(false);

        LoadPlayerPrefs();
        setFXVolume(currentSFXVolume);
        setMusicVolume(currentMusicVolume);
    }

    void Update()
    {
        /* The formula to scale the volume was taken from Unity Forum:
           https://forum.unity.com/threads/changing-audio-mixer-group-volume-with-ui-slider.297884/#post-3494983
        */
        setFXVolume(currentSFXVolume);
        setMusicVolume(currentMusicVolume);
    }

    public void PlayNavigationSound()
    {
        navigateMenuFX.PlayOneShot(navigateMenuFX.clip);
    }

    void OnGUI()
    {
        GUI.skin = guiSkin;
        if (showControlsMenu)
        {
            canvas.enabled = false;
            windowRect = GUI.Window(0, windowRect, controlsMenu, "Controls");
        }

        if (showOptionsMenu)
        {
            canvas.enabled = false;
            windowRect = GUI.Window(0, windowRect, optionsMenu, "Options");
        }
        if (showStartMenu)
        {
            canvas.enabled = false;
            windowRect = GUI.Window(0, windowRect, startGameMenu, "Start Game");
        }
    }

    public void startGame(bool hasPlayer2)
    {
        Debug.Log("Start new Game");
        BGMusic.Stop();
        startFX.Play();
        // Add Animation.
        statsPlayer1.Init();
        if(hasPlayer2)
        { 
            statsPlayer2.Init();
        }
        StartCoroutine(loadScene());
    }

    IEnumerator loadScene()
    {
        yield return new WaitForSecondsRealtime(2.5f);
        SceneManager.LoadScene("Tutorial");
    }

    public void startGameMenu(int windowID)
    {
        if (GUI.Button(new Rect(40, 160, 100, 25), "1 Player"))
        {
            startGame(false);
        }
        if (GUI.Button(new Rect(360, 160, 100, 25), "2 Players"))
        {
            startGame(true);
        }

        if (GUI.Button(new Rect(190, 300, 100, 20), "Back"))
        {
            navigateMenuFX.PlayOneShot(navigateMenuFX.clip);
            showStartMenu = false;
            canvas.enabled = true;
        }
    }

    private void saveSettings()
    {
        PlayerPrefs.SetFloat("sfxVolume", currentSFXVolume);
        PlayerPrefs.SetFloat("musicVolume", currentMusicVolume);
    }

    private void cancelChanges()
    {
        currentSFXVolume = initialSFXVolume;
        currentMusicVolume = initialMusicVolume;
        if (!PlayerPrefs.HasKey("sfxVolume"))
        {
            PlayerPrefs.SetFloat("sfxVolume", initialSFXVolume);
        }
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", initialMusicVolume);
        }
        setFXVolume(initialSFXVolume);
        setMusicVolume(initialMusicVolume);
    }

    public void optionsMenu(int windowID)
    {
        // SFX Volume.
        GUI.Label(new Rect(70, 90, 120, 30), "SFX Volume");
        currentSFXVolume = GUI.HorizontalSlider(new Rect(220, 90, 50, 30), currentSFXVolume, 0.0001F, 1.0F);
        if (GUI.Button(new Rect(280, 80, 100, 20), "SFX Test"))
        {
            testFX.PlayOneShot(testFX.clip);
        }
        // Music Volume.
        GUI.Label(new Rect(70, 130, 120, 30), "Music Volume");
        currentMusicVolume = GUI.HorizontalSlider(new Rect(220, 130, 50, 30), currentMusicVolume, 0.0001F, 1.0F);

        if (GUI.Button(new Rect(200, 200, 100, 20), "Controls"))
        {
            navigateMenuFX.PlayOneShot(navigateMenuFX.clip);
            showControlsMenu = true;
            showOptionsMenu = false;
        }

        if (GUI.Button(new Rect(40, 300, 100, 25), "Cancel"))
        {
            Debug.Log("Back");
            navigateMenuFX.PlayOneShot(navigateMenuFX.clip);
            showOptionsMenu = false;
            canvas.enabled = true;
            cancelChanges();
        }
        if (GUI.Button(new Rect(360, 300, 100, 25), "Done"))
        {
            Debug.Log("Save and back");
            navigateMenuFX.PlayOneShot(navigateMenuFX.clip);
            showOptionsMenu = false;
            canvas.enabled = true;
            saveSettings();
        }

    }

    public void controlsMenu(int windowID)
    {
        GUI.Box(new Rect(10, 40, 240, 370), "Player 1");
        GUI.enabled = false;
        GUI.Label(new Rect(40, 80, 60, 20), "Move");
        GUI.Button(new Rect(100, 90, 40, 20), "W");
        GUI.Button(new Rect(50, 130, 40, 20), "A");
        GUI.Button(new Rect(100, 130, 40, 20), "S");
        GUI.Button(new Rect(150, 130, 40, 20), "D");
    
        GUI.Label(new Rect(30, 180, 60, 20), "Shoot");
        GUI.Button(new Rect(100, 180, 40, 20), "Y");
        GUI.Label(new Rect(30, 220, 60, 20), "Bomb");
        GUI.Button(new Rect(100, 220, 40, 20), "U");
        GUI.Label(new Rect(30, 260, 60, 20), "ECD");
        GUI.Button(new Rect(100, 260, 40, 20), "I");

        GUI.enabled = true;
        GUI.Box(new Rect(250, 40, 240, 370), "Player 2");
        GUI.enabled = false;
        GUI.Label(new Rect(290, 80, 60, 20), "Move");
        GUI.Button(new Rect(340, 90, 40, 20), "\u21E7");
        GUI.Button(new Rect(290, 130, 40, 20), "\u21E6");
        GUI.Button(new Rect(340, 130, 40, 20), "\u21E9");
        GUI.Button(new Rect(390, 130, 40, 20), "\u21E8");

        GUI.Label(new Rect(270, 180, 60, 20), "Shoot");
        GUI.Button(new Rect(340, 180, 40, 20), "1");
        GUI.Label(new Rect(270, 220, 60, 20), "Bomb");
        GUI.Button(new Rect(340, 220, 40, 20), "2");
        GUI.Label(new Rect(270, 260, 60, 20), "ECD");
        GUI.Button(new Rect(340, 260, 40, 20), "3");
        GUI.enabled = true;

        if (GUI.Button(new Rect(200, 330, 100, 20), "Back"))
        {
            navigateMenuFX.PlayOneShot(navigateMenuFX.clip);
            showControlsMenu = false;
            showOptionsMenu = true;
            canvas.enabled = true;
        }
    }
}
