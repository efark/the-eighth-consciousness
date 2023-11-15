using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementTypes
{
    AcceleratingMovement,
    StraightMovement,
    HomingMovement,
    PropelledHomingMovement,
    WavyMovement,
    SpiralMovement
}


public class AbstractMovement : MonoBehaviour
{
    [Header("Basic Parameters")]
    public float speed;
    public Vector2 direction;
    public bool isEnabled;
    [System.NonSerialized] public Rigidbody2D rb;

    public bool IsEnabled
    {
        get { return isEnabled; }
        set { isEnabled = value; }
    }

    void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>();
    }

}
