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
    public float cooldown;
    public int numberOfBursts;
    public int order;
    public bool keepsAiming;
    public bool isStaticAttack;
    public bool isConstantAttack;
    public bool isActive = false;
    public bool isRunning = false;

    public void Init(TargetTypes targetType)
    {
        BulletFactory bf = new BulletFactory(bulletSettings, targetType, 0);
        this.spread = ExtensionMethods.InitSpread(bf, spreadSettings);
        //this.burst = new Burst(this.spread, burstSettings.offset, burstSettings.size, burstSettings.fireRate);
        //return ap;
    }
}
