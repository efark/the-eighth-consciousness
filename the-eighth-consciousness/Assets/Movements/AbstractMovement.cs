using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementTypes
{
    AcceleratingMovement,
    HomingMovement,
    PropelledHomingMovement,
    WavyMovement,
    SpiralMovement,
    CircularMovement,
    Teleport
}


public class AbstractMovement : MonoBehaviour
{
    [Header("Basic Parameters")]
    public float speed;
    public Vector2 direction;
    public Vector2 destination;
    public bool isActive;
    [System.NonSerialized] public Rigidbody2D rb;
    public int order;

    public bool IsActive
    {
        get { return isActive; }
        set {
            rb.velocity = direction * (value ? speed : 0f);
            isActive = value;
        }
    }

    void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>();
    }

}
