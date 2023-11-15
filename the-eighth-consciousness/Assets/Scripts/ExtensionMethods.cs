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
    /*
    public static GameObject Instantiate(
        GameObject prefab,
        ObjectTypes objType,
        MovementSettings movementSettings,
        Vector3 position,
        Quaternion rotation,
        Vector2 direction
        )
    {
        if (objType == ObjectTypes.Bullet)
        {
            GameObject bullet = GameObject.Instantiate(prefab, position, rotation) as GameObject;
            BulletController bc = bullet.GetComponent<BulletController>();
            bc.targetType = bulletSettings.targetType;
            bc.playerId = bulletSettings.playerId;
            bc.damage = bulletSettings.damage;
            bc.ttl = bulletSettings.ttl;

            switch (bulletSettings.movementSettings.type)
            {
                case MovementTypes.AcceleratingMovement:
                    AcceleratingMovement am = bullet.GetComponent<AcceleratingMovement>();
                    am.speed = bulletSettings.movementSettings.speed;
                    am.direction = direction;
                    am.acceleration = bulletSettings.movementSettings.acceleration;
                    am.minSpeed = bulletSettings.movementSettings.minSpeed;
                    am.maxSpeed = bulletSettings.movementSettings.maxSpeed;
                    return bullet;
                case MovementTypes.StraightMovement:
                    StraightMovement sm = bullet.GetComponent<StraightMovement>();
                    sm.speed = bulletSettings.movementSettings.speed;
                    sm.direction = direction;
                    return bullet;
                 case MovementTypes.HomingBullet:
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
                case MovementTypes.HomingPropelledBullet:
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
                case MovementTypes.WavyBullet:
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
        if (objType == ObjectTypes.Enemy)
        {
            return null;
        }

    }*/

    public static AbstractSpread InitSpread(
        ObjectFactory factory,
        SpreadSettings settings
        )
    {
        AbstractSpread spreadShot;
        switch (settings.type)
        {
            case SpreadTypes.RadialSpread:
                spreadShot = new RadialSpread(factory, settings.groupSize, settings.spreadAngle);
                return spreadShot;
            case SpreadTypes.MultiSpread:
                spreadShot = new MultiSpread(factory, settings.groupSize, settings.internalSpacing);
                return spreadShot;
            default:
                return null;
        }
    }
    
}

// IFactory _factory, TargetTypes _targetTypes, int _playerId, int _groupSize, int _internalSpacing, bool _isAlternating