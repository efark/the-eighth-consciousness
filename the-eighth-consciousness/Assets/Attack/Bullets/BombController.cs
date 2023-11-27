using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    public float ttl;
    private Vector3 scaleChange = new Vector3(4f, 4f, 4f);
    void Start()
    {
        Destroy(gameObject, ttl);
    }

    void FixedUpdate()
    {
        transform.localScale += scaleChange * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Bomb - Collision - other: {other.tag.ToLower()}");
        if (other.tag.ToLower() == "enemybullet" || other.tag.ToLower() == "enemy")
        {
            Destroy(other.gameObject);
        }
    }
}
