using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AttackPattern : ScriptableObject
{
    [Header("Basic Parameters")]
    public BulletSettings bulletSettings;
    public SpreadSettings spreadSettings;
    public AbstractShotSpread shotSpread;
    public BurstSettings burstSettings;
    public AbstractShotBurst shotBurst;

    public float cooldown;
    public int numberOfBursts;
    public int order;
    public bool keepsAiming;
    public bool isStaticAttack;
    public bool isConstantAttack;
}