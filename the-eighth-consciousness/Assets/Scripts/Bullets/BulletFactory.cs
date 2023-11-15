using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFactory : ObjectFactory
{
    private BulletSettings settings;

    public BulletFactory(BulletSettings _settings)
    {
        settings = _settings;
    }

    public GameObject Create(Vector3 position, Quaternion rotation, Vector2 direction)
    {
        GameObject bullet = GameObject.Instantiate(settings.prefab, position, rotation) as GameObject;
        BulletController bc = bullet.GetComponent<BulletController>();
        bc.targetType = settings.targetType;
        bc.playerId = settings.playerId;
        bc.damage = settings.damage;
        bc.ttl = settings.ttl;

        switch (settings.mvSettings.type)
        {
            case MovementTypes.StraightMovement:
                StraightMovement sm = bullet.GetComponent<StraightMovement>();
                sm.speed = settings.mvSettings.speed;
                sm.direction = direction;
                return bullet;
            case MovementTypes.AcceleratingMovement:
                AcceleratingMovement am = bullet.GetComponent<AcceleratingMovement>();
                am.speed = settings.mvSettings.speed;
                am.direction = direction;
                am.acceleration = settings.mvSettings.acceleration;
                am.minSpeed = settings.mvSettings.minSpeed;
                am.maxSpeed = settings.mvSettings.maxSpeed;
                return bullet;
            /* case MovementTypes.HomingBullet:
                bullet = GameObject.Instantiate(settings.prefab, position, rotation) as GameObject;
                HomingBulletController hbc = bullet.GetComponent<HomingBulletController>();
                hbc.speed = settings.speed;
                hbc.damage = settings.damage;
                hbc.ttl = settings.ttl;
                hbc.direction = direction;
                hbc.target = additionals.target;
                hbc.homingDelay = settings.homingDelay;
                hbc.homingSpeed = settings.homingSpeed;
                hbc.homingDuration = settings.homingDuration;
                return bullet;
            case MovementTypes.HomingPropelledBullet:
                bullet = GameObject.Instantiate(settings.prefab, position, rotation) as GameObject;
                HomingPropelledBulletController hpbc = bullet.GetComponent<HomingPropelledBulletController>();
                hpbc.targetType = targetType;
                hpbc.player = player;
                hpbc.speed = settings.speed;
                hpbc.damage = settings.damage;
                hpbc.ttl = settings.ttl;
                hpbc.direction = direction;
                hpbc.target = additionals.target;
                hpbc.homingDelay = settings.homingDelay;
                hpbc.homingSpeed = settings.homingSpeed;
                hpbc.homingDuration = settings.homingDuration;
                hpbc.force = settings.force;
                hpbc.initialForce = settings.initialForce;
                return bullet;
            case MovementTypes.WavyBullet:
                bullet = GameObject.Instantiate(settings.prefab, position, rotation) as GameObject;
                WavyBulletController wbc = bullet.GetComponent<WavyBulletController>();
                wbc.targetType = targetType;
                wbc.player = player;
                wbc.speed = settings.speed;
                wbc.damage = settings.damage;
                wbc.ttl = settings.ttl;
                wbc.direction = direction;
                wbc.waveSpeed = settings.waveSpeed;
                wbc.amplitude = settings.amplitude;
                wbc.waveFrequency = settings.waveFrequency;
                wbc.waveStartingSide = additionals.alternate ? 1 : -1;
                return bullet;*/
            default:
                // code block
                return null;
        }
    }
}
