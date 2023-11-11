using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BurstSettings : ScriptableObject
{
    [Header("Basic Parameters")]
    public float offset;
    public int size;
    public float fireRate;

}