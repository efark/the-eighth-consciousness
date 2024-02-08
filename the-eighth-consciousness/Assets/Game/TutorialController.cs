using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour
{
    public PlayerStats statsPlayer1;
    public PlayerStats statsPlayer2;
    public GameObject playerPrefab1;
    public GameObject playerPrefab2;
    public Vector3 InitialPosition1;
    public Vector3 InitialPosition2;

    private bool hasMoved1;
    private bool hasMoved2;
    private bool hasFired1;
    private bool hasFired2;
    private bool hasBombed1;
    private bool hasBombed2;
    private bool hasECDed1;
    private bool hasECDed2;

    public void Start()
    {
        hasMoved1 = !statsPlayer1.IsActive;
        hasFired1 = !statsPlayer1.IsActive;
        hasBombed1 = !statsPlayer1.IsActive;
        hasECDed1 = !statsPlayer1.IsActive;
        hasMoved2 = !statsPlayer2.IsActive;
        hasFired2 = !statsPlayer2.IsActive;
        hasBombed2 = !statsPlayer2.IsActive;
        hasECDed2 = !statsPlayer2.IsActive;

        if (statsPlayer1.IsActive)
        {
            Instantiate(playerPrefab1, InitialPosition1, Quaternion.identity);
            statsPlayer1.UpdateIsAlive(true);
        }
        if (statsPlayer2.IsActive)
        {
            Instantiate(playerPrefab2, InitialPosition2, Quaternion.identity);
            statsPlayer2.UpdateIsAlive(true);
        }
    }

    void Update()
    {
        bool skipTutorial = Input.GetButton("skipTutorial");

        if (skipTutorial)
        {
            endTutorial();
        }
        if ( // Either player is not active or has done all actions.
            (!statsPlayer1.IsActive || (hasMoved1 && hasFired1 && hasBombed1 && hasECDed1)) &&
            (!statsPlayer2.IsActive || (hasMoved2 && hasFired2 && hasBombed2 && hasECDed2))
            )
        {
            endTutorial();
        }        
    }

    private void endTutorial()
    {
        StartCoroutine(loadScene());
    }

    IEnumerator loadScene()
    {
        yield return new WaitForSecondsRealtime(1.0f);
        SceneManager.LoadScene("Proto1");
    }

}
