using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BulletSettings : ScriptableObject
{
    [Header("Basic Parameters")]
    public GameObject prefab;
    public float speed;
    public int damage;
    public float ttl;
    public BulletTypes type;

    [Header("Acceleration")]
    public float acceleration;
    public float minSpeed;
    public float maxSpeed;

    [Header("Homing")]
    public float homingDelay;
    public float homingSpeed;
    public float homingDuration;

    [Header("Propelled")]
    public float force;
    public float initialForce;

    [Header("Wave")]
    public float amplitude;
    public float waveFrequency;
    public float waveSpeed;

}
