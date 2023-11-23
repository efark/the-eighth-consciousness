using UnityEngine;

/*---------------------------------------------------------------------------------------
This code was adapted from the example for Extension methods in Unity Learn resources.
Spanish version: https://learn.unity.com/tutorial/metodos-de-extension# 
---------------------------------------------------------------------------------------*/
public static class ExtensionMethods
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
}
