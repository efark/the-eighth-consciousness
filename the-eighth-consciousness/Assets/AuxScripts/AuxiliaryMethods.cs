using UnityEngine;

/*---------------------------------------------------------------------------------------
This code was adapted from the example for Extension methods in Unity Learn resources.
Spanish version: https://learn.unity.com/tutorial/metodos-de-extension# 
---------------------------------------------------------------------------------------*/
public static class AuxiliaryMethods
{
    public static AbstractSpread InitSpread(
        ObjectFactory factory,
        SpreadSettings settings
        )
    {
        AbstractSpread spreadShot;
        switch (settings.type)
        {
            case SpreadTypes.RadialSpread:
                spreadShot = new RadialSpread(factory, settings.groupSize, settings.spreadAngle);
                return spreadShot;
            case SpreadTypes.MultiSpread:
                spreadShot = new MultiSpread(factory, settings.groupSize, settings.internalSpacing, settings.isCentered);
                return spreadShot;
            default:
                return null;
        }
    }

    public static GameObject FindTarget(string targetType, Vector3 position)
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(targetType);
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
}
