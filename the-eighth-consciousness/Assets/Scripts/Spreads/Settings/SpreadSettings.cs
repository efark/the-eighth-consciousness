using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SpreadSettings : ScriptableObject
{
    [Header("Basic Parameters")]
    public SpreadTypes type;
    public int groupSize;

    [Header("RadialSpread")]
    public float spreadAngle;

    [Header("MultiSpread")]
    public int internalSpacing;
    public bool isAlternating;
    public bool isCentered;
}
