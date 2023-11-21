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
    private float T = 3f;

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

        radius += spiralSpeed * Time.fixedDeltaTime;
        angle += speed * Time.fixedDeltaTime * 1.1455f;

        Vector2 offset = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * radius;
        transform.position = centre + offset;

        //direction = rotateVector2(direction.normalized, 10f);
        //rb.velocity = direction * (speed * 1.2f);
        //rb.rotation += 45f * Time.fixedDeltaTime;
        //rb.MoveRotation(rb.rotation + spiralSpeed * Time.fixedDeltaTime);

    }

    Vector2 rotateVector2(Vector2 vec, float angle)
    {
        float newAngle = Mathf.Atan2(vec.y, vec.x) + angle * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(newAngle), Mathf.Sin(newAngle));
    }
}
