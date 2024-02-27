using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : AbstractMovement
{
    public float prepTime;
    public float delay;
    private float time;
    private Vector3 tempLocation = new Vector3(9000, 9000, 9000);

    void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        delay += prepTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            time += Time.deltaTime;
            if (time < prepTime)
            {
                return;
            }
            if (time < delay)
            {
                transform.position = tempLocation;
                return;
            }
            transform.position = direction;
            isActive = false;
            return;
        }
    }
}
