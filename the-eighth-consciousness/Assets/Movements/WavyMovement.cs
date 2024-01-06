using System.Collections;
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

        time = 0.0f;
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

        transform.position = startPosition + (new Vector3(direction.x, direction.y, 0) * speed * time);
        transform.position += (crossDirection * Mathf.Sin(time * waveFrequency)) * amplitude * waveStartingSide;
        time += Time.fixedDeltaTime;
    }
}
