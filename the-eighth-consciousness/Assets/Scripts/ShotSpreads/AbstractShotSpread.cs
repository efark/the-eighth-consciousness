using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShotSpreadTypes
{
    RadialSpread,
    MultiShotSpread
}

public abstract class AbstractShotSpread
{
    public abstract void Fire(Vector3 startPosition, Quaternion rotation, Vector2 direction, AdditionalBulletSettings additionals);
}
