using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public TargetTypes targetType;
    public int playerId;
    public int damage;
    public float ttl;

    void Start()
    {
        Destroy(gameObject, ttl);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other == null)
        {
            return;
        }
        if (targetType == TargetTypes.Enemy && (other.gameObject.tag.ToLower() == "enemy" || other.gameObject.tag.ToLower() == "boss"))
        {
            other.transform.GetComponent<AbstractEnemyController>().Hit(this.playerId, -damage);
            Destroy(gameObject);
            return;
        }
        if (targetType == TargetTypes.Player && other.gameObject.tag.ToLower() == "player")
        {
            other.transform.GetComponent<PlayerController>().stats.UpdateHP(-damage);
            Destroy(gameObject);
            return;
        }
    }

}
