using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    public float ttl;
    public int damage;

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
        if (other.tag.ToLower() == "enemybullet" || other.tag.ToLower() == "enemy")
        {
            Destroy(other.gameObject);
        }
        if (other.tag.ToLower() == "boss")
        {
            other.transform.GetComponent<AbstractEnemyController>().HP = -damage;
        }
    }
}
