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

    public bool Activated => activated;
    public void SetActivated(bool value)
    { 
        activated = value;
    }
}
