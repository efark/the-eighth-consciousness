using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractBullet : MonoBehaviour
{
    [Header("Basic Parameters")]
    public string targetType;
    public int player;
    public float speed;
    public int damage;
    public float ttl;
    // public float wavePeriod;

    public Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = transform.GetComponent<Rigidbody>();
        Destroy(gameObject, ttl);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (targetType.ToLower() == "enemy" && other.gameObject.tag.ToLower() == "enemy")
        {
            //other.transform.GetComponent<EnemyController>().AddHP(damage);
            Destroy(gameObject);
            return;
        }
        if (targetType.ToLower() == "player" && other.gameObject.tag.ToLower() == "player")
        {
            other.transform.GetComponent<PlayerController>().playerHP -= damage;
            Destroy(gameObject);
            return;
        }
    }
}
