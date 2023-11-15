using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TargetTypes
{
    Enemy,
    Player
}

public enum ObjectTypes
{ 
    Enemy,
    Bullet
}

public class ObjectSettings: ScriptableObject
{
    public GameObject prefab;
    public MovementSettings movementSettings;
    public ObjectTypes objectType;
    public TargetTypes targetType;
    public int playerId;
}
