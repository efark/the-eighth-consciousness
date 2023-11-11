using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractShotBurst
{
    public abstract IEnumerator Fire(Vector3 startPosition, Quaternion rotation, Vector2 direction);
}
