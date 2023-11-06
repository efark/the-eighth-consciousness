using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : AbstractBullet
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

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = direction * speed;
    }

}
