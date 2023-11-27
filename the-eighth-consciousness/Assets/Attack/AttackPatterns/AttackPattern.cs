using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AttackPattern : ScriptableObject
{
    [Header("Basic Parameters")]
    public SpreadSettings spreadSettings;
    public AbstractSpread spread;
    public BulletSettings bulletSettings;

    [Header("Burst Parameters")]
    public float burstOffset;
    public int burstSize;
    public float burstSpacing;

    [Header("Attack Parameters")]
    public float offset;
    public float cooldown;
    public int numberOfBursts;
    public int order;
    public bool keepsAiming;
    public bool isStaticAttack;
    public bool isConstantAttack;
    public bool isActive = false;
    private bool isRunning = false;
    public bool IsRunning => isRunning;
    public void UpdateIsRunning(bool value)
    {
        isRunning = value;
    }
    public bool isOpposite = false;

    public void Init(TargetTypes targetType)
    {
        BulletFactory bf = new BulletFactory(bulletSettings, targetType, 0, offset, 1f);
        this.spread = AuxiliaryMethods.InitSpread(bf, spreadSettings);
    }
}
