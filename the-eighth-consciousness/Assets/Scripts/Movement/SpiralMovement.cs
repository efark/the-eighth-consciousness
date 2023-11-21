using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiralMovement : AbstractMovement
{
    /* Inherited from Abstract class:
    public float speed;
    public Vector2 direction;
    public Rigidbody rb;
    */
    [Header("Spiral parameters")]
    public float radius = 1f;
    public float spiralSpeed;
    //private float T = 3f;

    private Vector2 originalDirection;
    public Vector2 center;
    private float angle;

    void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        center = transform.position;
        //originalDirection = direction.normalized;
        //angle = Vector3.Angle(originalDirection, transform.forward);
        //Debug.Log($"originalDirection: {originalDirection}");
        //Debug.Log($"angle: {angle}");
    }

    public void Init()
    {
        angle = Vector2.Angle(direction, Vector2.up);
    }

    void FixedUpdate()
    {
        if (!isEnabled)
        {
            return;
        }

        
        angle += speed * Time.fixedDeltaTime;
        radius += spiralSpeed * Time.fixedDeltaTime;

        Vector2 offset = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * radius;
        transform.position = center + offset;        
        

    }

}
