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

    private Vector2 centre;
    private float angle;

    void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        centre = transform.position;
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
        transform.position = centre + offset;
    }
}
