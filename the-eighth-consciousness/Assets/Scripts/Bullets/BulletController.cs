using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : AbstractBullet
{
    
    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = transform.forward * speed;
    }

}