using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public List<WavesSettings> settings = new List<WavesSettings>();

    private float time;

    void Start()
    {
        time = 0;
        foreach (WavesSettings ws in settings)
        {
            ws.SetActivated(false);
        }
    }

    void Update()
    {
        foreach (WavesSettings ws in settings)
        {
            if (ws.startingTime < time && !ws.Activated)
            {
                ws.SetActivated(true);
                Instantiate(ws.enemySpawner, ws.startingPosition, Quaternion.identity);
            }
        }
        time += Time.deltaTime;
    }
}
