using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MovementSettings : ScriptableObject
{
    [Header("Basic Parameters")]
    public float speed;
    public MovementTypes type;

    [Header("Acceleration")]
    public float acceleration;
    public float minSpeed;
    public float maxSpeed;

    [Header("Homing")]
    public float homingDelay;
    public float homingSpeed;
    public float homingDuration;

    [Header("Propelled Homing")]
    public float force;
    public float initialForce;

    [Header("Wave")]
    public float amplitude;
    public float waveFrequency;
    public float waveSpeed;
    public bool isAlternating;
    public bool isRightSided;

    [Header("Spiral")]
    public float radius;
    public float spiralSpeed;
    
    [Header("Circular")]
    public float rotationSpeed;
}
