﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SpawnerSettings : ScriptableObject
{
    [Header("Spawner")]
    public GameObject prefab;
    public SpreadSettings spreadSettings;
    [Tooltip("Number of waves that the spawner will generate.")]
    public int waves;
    [Tooltip("Wait before the spawner created the first wave.")]
    public float startDelay;
    [Tooltip("Time between waves.")]
    public float cooldown;
    [Tooltip("If autotarget is on, then the spawned enemy will find the direction on its own. Otherwise, the spawner will find the closest enemy.")]
    public bool autoTarget;
    [Tooltip("If hasFixedDirection is on, then the spawner will send the enemy in the predetermined direction.")]
    public bool hasFixedDirection;
    public Vector2 fixedDirection;
    public Vector2 fixedDirectionRange;

}
