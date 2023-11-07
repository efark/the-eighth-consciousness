using UnityEngine;

/*---------------------------------------------------------------------------------------
This code was adapted from the example for Extension methods in Unity Learn resources.
Spanish version: https://learn.unity.com/tutorial/metodos-de-extension# 
---------------------------------------------------------------------------------------*/
public static class ExtensionMethods
{
    /*
    /////////////////////////////////////
    Abstract bullet:
    public string targetType;
    public int player;
    public float speed;
    public int damage;
    public float ttl;
    public Vector2 direction;

    /////////////////////////////////////
    (Basic) Bullet:
    // No additional parameters.

    /////////////////////////////////////
    Accelerating bullet:
    public float acceleration;
    public float minSpeed;
    public float maxSpeed;

    /////////////////////////////////////
    Homing:
    public GameObject target;
    public float homingDelay;
    public float homingSpeed;
    public float homingDuration;

    /////////////////////////////////////
    Homing Propelled:
    public GameObject target;
    public float homingDelay;
    public float homingSpeed;
    public float homingDuration;
    public float force;
    public float InitialForce;

    /////////////////////////////////////
    Wavy Bullet:
    public float amplitude = 1f;
    public float waveFrequency = 2f;
    public int waveStartingSide;
    public float waveSpeed;     
     */

    // Simple Bullet.
    public static GameObject Instantiate(
        // Default parameters:
        GameObject original, Vector3 position, Quaternion rotation,
        // Inherited parameters:
        string targetType, int player, float speed, int damage, float ttl, Vector2 direction
        )
    {
        GameObject bullet = GameObject.Instantiate(original, position, rotation) as GameObject;
        BulletController bc = bullet.GetComponent<BulletController>();
        bc.targetType = targetType;
        bc.player = player;
        bc.speed = speed;
        bc.damage = damage;
        bc.ttl = ttl;
        bc.direction = direction;
        return bullet;
    }

    // Accelerating Bullet.
    public static GameObject Instantiate(
    // Default parameters:
    GameObject original, Vector3 position, Quaternion rotation,
    // Inherited parameters:
    string targetType, int player, float speed, int damage, float ttl, Vector2 direction,
    float acceleration, float minSpeed, float maxSpeed
    )
    {
        GameObject bullet = GameObject.Instantiate(original, position, rotation) as GameObject;
        AcceleratingBulletController abc = bullet.GetComponent<AcceleratingBulletController>();
        abc.targetType = targetType;
        abc.player = player;
        abc.speed = speed;
        abc.damage = damage;
        abc.ttl = ttl;
        abc.direction = direction;
        abc.acceleration = acceleration;
        abc.minSpeed = minSpeed;
        abc.maxSpeed = maxSpeed;
        return bullet;
    }
    
    public static GameObject Instantiate(
        // Default parameters:
        GameObject original, Vector3 position, Quaternion rotation,
        // Inherited parameters:
        string targetType, int player, float speed, int damage, float ttl, Vector2 direction,
        // Homing bullet parameters:
        GameObject target, float homingDelay, float homingSpeed, float homingDuration)
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
            GameObject original, Vector3 position, Quaternion rotation,
            // Inherited parameters:
            string targetType, int player, float speed, int damage, float ttl, Vector2 direction,
            // Homing bullet parameters:
            GameObject target, float homingDelay, float homingSpeed, float homingDuration, float force, float InitialForce)
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

    public static GameObject Instantiate(
        // Default parameters:
        GameObject original, Vector3 position, Quaternion rotation,
        // Inherited parameters:
        string targetType, int player, float speed, int damage, float ttl, Vector2 direction,
        // Wavy bullet parameters:
        float waveSpeed, float amplitude, float waveFrequency, bool waveStartsRight)
    {
        GameObject bullet = GameObject.Instantiate(original, position, rotation) as GameObject;
        WavyBulletController wbc = bullet.GetComponent<WavyBulletController>();
        wbc.targetType = targetType;
        wbc.player = player;
        wbc.speed = speed;
        wbc.damage = damage;
        wbc.ttl = ttl;
        wbc.direction = direction;
        wbc.waveSpeed = waveSpeed;
        wbc.amplitude = amplitude;
        wbc.waveFrequency = waveFrequency;
        wbc.waveStartingSide = waveStartsRight ? 1 : -1;
        return bullet;
    }
    
}