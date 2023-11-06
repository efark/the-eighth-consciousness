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
    public Vector3 direction;
     */
    [Header("")]
    public float acceleration;
    public float minSpeed;
    public float maxSpeed;

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = direction * speed;
    }

}
