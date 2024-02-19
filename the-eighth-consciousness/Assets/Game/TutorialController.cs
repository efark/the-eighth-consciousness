using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour
{
    public GUISkin guiSkin;
    private Rect windowRect;
    private Rect skipTutorialRect;

    public AudioSource startFX;
    public AudioSource okFX;
    public AudioSource BGMusic;

    public GUIController gui;
    public PlayerStats statsPlayer1;
    public PlayerStats statsPlayer2;
    public GameObject playerPrefab1;
    public GameObject playerPrefab2;
    public Vector3 InitialPosition1;
    public Vector3 InitialPosition2;
    private GameObject player1;
    private GameObject player2;
    [SerializeField] private Image _ECDSprite1;
    [SerializeField] private Image _ECDSprite2;
    private bool hasPlayer2;
    private bool hasEndSequenceStarted;

    private bool hasMoved1;
    private bool hasMoved2;
    private bool hasFired1;
    private bool hasFired2;
    private bool hasBombed1;
    private bool hasBombed2;
    private bool hasECDed1;
    private bool hasECDed2;

    private Camera cam;
    private Vector3 bottomLeft;
    private Vector3 topRight;
    private float centerX;

    public void Start()
    {
        hasEndSequenceStarted = false;
        hasPlayer2 = statsPlayer2.IsActive;
        hasMoved1 = !statsPlayer1.IsActive;
        hasFired1 = !statsPlayer1.IsActive;
        hasBombed1 = !statsPlayer1.IsActive;
        hasECDed1 = !statsPlayer1.IsActive;
        hasMoved2 = !statsPlayer2.IsActive;
        hasFired2 = !statsPlayer2.IsActive;
        hasBombed2 = !statsPlayer2.IsActive;
        hasECDed2 = !statsPlayer2.IsActive;

        /*-------------------------------------------------------------------------------------
        The logic to calculate the screen borders was taken from Unity's documentation:
        https://docs.unity3d.com/ScriptReference/Camera.ScreenToWorldPoint.html
        -------------------------------------------------------------------------------------*/
        cam = Camera.main;
        centerX = cam.pixelWidth / 2;

        skipTutorialRect = new Rect(cam.pixelWidth - 170, cam.pixelHeight - 80, 160, 50);

        if (statsPlayer1.IsActive)
        {
            player1 = Instantiate(playerPrefab1, InitialPosition1, Quaternion.identity) as GameObject;
            player1.transform.GetComponent<PlayerController>().SetECDSprite(_ECDSprite1);
            statsPlayer1.Init();
            statsPlayer1.UpdateIsAlive(true);
            windowRect = new Rect(centerX-125, 40, 250, 250);
        }
        if (statsPlayer2.IsActive)
        {
            player2 = Instantiate(playerPrefab2, InitialPosition2, Quaternion.identity) as GameObject;
            player2.transform.GetComponent<PlayerController>().SetECDSprite(_ECDSprite2);
            statsPlayer2.Init();
            statsPlayer2.UpdateIsAlive(true);
            windowRect = new Rect(centerX-300, 40, 600, 250);
        }

        PlayerController.OnMovement += markMovement;
        PlayerController.OnBombUse += markBombUse;
        PlayerController.OnShoot += markShoot;
        PlayerController.OnECD += markECD;

    }

    private void markMovement(int playerId)
    {
        if (playerId == 1)
        {
            hasMoved1 = true;
            return;
        }
        if (playerId == 2)
        {
            hasMoved2 = true;
            return;
        }
    }

    private void markShoot(int playerId)
    {
        if (playerId == 1 && hasMoved1)
        {
            hasFired1 = true;
            return;
        }
        if (playerId == 2 && hasMoved2)
        {
            hasFired2 = true;
            return;
        }
    }

    private void markBombUse(int playerId)
    {
        if (playerId == 1 && hasFired1)
        {
            hasBombed1 = true;
            return;
        }
        if (playerId == 2 && hasFired2)
        {
            hasBombed2 = true;
            return;
        }
    }

    private void markECD(int playerId)
    {
        if (playerId == 1 && hasBombed1)
        {
            hasECDed1 = true;
            return;
        }
        if (playerId == 2 && hasBombed2)
        {
            hasECDed2 = true;
            return;
        }
    }


    void Update()
    {
        if (hasEndSequenceStarted)
        {
            return;
        }

        bool skipTutorial = Input.GetButton("skipTutorial");
        if (skipTutorial)
        {
            endTutorial();
            return;
        }
        if ( // Either player is not active or has done all actions.
            (!statsPlayer1.IsActive || (hasMoved1 && hasFired1 && hasBombed1 && hasECDed1)) &&
            (!statsPlayer2.IsActive || (hasMoved2 && hasFired2 && hasBombed2 && hasECDed2))
            )
        {
            endTutorial();
            return;
        }
    }

    private void OnGUI()
    {
        GUI.skin = guiSkin;

        GUI.skin.button.wordWrap = true;
        if (!hasEndSequenceStarted)
        {
            if (GUI.Button(skipTutorialRect, "Skip tutorial [Shift]"))
            {
                endTutorial();
            }
        }

        if (hasEndSequenceStarted)
        {
            return;
        }
        if (!hasMoved1 || !hasMoved2)
        {
            windowRect = GUI.Window(0, windowRect, movementHelp, "Tutorial");
            return;
        }
        else if (!hasFired1 || !hasFired2)
        {
            windowRect = GUI.Window(0, windowRect, fireHelp, "Tutorial");
            return;
        }
        else if (!hasBombed1 || !hasBombed2)
        {
            windowRect = GUI.Window(0, windowRect, bombHelp, "Tutorial");
            return;
        }
        else if (!hasECDed1 || !hasECDed2)
        {
            windowRect = GUI.Window(0, windowRect, ecdHelp, "Tutorial");
            return;
        }
    }

    private void endTutorial()
    {
        PlayerController.OnMovement -= markMovement;
        PlayerController.OnBombUse -= markBombUse;
        PlayerController.OnShoot -= markShoot;
        PlayerController.OnECD -= markECD;
        StartCoroutine(loadScene());
    }

    IEnumerator loadScene()
    {
        gui.DisableGUI();
        hasEndSequenceStarted = true;
        cam.GetComponent<FadeController>().StartFadeIn();
        startFX.Play();
        yield return new WaitForSecondsRealtime(5.0f);
        SceneManager.LoadScene("Proto1");
    }


    private void movementHelp(int windowID)
    {
        if (hasPlayer2)
        {
            GUI.Box(new Rect(70, 30, 170, 280), "Player 1");
            GUI.Label(new Rect(90, 50, 60, 20), "Move");
            GUI.enabled = false;
            GUI.Button(new Rect(130, 80, 40, 20), "W");
            GUI.Button(new Rect(80, 120, 40, 20), "A");
            GUI.Button(new Rect(130, 120, 40, 20), "S");
            GUI.Button(new Rect(180, 120, 40, 20), "D");
            if (hasMoved1)
            {
                GUI.Button(new Rect(130, 170, 40, 20), "OK");
            }
            GUI.enabled = true;

            GUI.Box(new Rect(360, 30, 170, 280), "Player 2");
            GUI.Label(new Rect(390, 50, 60, 20), "Move");
            GUI.enabled = false;
            GUI.Button(new Rect(430, 80, 40, 20), "\u21E7");
            GUI.Button(new Rect(380, 120, 40, 20), "\u21E6");
            GUI.Button(new Rect(430, 120, 40, 20), "\u21E9");
            GUI.Button(new Rect(480, 120, 40, 20), "\u21E8");
            if (hasMoved2)
            {
                GUI.Button(new Rect(430, 170, 60, 20), "OK");
            }
            GUI.enabled = true;
            return;
        }
        GUI.Box(new Rect(40, 30, 170, 240), "Player 1");
        GUI.Label(new Rect(60, 50, 60, 20), "Move");
        GUI.enabled = false;
        GUI.Button(new Rect(100, 80, 40, 20), "W");
        GUI.Button(new Rect(50, 120, 40, 20), "A");
        GUI.Button(new Rect(100, 120, 40, 20), "S");
        GUI.Button(new Rect(150, 120, 40, 20), "D");
        GUI.enabled = true;
        return;

    }

    private void fireHelp(int windowID)
    {
        if (hasPlayer2)
        {
            GUI.Box(new Rect(70, 30, 170, 280), "Player 1");
            GUI.Label(new Rect(90, 50, 60, 20), "Shoot");
            GUI.enabled = false;
            GUI.Button(new Rect(130, 80, 40, 20), "Y");
            if (hasFired1)
            {
                GUI.Button(new Rect(130, 170, 40, 20), "OK");
            }
            GUI.enabled = true;

            GUI.Box(new Rect(360, 30, 170, 280), "Player 2");
            GUI.Label(new Rect(390, 50, 60, 20), "Shoot");
            GUI.enabled = false;
            GUI.Button(new Rect(430, 80, 40, 20), "1");
            if (hasFired2)
            {
                GUI.Button(new Rect(430, 170, 60, 20), "OK");
            }
            GUI.enabled = true;
            return;
        }
        GUI.Box(new Rect(40, 30, 170, 240), "Player 1");
        GUI.Label(new Rect(60, 50, 60, 20), "Shoot");
        GUI.enabled = false;
        GUI.Button(new Rect(100, 80, 40, 20), "Y");
        GUI.enabled = true;
        return;
    }

    private void bombHelp(int windowID)
    {
        if (hasPlayer2)
        {
            GUI.Box(new Rect(70, 30, 170, 280), "Player 1");
            GUI.Label(new Rect(90, 50, 60, 20), "Bomb");
            GUI.enabled = false;
            GUI.Button(new Rect(130, 80, 40, 20), "U");
            if (hasBombed1)
            {
                GUI.Button(new Rect(130, 170, 40, 20), "OK");
            }
            GUI.enabled = true;

            GUI.Box(new Rect(360, 30, 170, 280), "Player 2");
            GUI.Label(new Rect(390, 50, 60, 20), "Bomb");
            GUI.enabled = false;
            GUI.Button(new Rect(430, 80, 40, 20), "2");
            if (hasBombed2)
            {
                GUI.Button(new Rect(430, 170, 60, 20), "OK");
            }
            GUI.enabled = true;
            return;
        }
        GUI.Box(new Rect(40, 30, 170, 240), "Player 1");
        GUI.Label(new Rect(60, 50, 60, 20), "Bomb");
        GUI.enabled = false;
        GUI.Button(new Rect(100, 80, 40, 20), "U");
        GUI.enabled = true;
        return;
    }

    private void ecdHelp(int windowID)
    {
        if (hasPlayer2)
        {
            GUI.Box(new Rect(70, 30, 170, 280), "Player 1");
            GUI.Label(new Rect(90, 50, 60, 20), "ECD");
            GUI.enabled = false;
            GUI.Button(new Rect(130, 80, 40, 20), "I");
            if (hasECDed1)
            {
                GUI.Button(new Rect(130, 170, 40, 20), "OK");
            }
            GUI.enabled = true;

            GUI.Box(new Rect(360, 30, 170, 280), "Player 2");
            GUI.Label(new Rect(390, 50, 60, 20), "ECD");
            GUI.enabled = false;
            GUI.Button(new Rect(430, 80, 40, 20), "3");
            if (hasECDed2)
            {
                GUI.Button(new Rect(430, 170, 60, 20), "OK");
            }
            GUI.enabled = true;
            return;
        }
        GUI.Box(new Rect(40, 30, 170, 240), "Player 1");
        GUI.Label(new Rect(60, 50, 60, 20), "ECD");
        GUI.enabled = false;
        GUI.Button(new Rect(100, 80, 40, 20), "I");
        GUI.enabled = true;
        return;
    }

}
