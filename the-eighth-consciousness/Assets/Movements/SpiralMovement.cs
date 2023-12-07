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
        //center = transform.position;
        angle = GetAngleMeasure(direction, center);
        /*if (angle >= 180)
        {
            angle = -angle;
            speed *= -1;
        }*/
        Debug.Log($"center: {center} - direction: {direction} - angle: {angle}");
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

    private static float GetAngleMeasure(Vector2 from, Vector2 to)
    {
        float x = from.x - to.x;
        float y = from.y - to.y;
        float value = (float)((System.Math.Atan2(x, y) / System.Math.PI) * 180f);
        if (value < 0)
        {
            value += 360f;
        }
        return value;
    }

}