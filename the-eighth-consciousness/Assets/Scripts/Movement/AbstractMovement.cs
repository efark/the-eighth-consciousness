using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementTypes
{
    AcceleratingMovement,
    StraightMovement,
    HomingMovement,
    PropelledHomingMovement,
    WavyMovement
}


public class AbstractMovement : MonoBehaviour
{
    [Header("Basic Parameters")]
    public float speed;
    public Vector2 direction;
    public Rigidbody2D rb;
    public bool isEnabled;

    public bool active
    {
        get
        {
            return isEnabled;
        }
        set
        {
            isEnabled = value;
        }
    }


    void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>();
    }

}
