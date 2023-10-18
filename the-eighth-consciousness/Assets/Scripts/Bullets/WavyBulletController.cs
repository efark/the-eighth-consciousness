using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*---------------------------------------------------------------------------------------
This code was adapted from the example for Extension methods in Unity Learn resources.
Spanish version: https://github.com/hlimbo/Space-Shooter-Tutorial/blob/main/Assets/Scripts/ShootPatterns/SinLaser.cs 
---------------------------------------------------------------------------------------*/


public class WavyBulletController : AbstractBullet
{
    /* Inherited from Abstract class:
    public string targetType;
    public int player;
    public float speed;
    public int damage;
    public float ttl;
    private Rigidbody rb;
    */
    [Header("Wave parameters")]
    public Vector3 direction;
    public float waveDistance = 4;
    public float thetaStep = Mathf.PI / 32f;
    public float theta = 0f;
    public float amplitude = 4f;

    // how stretched or expanded the sine wave is
    // if number > 1, wave will shrink (meaning it will take a shorter time to reach a full sin wave cycle) 
    // if number < 1 but > 0,  wave will stretch out (meaning it will take longer to reach a full sine wave cycle)
    public float waveFrequency = 2f;

    // Determines which direction the sine wave should go initially (e.g. left or right)
    public int waveDirection = 1;
    
    private float xOffset;

    private Vector3 startPosition;
    private Vector3 forwardDirection, sineDirection;
    private float forwardSpeed, forwardProgress, sineSpeed, sineProgress;

    void Start()
    {
        rb = transform.GetComponent<Rigidbody>();
        Destroy(gameObject, ttl);

        xOffset = transform.position.x;
        startPosition = this.transform.position;
        sineDirection = Vector3.Cross(Vector3.up, direction);
        sineSpeed = 10f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        /*
        Vector3 targetDirection = targetPlayer.transform.position - this.transform.position;
        float singleStep = rotationSpeed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
        Debug.DrawRay(transform.position, newDirection, Color.red);
        transform.rotation = Quaternion.LookRotation(newDirection);

        // go between 0 and 2pi
        // need a theta step every update
        // sin wave needs to move relative to the initial position it was shot from
        float newXPos = waveDirection * amplitude * Mathf.Sin(theta * waveFrequency) + xOffset;
        float xStep = newXPos - transform.position.x;
        Vector3 waveVector = new Vector3(xStep, 0, zOffset + speed * Time.deltaTime);
        //Vector3 translation = Quaternion.AngleAxis(Vector3.Angle(direction, new Vector3(1,0,1)), Vector3.up) * waveVector;
        //Vector3 newDirection = Vector3.RotateTowards(waveVector, targetDirection, 360f, 0.0f);

        transform.Translate(waveVector);

        theta += thetaStep;
        */

        forwardProgress += speed * Time.fixedDeltaTime;
        Vector3 position = startPosition + forwardProgress * direction;

        sineProgress += sineSpeed * Time.fixedDeltaTime;
        position += sineDirection * Mathf.Sin(sineProgress);

        //Debug.Log($"{position}");
        this.transform.position = position;

    }

}
