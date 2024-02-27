using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcceleratingMovement : AbstractMovement
{
    /* Inherited from Abstract class:
        public float speed;
        public Vector2 direction;
        public Vector2 destination;
        public bool isActive = true;
        [System.NonSerialized] public Rigidbody2D rb;
        public int order;
     */

    [Header("Acceleration parameters")]
    public float acceleration;
    public float minSpeed;
    public float maxSpeed;

    void FixedUpdate()
    {
        if (isActive)
        {
            speed = Mathf.Clamp(speed + acceleration * Time.fixedDeltaTime, minSpeed, maxSpeed);
            rb.velocity = direction * speed;
        }
    }
}
