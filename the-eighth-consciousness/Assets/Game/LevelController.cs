using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public List<WavesSettings> settings = new List<WavesSettings>();
    private int index;
    private int lastIndex;
    private GameObject spawner;
    public GameObject boss;

    //private float time;

    void Start()
    {
        // time = 0;
        index = 0;
        lastIndex = 0;
        foreach (WavesSettings ws in settings)
        {
            ws.SetActivated(false);
            lastIndex++;
        }
    }

    void Update()
    {
        if (index < lastIndex)
        {
            if (spawner == null)
            {
                WavesSettings ws = settings[index];
                spawner = Instantiate(ws.enemySpawner, ws.startingPosition, Quaternion.identity) as GameObject;
                index++;
            }
        }
        if (index == lastIndex)
        {
            if (spawner == null)
            {
                Instantiate(boss, transform.position, Quaternion.identity);
                index++;
            }
        }
    }
}
