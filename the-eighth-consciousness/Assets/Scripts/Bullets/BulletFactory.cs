using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFactory : ObjectFactory
{
    private BulletSettings settings;
    private TargetTypes targetType;
    private int playerId;
    private bool alternate;

    public BulletFactory(BulletSettings _settings, TargetTypes _targetType, int _playerId)
    {
        settings = _settings;
        targetType = _targetType;
        playerId = _playerId;

        alternate = settings.mvSettings.isRightSided;
        
    }

    private GameObject FindTarget(Vector3 position)
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(targetType.ToString());

        if (targets.Length == 1)
        {
            return targets[0];
        }
        float min_distance = 0;
        GameObject closest = null;
        foreach (GameObject p in targets)
        {
            float dist = Vector3.Distance(position, p.transform.position);
            if (min_distance == 0)
            {
                min_distance = dist;
                closest = p;
                continue;
            }
            if (dist < min_distance)
            {
                min_distance = dist;
                closest = p;
                continue;
            }
        }
        return closest;
    }

    public GameObject Create(Vector3 position, Quaternion rotation, Vector2 direction)
    {
        GameObject bullet = GameObject.Instantiate(settings.prefab, position, rotation) as GameObject;
        BulletController bc = bullet.GetComponent<BulletController>();
        bc.targetType = targetType;
        bc.playerId = playerId;
        bc.damage = settings.damage;
        bc.ttl = settings.ttl;

        switch (settings.mvSettings.type)
        {
            case MovementTypes.StraightMovement:
                StraightMovement sm = bullet.GetComponent<StraightMovement>();
                sm.isEnabled = true;
                sm.speed = settings.mvSettings.speed;
                sm.direction = direction;
                return bullet;
            case MovementTypes.AcceleratingMovement:
                AcceleratingMovement am = bullet.GetComponent<AcceleratingMovement>();
                am.isEnabled = true;
                am.speed = settings.mvSettings.speed;
                am.direction = direction;
                am.acceleration = settings.mvSettings.acceleration;
                am.minSpeed = settings.mvSettings.minSpeed;
                am.maxSpeed = settings.mvSettings.maxSpeed;
                return bullet;
            case MovementTypes.HomingMovement:
                HomingMovement hm = bullet.GetComponent<HomingMovement>();
                hm.isEnabled = true;
                hm.speed = settings.mvSettings.speed;
                hm.direction = direction;
                hm.target = FindTarget(position);
                hm.homingDelay = settings.mvSettings.homingDelay;
                hm.homingSpeed = settings.mvSettings.homingSpeed;
                hm.homingDuration = settings.mvSettings.homingDuration;
                return bullet;
            case MovementTypes.PropelledHomingMovement:
                PropelledHomingMovement phm = bullet.GetComponent<PropelledHomingMovement>();
                phm.isEnabled = true;
                phm.speed = settings.mvSettings.speed;
                phm.direction = direction;
                phm.target = FindTarget(position);
                phm.homingDelay = settings.mvSettings.homingDelay;
                phm.homingSpeed = settings.mvSettings.homingSpeed;
                phm.homingDuration = settings.mvSettings.homingDuration;
                phm.force = settings.mvSettings.force;
                phm.initialForce = settings.mvSettings.initialForce;
                return bullet;
            case MovementTypes.WavyMovement:
                WavyMovement wm = bullet.GetComponent<WavyMovement>();
                wm.isEnabled = true;
                wm.speed = settings.mvSettings.speed;
                wm.direction = direction;
                wm.waveSpeed = settings.mvSettings.waveSpeed;
                wm.amplitude = settings.mvSettings.amplitude;
                wm.waveFrequency = settings.mvSettings.waveFrequency;
                wm.waveStartingSide = alternate ? 1 : -1;

                if (settings.mvSettings.isAlternating)
                {
                    alternate = !alternate;
                }
                return bullet;
            case MovementTypes.SpiralMovement:
                SpiralMovement sp = bullet.GetComponent<SpiralMovement>();
                sp.isEnabled = true;
                //sp.speed = settings.mvSettings.speed;
                sp.speed = settings.mvSettings.speed;
                sp.spiralSpeed = settings.mvSettings.spiralSpeed;
                sp.radius = settings.mvSettings.radius;
                return bullet;
            default:
                // code block
                return null;
        }
    }
}
