using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : ObjectFactory
{
    public GameObject prefab;

    public EnemyFactory(GameObject _prefab)
    {
        this.prefab = _prefab;
    }

    public GameObject Create(Vector3 position, Quaternion rotation, Vector2 _)
    {
        GameObject e = GameObject.Instantiate(this.prefab, position, rotation) as GameObject;
        return e;
    }
}
