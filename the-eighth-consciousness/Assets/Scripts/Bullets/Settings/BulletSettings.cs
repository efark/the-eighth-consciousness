using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BulletSettings : ScriptableObject
{
    public MovementSettings mvSettings;
    public GameObject prefab;
    public int damage;
    public float ttl;

    private int currentDamage;
    public int CurrentDamage => currentDamage;

    public void UpdateDamage(int newValue)
    {
        currentDamage = newValue;
    }
}