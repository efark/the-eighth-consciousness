using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMovement : AbstractMovement
{
    /* Inherited from Abstract class:
        public float speed;
        public Vector2 direction;
        public Rigidbody rb;
    */
    [Header("Homing parameters")]
    public float acceleration;
    public float minSpeed;
    public float maxSpeed;
    public GameObject target;
    public float homingDelay;
    public float homingSpeed;
    public float homingDuration;

    private float homingAccumTime = 0f;
    private float homingDelayAccumTime = 0f;
    private bool homingStarted = false;
    private bool isHoming = false;
    private Vector3 lastDirection;

    void FixedUpdate()
    {
        Debug.Log($"speed: {speed}");
        if (!isEnabled)
        {
            return;
        }

        if (!homingStarted)
        {
            homingDelayAccumTime += Time.fixedDeltaTime;
            if (homingDelayAccumTime >= homingDelay)
            {
                homingStarted = true;
                isHoming = true;
            }
        }

        if (isHoming)
        {
            if (target == null)
            {
                return;
            }
            homingAccumTime += Time.fixedDeltaTime;
            if (homingAccumTime >= homingDuration)
            {
                isHoming = false;
            }
            if (acceleration != 0)
            {
                speed = Mathf.Clamp(speed + (acceleration * Time.fixedDeltaTime), minSpeed, maxSpeed);
            }
            Vector3 newDirection = lastDirection;
            float singleStep = homingSpeed * Time.fixedDeltaTime;
            if (target != null)
            {
                Vector3 targetDirection = target.transform.position - this.transform.position;
                newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
                lastDirection = newDirection.normalized;
            }
            // Debug.DrawRay(transform.position, newDirection, Color.red);

            transform.rotation = Quaternion.LookRotation(Vector3.forward, newDirection);
            rb.velocity = lastDirection * speed;
            return;
        }
        rb.velocity = direction.normalized * speed;
    }
}
