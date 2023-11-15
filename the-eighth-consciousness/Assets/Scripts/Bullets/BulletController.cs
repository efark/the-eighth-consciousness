using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSettings
{
    public MovementSettings mvSettings;
    public GameObject prefab;
    public TargetTypes targetType;
    public int playerId;
    public int damage;
    public float ttl;
}

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
        if (targetType == TargetTypes.Enemy && other.gameObject.tag.ToLower() == "enemy")
        {
            //other.transform.GetComponent<EnemyController>().AddHP(damage);
            //Add points to player's score.
            Destroy(gameObject);
            return;
        }
        if (targetType == TargetTypes.Player && other.gameObject.tag.ToLower() == "player")
        {
            other.transform.GetComponent<PlayerController>().playerHP -= damage;
            Destroy(gameObject);
            return;
        }
    }

}
