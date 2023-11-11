using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SpreadSettings : ScriptableObject
{
    [Header("Basic Parameters")]
    public ShotSpreadTypes type;
    public BulletSettings bulletSettings;
    public int roundSize;

    [Header("RadialSpread")]
    public float spreadAngle;

    [Header("MultiShotSpread")]
    public int roundSpacing;
    public bool isAlternating;

}
