using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpreadTypes
{
    RadialSpread,
    MultiSpread
}

public enum FirepointTypes
{ 
    All,
    Central,
    Forward,
    Lateral,
    Main
}

public abstract class AbstractSpread
{
    public abstract void Create(Vector3 startPosition, Quaternion rotation, Vector2 direction);
}
