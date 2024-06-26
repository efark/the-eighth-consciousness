﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFactory : ObjectFactory
{
    private float offset;
    private BulletSettings settings;
    private TargetTypes targetType;
    private int playerId;
    private bool alternate;
    private float factor;

    public BulletFactory(BulletSettings _settings, TargetTypes _targetType, int _playerId, float _offset, float _factor)
    {
        this.settings = _settings;
        this.targetType = _targetType;
        this.playerId = _playerId;
        this.offset = _offset;
        this.factor = _factor;

        this.alternate = this.settings.mvSettings.isRightSided;
    }

    public void UpdateFactor(float newFactor)
    { 
        this.factor = newFactor;
    }

    public GameObject Create(Vector3 position, Quaternion rotation, Vector2 direction)
    {
        Vector2 center = new Vector2(direction.x, direction.y);
        position += new Vector3(direction.normalized.x, direction.normalized.y, 0) * offset;
        GameObject bullet = GameObject.Instantiate(settings.prefab, position, rotation) as GameObject;
        bullet.tag = targetType.ToString().ToLower() == "player" ? "EnemyBullet" : "PlayerBullet";
        BulletController bc = bullet.GetComponent<BulletController>();
        bc.targetType = targetType;
        bc.playerId = playerId;
        bc.damage = (int)(settings.damage * factor);
        bc.ttl = settings.ttl;

        switch (settings.mvSettings.type)
        {
            case MovementTypes.AcceleratingMovement:
                AcceleratingMovement am = bullet.GetComponent<AcceleratingMovement>();
                am.isActive = true;
                am.speed = settings.mvSettings.speed;
                am.direction = direction;
                am.acceleration = settings.mvSettings.acceleration;
                am.minSpeed = settings.mvSettings.minSpeed;
                am.maxSpeed = settings.mvSettings.maxSpeed;
                return bullet;
            case MovementTypes.HomingMovement:
                HomingMovement hm = bullet.GetComponent<HomingMovement>();
                hm.isActive = true;
                hm.speed = settings.mvSettings.speed;
                hm.direction = direction;
                hm.target = AuxiliaryMethods.FindTarget(targetType.ToString(), position);
                hm.acceleration = settings.mvSettings.acceleration;
                hm.minSpeed = settings.mvSettings.minSpeed;
                hm.maxSpeed = settings.mvSettings.maxSpeed;
                hm.homingDelay = settings.mvSettings.homingDelay;
                hm.homingSpeed = settings.mvSettings.homingSpeed;
                hm.homingDuration = settings.mvSettings.homingDuration;
                return bullet;
            case MovementTypes.PropelledHomingMovement:
                PropelledHomingMovement phm = bullet.GetComponent<PropelledHomingMovement>();
                phm.isActive = true;
                phm.speed = settings.mvSettings.speed;
                phm.direction = direction;
                phm.target = AuxiliaryMethods.FindTarget(targetType.ToString(), position);
                phm.homingDelay = settings.mvSettings.homingDelay;
                phm.homingSpeed = settings.mvSettings.homingSpeed;
                phm.homingDuration = settings.mvSettings.homingDuration;
                phm.force = settings.mvSettings.force;
                phm.initialForce = settings.mvSettings.initialForce;
                return bullet;
            case MovementTypes.WavyMovement:
                WavyMovement wm = bullet.GetComponent<WavyMovement>();
                wm.isActive = true;
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
                sp.isActive = true;
                sp.direction = direction;
                sp.speed = settings.mvSettings.speed;
                sp.spiralSpeed = settings.mvSettings.spiralSpeed;
                sp.radius = settings.mvSettings.radius;
                return bullet;
            case MovementTypes.CircularMovement:
                CircularMovement cm = bullet.GetComponent<CircularMovement>();
                cm.isActive = true;
                cm.direction = direction;
                cm.speed = settings.mvSettings.speed;
                cm.rotationSpeed = settings.mvSettings.rotationSpeed;
                return bullet;
            default:
                return null;
        }
    }
}
