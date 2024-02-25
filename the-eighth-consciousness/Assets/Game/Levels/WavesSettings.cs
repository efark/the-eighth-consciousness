using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WavesSettings : ScriptableObject
{
    public float startingTime;
    public GameObject enemySpawner;
    public Vector3 startingPosition;
    private bool activated;
    private bool done;

    public bool Activated => activated;
    public bool Done => done;
    public void SetActivated(bool value)
    { 
        activated = value;
    }
    public void SetDone(bool value)
    {
        done = value;
    }
}
