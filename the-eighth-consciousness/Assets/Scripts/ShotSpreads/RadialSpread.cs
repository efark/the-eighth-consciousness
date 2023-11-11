using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialSpread : AbstractShotSpread
{
    private BulletSettings bulletSettings;
    //public GameObject bullet;

    //public SpreadSettings spreadSettings;
    private int roundSize;
    private float spreadAngle;
    private string targetType;
    private int playerId;

    private float radius = 1f;
    private float rotationStep;

    public RadialSpread(BulletSettings _bulletSettings, string _targetType, int _playerId,
        int _roundSize, float _spreadAngle)
    {
        bulletSettings = _bulletSettings;
        targetType = _targetType;
        playerId = _playerId;
        roundSize = _roundSize;
        spreadAngle = _spreadAngle;
    }

    public override void Fire(Vector3 startPosition, Quaternion rotation, Vector2 direction)
    {
        /*---------------------------------------------------------------------------------------
        The following code was adapted from ivuecode's RadialBulletSpread repository in Github:
        https://github.com/ivuecode/RadialBulletSpread/blob/master/Assets/RadialBulletController.cs
        ---------------------------------------------------------------------------------------*/

        float angleStep = spreadAngle / roundSize;
        float angle = -(angleStep * Mathf.Floor(roundSize / 2));
        Vector3 eulerAngles = rotation.eulerAngles;


        // Vector2 unitVector = new Vector2(0, 1);
        // float directionAngle = Vector2.SignedAngle(unitVector, direction);
        float directionAngle = CalculateAngle(direction);
        Debug.Log(directionAngle);

        for (int i = 0; i < roundSize; i++)
        {
            // Direction calculations.
            //eulerAngles.y
            float projectileDirXPosition = startPosition.x + Mathf.Sin(((directionAngle + angle) * Mathf.PI) / 180) * radius;
            float projectileDirYPosition = startPosition.y + Mathf.Cos(((directionAngle + angle) * Mathf.PI) / 180) * radius;

            // Create vectors.
            Vector3 projectileVector = new Vector3(projectileDirXPosition, projectileDirYPosition, 0);
            Vector3 projectileMoveDirection = (projectileVector - startPosition).normalized;
            Vector2 projectileFinalDirection = new Vector2(projectileMoveDirection.x, projectileMoveDirection.y);

            Quaternion newRotation = Quaternion.Euler(Vector3.back * (directionAngle + angle));
            ExtensionMethods.Instantiate(
                bulletSettings, targetType, playerId, startPosition, newRotation, projectileFinalDirection);

            angle += angleStep;
        }
        /*---------------------------------------------------------------------------------------
        End of quoted code.
        ---------------------------------------------------------------------------------------*/
    }

    /*
    https://discussions.unity.com/t/calculating-the-angle-of-a-vector2-from-zero/69663/3 
    */
    public static float CalculateAngle(Vector2 vector2)
    {
        return 360 - (Mathf.Atan2(vector2.x, vector2.y) * -1 *Mathf.Rad2Deg /** Mathf.Sign(vector2.x)*/);
    }

}
