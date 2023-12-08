using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SpawnerSettings : ScriptableObject
{
    [Header("Spawner")]
    public GameObject prefab;
    public SpreadSettings spreadSettings;
    public int waves;
    public float startDelay;
    public float cooldown;
    public bool autoTarget;
}
