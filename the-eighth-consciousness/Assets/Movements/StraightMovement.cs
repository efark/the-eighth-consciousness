﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightMovement : AbstractMovement
{
    /* Inherited from Abstract class:
    public float speed;
    public Vector2 direction;
    public Rigidbody rb;
    */
    void FixedUpdate()
    {
        if (!isEnabled)
        {
            return;
        }
        rb.velocity = this.transform.forward * speed * Time.fixedDeltaTime;
    }
}
