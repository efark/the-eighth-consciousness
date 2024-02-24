using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularMovement : AbstractMovement
{
    /* Inherited from Abstract class:
    public float speed;
    public Vector2 direction;
    public Rigidbody rb;
    */
    [Header("Circle parameters")]
    //public float radius = 1f;
    public float rotationSpeed;

    //private Vector2 centre;
    private float angle;

    void FixedUpdate()
    {
        if (!isEnabled)
        {
            return;
        }
        direction = rotateVector2(direction.normalized, rotationSpeed);
        rb.velocity = direction * (speed * 1.2f);
        transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
    }

    Vector2 rotateVector2(Vector2 vec, float angle)
    {
        float newAngle = Mathf.Atan2(vec.y, vec.x) + angle * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(newAngle), Mathf.Sin(newAngle));
    }
}
