using System.Collections;
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
    [Tooltip("If isAimingPlayer is on, then the spawned enemy will start pointing in the direction of the player.")]
    public bool isAimingPlayer;
    [Tooltip("If hasFixedDirection is on, then the spawner will send the enemy in the predetermined direction.")]
    public bool hasFixedDirection;
    public Vector2 fixedDirection;
    public Vector2 fixedDirectionRange;
    [Tooltip("If hasFixedDirection is on, then the spawner will send the enemy in the predetermined direction.")]
    public bool hasTargetPoint;
    public Vector2 targetPoint;
    public Vector2 targetPointRange;

}
