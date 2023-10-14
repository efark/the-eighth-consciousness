using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingPropelledBulletController : AbstractBullet
{
    /* Inherited from Abstract class:
    public string targetType;
    public int player;
    public float speed;
    public int damage;
    public float ttl;
    private Rigidbody rb;
    */
    [Header("Homing parameters")]
    public Vector3 direction;
    public GameObject target;
    public float homingDelay;
    public float homingSpeed;
    public float homingDuration;
    public float force;
    public float InitialForce;

    private float homingAccumTime = 0f;
    private float homingDelayAccumTime = 0f;
    private bool homingStarted = false;
    private bool isHoming = false;

    void Start()
    {
        rb = transform.GetComponent<Rigidbody>();
        Destroy(gameObject, ttl);

        rb.AddForce(direction.normalized * InitialForce);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (!homingStarted)
        {
            homingDelayAccumTime += Time.fixedDeltaTime;
            if (homingDelayAccumTime >= homingDelay)
            {
                //Debug.Log("Start homing!");
                homingStarted = true;
                isHoming = true;
            }
        }

        if (isHoming)
        {
            homingAccumTime += Time.fixedDeltaTime;
            if (homingAccumTime >= homingDuration)
            {
                isHoming = false;
                //Debug.Log("Stop homing!");
            }
            Vector3 targetDirection = target.transform.position - this.transform.position;
            float singleStep = homingSpeed * Time.fixedDeltaTime;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
            Debug.DrawRay(transform.position, newDirection, Color.red);
            transform.rotation = Quaternion.LookRotation(newDirection);
            //rb.velocity = newDirection * speed;
            rb.AddForce(newDirection.normalized * force);
        }


    }
}
