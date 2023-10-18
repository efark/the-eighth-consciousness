using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*---------------------------------------------------------------------------------------
This code was adapted from an answer in Unity's public forum. (Originally published: 2022-10-22, accessed: 2023-10-17)
https://forum.unity.com/threads/sinusoidal-movement-of-the-projectile.1351121/#post-8529059
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
    // How much the bullet deviates to the sides.
    public float amplitude = 1f;

    // how stretched or expanded the sine wave is
    // if number > 1, wave will shrink (meaning it will take a shorter time to reach a full sin wave cycle) 
    // if number < 1 but > 0,  wave will stretch out (meaning it will take longer to reach a full sine wave cycle)
    public float waveFrequency = 2f;

    // Determines which direction the sine wave should go initially (e.g. left or right)
    public int waveStartingSide;
    public float waveSpeed;

    private Vector3 startPosition;
    private Vector3 forwardDirection, sineDirection;
    private float forwardSpeed, forwardProgress, sineProgress;

    void Start()
    {
        rb = transform.GetComponent<Rigidbody>();
        Destroy(gameObject, ttl);

        startPosition = this.transform.position;
        sineDirection = Vector3.Cross(Vector3.up, direction);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        forwardProgress += speed * Time.fixedDeltaTime;
        Vector3 position = startPosition + forwardProgress * direction;

        sineProgress += waveSpeed * Time.fixedDeltaTime;
        position += sineDirection * Mathf.Sin(sineProgress * waveFrequency) * amplitude * waveStartingSide;

        //Debug.Log($"{position}");
        this.transform.position = position;
    }

}
