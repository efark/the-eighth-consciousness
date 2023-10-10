using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player stats")]
    public float speed;
    public float tiltAngle;

    private Rigidbody rigidBody;

    void Start()
    {
        rigidBody = transform.GetComponent<Rigidbody>();

    }

    void FixedUpdate()
    {
        // Character movement.
        float moveVertical = Input.GetAxis("Vertical");
        float moveHorizontal = Input.GetAxis("Horizontal");

        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
        rigidBody.velocity = movement * speed;

        rigidBody.rotation = Quaternion.Euler(Vector3.forward * moveHorizontal * tiltAngle);
        //rigidBody.rotation = Quaternion.Euler(Vector3.right * 90);

    }
}
