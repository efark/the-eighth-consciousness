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

    private Vector2 center;
    private float angle;

    void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        center = transform.position;
        angle = GetAngleAmplitude(direction);
        angle *= Mathf.Deg2Rad;
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

        transform.rotation = Quaternion.LookRotation(Vector3.forward, offset);
    }

    private static float GetAngleAmplitude(Vector2 v)
    {
        float value = (float)((System.Math.Atan2(v.x, v.y) / System.Math.PI) * 180f);
        if (value < 0)
        {
            value += 360f;
        }
        return value;
    }

}