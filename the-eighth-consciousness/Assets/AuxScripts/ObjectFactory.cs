using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ObjectFactory
{
    GameObject Create(Vector3 position, Quaternion rotation, Vector2 direction);
}
