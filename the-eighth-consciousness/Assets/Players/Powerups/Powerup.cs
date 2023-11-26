using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerupTypes
{ 
    HP,
    FirePower
}

public class Powerup : MonoBehaviour
{
    public PowerupTypes powerupType;
    public float ttl;

    void Start()
    {
        Destroy(gameObject, ttl);
    }

    void OnDestroy()
    { 
    
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other == null)
        {
            return;
        }
        if (other.gameObject.tag.ToLower() == "enemy")
        {
            return;
        }
        if (other.gameObject.tag.ToLower() == "player")
        {
            if (powerupType == PowerupTypes.HP)
            {
                other.transform.GetComponent<PlayerController>().stats.UpdateHP(25);
                Destroy(gameObject);
                return;
            }
            if (powerupType == PowerupTypes.FirePower)
            {
                other.transform.GetComponent<PlayerController>().stats.UpdateFirePower(1);
                Destroy(gameObject);
                return;
            }
        }
    }

}
