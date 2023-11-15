using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractBurst: MonoBehaviour
{
    public abstract void Fire(Vector3 startPosition, Quaternion rotation, Vector2 direction);
}
