using UnityEngine;

/*---------------------------------------------------------------------------------------
This code was adapted from the example for Extension methods in Unity Learn resources.
Spanish version: https://learn.unity.com/tutorial/metodos-de-extension# 
---------------------------------------------------------------------------------------*/
public static class ExtensionMethod
{
    /* from the Abstract class.
    public string targetType;
    public int player;
    public float speed;
    public int damage;
    public float ttl;

    Directional bullet:
    public Vector3 direction;
    public float acceleration;
    public float accelerationDelay;

    Homing:
    public Vector3 direction;
    public GameObject target;
    public float homingDelay;
    public float homingSpeed;
    public float homingDuration;

    Homing Propelled:
    public Vector3 direction;
    public GameObject target;
    public float homingDelay;
    public float homingSpeed;
    public float homingDuration;
    public float force;
    public float InitialForce;
     */

    public static GameObject Instantiate(
        // Default parameters:
        this GameObject thisObj, GameObject original, Vector3 position, Quaternion rotation,
        // Inherited parameters:
        string targetType, int player, float speed, int damage, float ttl,
        // Directional bullet parameters:
        Vector3 direction)
    {
        GameObject bullet = GameObject.Instantiate(original, position, rotation) as GameObject;
        DirectionalBulletController dbc = bullet.GetComponent<DirectionalBulletController>();
        dbc.targetType = targetType;
        dbc.player = player;
        dbc.speed = speed;
        dbc.damage = damage;
        dbc.ttl = ttl;
        dbc.direction = direction;
        return bullet;
    }
    
    public static GameObject Instantiate(
        // Default parameters:
        this GameObject thisObj, GameObject original, Vector3 position, Quaternion rotation,
        // Inherited parameters:
        string targetType, int player, float speed, int damage, float ttl,
        // Homing bullet parameters:
        Vector3 direction, GameObject target, float homingDelay, float homingSpeed, float homingDuration)
        {
            GameObject bullet = GameObject.Instantiate(original, position, rotation) as GameObject;
            HomingBulletController hbc = bullet.GetComponent<HomingBulletController>();
            hbc.targetType = targetType;
            hbc.player = player;
            hbc.speed = speed;
            hbc.damage = damage;
            hbc.ttl = ttl;
            hbc.direction = direction;
            hbc.target = target;
            hbc.homingDelay = homingDelay;
            hbc.homingSpeed = homingSpeed;
            hbc.homingDuration = homingDuration;
        return bullet;
        }

    public static GameObject Instantiate(
            // Default parameters:
            this GameObject thisObj, GameObject original, Vector3 position, Quaternion rotation,
            // Inherited parameters:
            string targetType, int player, float speed, int damage, float ttl,
            // Homing bullet parameters:
            Vector3 direction, GameObject target, float homingDelay, float homingSpeed, float homingDuration, float force, float InitialForce)
    {
        GameObject bullet = GameObject.Instantiate(original, position, rotation) as GameObject;
        HomingPropelledBulletController hpbc = bullet.GetComponent<HomingPropelledBulletController>();
        hpbc.targetType = targetType;
        hpbc.player = player;
        hpbc.speed = speed;
        hpbc.damage = damage;
        hpbc.ttl = ttl;
        hpbc.direction = direction;
        hpbc.target = target;
        hpbc.homingDelay = homingDelay;
        hpbc.homingSpeed = homingSpeed;
        hpbc.homingDuration = homingDuration;
        hpbc.force = force;
        hpbc.InitialForce = InitialForce;
        return bullet;
    }
}
/*---------------------------------------------------------------------------------------
 End of quoted code.
---------------------------------------------------------------------------------------*/