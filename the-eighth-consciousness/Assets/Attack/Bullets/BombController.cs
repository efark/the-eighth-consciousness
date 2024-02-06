using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    public float ttl;
    public int playerId;
    public int damage;
    public float scaleFactor = 0f;
    private Vector3 scaleChange;

    void Start()
    {
        Destroy(gameObject, ttl);
        scaleChange = new Vector3(scaleFactor, scaleFactor, scaleFactor);
    }

    void FixedUpdate()
    {
        transform.localScale += scaleChange * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.ToLower() == "enemybullet")
        {
            Destroy(other.gameObject);
        }
        if (other.tag.ToLower() == "enemy" || other.tag.ToLower() == "boss")
        {
            other.transform.GetComponent<AbstractEnemyController>().Hit(this.playerId, -damage);
        }
    }
}
