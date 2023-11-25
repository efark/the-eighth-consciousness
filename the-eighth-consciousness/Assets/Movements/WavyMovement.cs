﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*---------------------------------------------------------------------------------------
This code was adapted from an answer in Unity's public forum. (Originally published: 2022-10-22, accessed: 2023-10-17)
https://forum.unity.com/threads/sinusoidal-movement-of-the-projectile.1351121/#post-8529059
---------------------------------------------------------------------------------------*/
public class WavyMovement : AbstractMovement
{
    /* Inherited from Abstract class:
    public float speed;
    public Vector2 direction;
    public Rigidbody rb;
    */
    [Header("Wave parameters")]
    [Tooltip("How much the bullet deviates to the sides.")]
    public float amplitude = 1f;

    [Tooltip(@"how stretched or expanded the sine wave is
if number > 1, wave will shrink (meaning it will take a shorter time to reach a full sin wave cycle) 
if number < 1 but > 0,  wave will stretch out (meaning it will take longer to reach a full sine wave cycle)")]
    public float waveFrequency = 2f;

    // Determines which direction the sine wave should go initially (e.g. left or right)
    public int waveStartingSide;
    public float waveSpeed;

    private Vector3 startPosition;
    private Vector3 v3Direction;
    private Vector3 forwardDirection, sineDirection;
    private float forwardSpeed, forwardProgress, sineProgress;
    private Vector3 crossDirection;
    private float time;

    void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>();

        startPosition = this.transform.position;
        // v3Direction = new Vector3(direction.x, 0, direction.y);
        // sineDirection = Vector3.Cross(Vector3.up, v3Direction);
        crossDirection = new Vector3(direction.y, -direction.x, 0);
        time = 0.0f;
        // Debug.Log($"sineDirection: {sineDirection}");
        direction = direction.normalized;
        crossDirection = new Vector2(direction.y, -direction.x); // A quick right angle for 2D
        startPosition = transform.position;
    }

    void FixedUpdate()
    {
        if (!isEnabled)
        {
            return;
        }
        //forwardProgress += speed * Time.fixedDeltaTime;
        //Vector3 position = startPosition + forwardProgress * new Vector3(v3Direction.x, v3Direction.z, 0);

        //sineProgress += waveSpeed * Time.fixedDeltaTime;
        //position += sineDirection * Mathf.Sin(sineProgress * waveFrequency) * amplitude * waveStartingSide;

        //Vector3 position = startPosition + (new Vector3(direction.x, direction.y, 0).normalized * speed * time);
        //position += sineDirection * Mathf.Sin(time * waveFrequency) * amplitude * waveStartingSide;
        //transform.position = position;

        transform.position = startPosition + (new Vector3(direction.x, direction.y, 0) * speed * time);
        transform.position += (crossDirection * Mathf.Sin(time * waveFrequency)) * amplitude * waveStartingSide;
        time += Time.fixedDeltaTime;
        //Debug.Log($"{position}");
        //this.transform.position = position;
    }
}