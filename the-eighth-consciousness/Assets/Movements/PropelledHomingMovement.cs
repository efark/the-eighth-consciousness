using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropelledHomingMovement : AbstractMovement
{
    /* Inherited from Abstract class:
        public float speed;
        public Vector2 direction;
        private Rigidbody rb;
    */

    [Header("Homing parameters")]
    public GameObject target;
    public float homingDelay;
    public float homingSpeed;
    public float homingDuration;
    public float force;
    public float initialForce;

    private float homingAccumTime = 0f;
    private float homingDelayAccumTime = 0f;
    private bool homingStarted = false;
    private bool isHoming = false;
    private Vector3 lastDirection;

    void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        rb.AddForce(direction.normalized * initialForce);
    }

    void FixedUpdate()
    {
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
            Vector3 newDirection = lastDirection;
            if (target != null)
            {
                Vector3 targetDirection = target.transform.position - this.transform.position;
                float singleStep = homingSpeed * Time.fixedDeltaTime;
                newDirection = Vector3.RotateTowards(transform.up, targetDirection, singleStep, 0.0f);
                lastDirection = newDirection;
            }
            // Debug.DrawRay(transform.position, newDirection, Color.red);
            transform.rotation = Quaternion.LookRotation(Vector3.forward, newDirection);

            Debug.Log($"lastDirection: {lastDirection}");
            rb.AddForce(lastDirection.normalized * force);
        }


    }
}
