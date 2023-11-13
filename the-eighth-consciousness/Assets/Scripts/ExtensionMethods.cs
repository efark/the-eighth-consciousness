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

    public static GameObject Instantiate(
        BulletSettings bulletSettings,
        string targetType,
        int player,
        Vector3 position,
        Quaternion rotation,
        Vector2 direction,
        AdditionalBulletSettings additionals
        )
    {
        GameObject bullet;
        switch (bulletSettings.type)
        {
            case BulletTypes.AcceleratingBullet:
                bullet = GameObject.Instantiate(bulletSettings.prefab, position, rotation) as GameObject;
                AcceleratingBulletController abc = bullet.GetComponent<AcceleratingBulletController>();
                abc.targetType = targetType;
                abc.player = player;
                abc.speed = bulletSettings.speed;
                abc.damage = bulletSettings.damage;
                abc.ttl = bulletSettings.ttl;
                abc.direction = direction;
                abc.acceleration = bulletSettings.acceleration;
                abc.minSpeed = bulletSettings.minSpeed;
                abc.maxSpeed = bulletSettings.maxSpeed;
                return bullet;
            case BulletTypes.Bullet:
                bullet = GameObject.Instantiate(bulletSettings.prefab, position, rotation) as GameObject;
                BulletController bc = bullet.GetComponent<BulletController>();
                bc.targetType = targetType;
                bc.player = player;
                bc.speed = bulletSettings.speed;
                bc.damage = bulletSettings.damage;
                bc.ttl = bulletSettings.ttl;
                bc.direction = direction;
                return bullet;
            case BulletTypes.HomingBullet:
                bullet = GameObject.Instantiate(bulletSettings.prefab, position, rotation) as GameObject;
                HomingBulletController hbc = bullet.GetComponent<HomingBulletController>();
                hbc.targetType = targetType;
                hbc.player = player;
                hbc.speed = bulletSettings.speed;
                hbc.damage = bulletSettings.damage;
                hbc.ttl = bulletSettings.ttl;
                hbc.direction = direction;
                hbc.target = additionals.target;
                hbc.homingDelay = bulletSettings.homingDelay;
                hbc.homingSpeed = bulletSettings.homingSpeed;
                hbc.homingDuration = bulletSettings.homingDuration;
                return bullet;
            case BulletTypes.HomingPropelledBullet:
                bullet = GameObject.Instantiate(bulletSettings.prefab, position, rotation) as GameObject;
                HomingPropelledBulletController hpbc = bullet.GetComponent<HomingPropelledBulletController>();
                hpbc.targetType = targetType;
                hpbc.player = player;
                hpbc.speed = bulletSettings.speed;
                hpbc.damage = bulletSettings.damage;
                hpbc.ttl = bulletSettings.ttl;
                hpbc.direction = direction;
                hpbc.target = additionals.target;
                hpbc.homingDelay = bulletSettings.homingDelay;
                hpbc.homingSpeed = bulletSettings.homingSpeed;
                hpbc.homingDuration = bulletSettings.homingDuration;
                hpbc.force = bulletSettings.force;
                hpbc.initialForce = bulletSettings.initialForce;
                return bullet;
            case BulletTypes.WavyBullet:
                bullet = GameObject.Instantiate(bulletSettings.prefab, position, rotation) as GameObject;
                WavyBulletController wbc = bullet.GetComponent<WavyBulletController>();
                wbc.targetType = targetType;
                wbc.player = player;
                wbc.speed = bulletSettings.speed;
                wbc.damage = bulletSettings.damage;
                wbc.ttl = bulletSettings.ttl;
                wbc.direction = direction;
                wbc.waveSpeed = bulletSettings.waveSpeed;
                wbc.amplitude = bulletSettings.amplitude;
                wbc.waveFrequency = bulletSettings.waveFrequency;
                wbc.waveStartingSide = additionals.alternate ? 1 : -1;
                return bullet;
            default:
                // code block
                return null;
        }
    }


    public static AbstractShotSpread InitShotSpread(
        SpreadSettings spreadSettings,
        string targetType,
        int playerId
        )
    {
        AbstractShotSpread spreadShot;
        switch (spreadSettings.type)
        {
            case ShotSpreadTypes.RadialSpread:
                spreadShot = new RadialSpread(spreadSettings.bulletSettings, targetType, playerId,
        spreadSettings.roundSize, spreadSettings.spreadAngle);
                return spreadShot;
            case ShotSpreadTypes.MultiShotSpread:
                spreadShot = new MultiShotSpread(spreadSettings.bulletSettings, targetType, playerId,
        spreadSettings.roundSize, spreadSettings.roundSpacing, spreadSettings.isAlternating);
                return spreadShot;
            default:
                return null;
        }
    }
}