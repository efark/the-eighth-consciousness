using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcceleratingBulletController : AbstractBullet
{
    /* Inherited from Abstract class:
    public string targetType;
    public int player;
    public float speed;
    public int damage;
    public float ttl;
    private Rigidbody rb;
    public Vector2 direction;
     */
    [Header("Acceleration parameters")]
    public float acceleration;
    public float minSpeed;
    public float maxSpeed;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (acceleration > 0)
        {
            if (maxSpeed != speed)
            {
                speed = Mathf.Min(maxSpeed, speed * acceleration * Time.fixedDeltaTime);
            }
        }
        if (acceleration < 0)
        {
            if (minSpeed != speed)
            {
                speed = Mathf.Max(minSpeed, speed * acceleration * Time.fixedDeltaTime);
            }

        }
        rb.velocity = direction * speed;
    }

}
