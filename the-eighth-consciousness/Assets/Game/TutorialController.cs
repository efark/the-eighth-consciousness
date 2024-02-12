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

    public AudioSource startFX;
    public AudioSource okFX;
    public AudioSource BGMusic;

    public PlayerStats statsPlayer1;
    public PlayerStats statsPlayer2;
    public GameObject playerPrefab1;
    public GameObject playerPrefab2;
    public Vector3 InitialPosition1;
    public Vector3 InitialPosition2;
    private GameObject player1;
    private GameObject player2;
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
        //bottomLeft = cam.ScreenToWorldPoint(Vector3.zero);
        //topRight = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, 0, cam.pixelHeight));
        centerX = cam.pixelWidth / 2;

        if (statsPlayer1.IsActive)
        {
            player1 = Instantiate(playerPrefab1, InitialPosition1, Quaternion.identity) as GameObject;
            statsPlayer1.Init();
            statsPlayer1.UpdateIsAlive(true);
            windowRect = new Rect(centerX-125, 40, 250, 180);
        }
        if (statsPlayer2.IsActive)
        {
            player2 = Instantiate(playerPrefab2, InitialPosition2, Quaternion.identity) as GameObject;
            statsPlayer2.Init();
            statsPlayer2.UpdateIsAlive(true);
            windowRect = new Rect(centerX-200, 40, 400, 180);
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
        if (playerId == 1)
        {
            hasFired1 = true;
            return;
        }
        if (playerId == 2)
        {
            hasFired2 = true;
            return;
        }
    }

    private void markBombUse(int playerId)
    {
        if (playerId == 1)
        {
            hasBombed1 = true;
            return;
        }
        if (playerId == 2)
        {
            hasBombed2 = true;
            return;
        }
    }

    private void markECD(int playerId)
    {
        if (playerId == 1)
        {
            hasECDed1 = true;
            return;
        }
        if (playerId == 2)
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
        if (hasEndSequenceStarted)
        {
            windowRect = GUI.Window(0, windowRect, readyHelp, "Start");
            return;
        }
        if (!hasMoved1 || !hasMoved2)
        {
            windowRect = GUI.Window(0, windowRect, movementHelp, "Help");
            return;
        }
        else if (!hasFired1 || !hasFired2)
        {
            windowRect = GUI.Window(0, windowRect, fireHelp, "Help");
            return;
        }
        else if (!hasBombed1 || !hasBombed2)
        {
            windowRect = GUI.Window(0, windowRect, bombHelp, "Help");
            return;
        }
        else if (!hasECDed1 || !hasECDed2)
        {
            windowRect = GUI.Window(0, windowRect, ecdHelp, "Help");
            return;
        }
    }

    private void endTutorial()
    {
        PlayerController.OnMovement -= markMovement;
        PlayerController.OnBombUse -= markBombUse;
        PlayerController.OnShoot -= markShoot;
        PlayerController.OnECD -= markECD;
        hasEndSequenceStarted = true;
        StartCoroutine(loadScene());
    }

    IEnumerator loadScene()
    {
        yield return new WaitForSecondsRealtime(1.0f);
        SceneManager.LoadScene("Proto1");
    }


    private void movementHelp(int windowID)
    {
        if (hasPlayer2)
        {
            GUI.Box(new Rect(centerX - 180, 20, 160, 160), "Player 1");
            GUI.Label(new Rect(centerX - 170, 40, 60, 20), "Move");
            GUI.enabled = false;
            GUI.Button(new Rect(centerX - 130, 60, 40, 20), "W");
            GUI.Button(new Rect(centerX - 170, 100, 40, 20), "A");
            GUI.Button(new Rect(centerX - 130, 100, 40, 20), "S");
            GUI.Button(new Rect(centerX - 90, 100, 40, 20), "D");
            GUI.enabled = true;

            GUI.Box(new Rect(centerX + 10, 20, 160, 160), "Player 2");
            GUI.Label(new Rect(centerX + 20, 40, 60, 20), "Move");
            GUI.enabled = false;
            GUI.Button(new Rect(centerX + 20, 60, 40, 20), "\u21E7");
            GUI.Button(new Rect(centerX + 60, 100, 40, 20), "\u21E6");
            GUI.Button(new Rect(centerX + 20, 100, 40, 20), "\u21E9");
            GUI.Button(new Rect(centerX + 100, 100, 40, 20), "\u21E8");
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
            GUI.Box(new Rect(10, 40, 240, 370), "Player 2");
            GUI.Label(new Rect(270, 220, 60, 20), "Shoot");
            GUI.enabled = false;
            GUI.Button(new Rect(340, 220, 40, 20), "1");
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
            GUI.Box(new Rect(10, 40, 240, 370), "Player 2");
            GUI.Label(new Rect(270, 220, 60, 20), "Bomb");
            GUI.enabled = false;
            GUI.Button(new Rect(340, 220, 40, 20), "2");
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
            GUI.Box(new Rect(10, 40, 240, 370), "Player 2");
            GUI.Label(new Rect(30, 260, 60, 20), "ECD");
            GUI.enabled = false;
            GUI.Button(new Rect(340, 260, 40, 20), "3");
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

    private void readyHelp(int windowID)
    {
        GUI.Box(new Rect(40, 30, 170, 100), "Start");
    }
}
