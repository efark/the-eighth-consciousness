using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialSpread : AbstractSpread
{
    private ObjectFactory factory;

    private int groupSize;
    private float spreadAngle;

    private float radius = 1f;
    private float rotationStep;

    public RadialSpread(ObjectFactory _factory, int _roundSize, float _spreadAngle)
    {
        this.factory = _factory;
        this.groupSize = _roundSize;
        this.spreadAngle = _spreadAngle;
    }

    public override void Create(Vector3 startPosition, Quaternion rotation, Vector2 direction)
    {
        /*---------------------------------------------------------------------------------------
        The following code was adapted from ivuecode's RadialBulletSpread repository in Github:
        https://github.com/ivuecode/RadialBulletSpread/blob/master/Assets/RadialBulletController.cs
        ---------------------------------------------------------------------------------------*/

        float angleStep = spreadAngle / groupSize;
        float angle = -(angleStep * Mathf.Floor(groupSize / 2));
        Vector3 eulerAngles = rotation.eulerAngles;

        float directionAngle = CalculateAngle(direction);

        for (int i = 0; i < groupSize; i++)
        {
            // Direction calculations.
            float projectileDirXPosition = startPosition.x + Mathf.Sin(((directionAngle + angle) * Mathf.PI) / 180) * radius;
            float projectileDirYPosition = startPosition.y + Mathf.Cos(((directionAngle + angle) * Mathf.PI) / 180) * radius;

            // Create vectors.
            Vector3 projectileVector = new Vector3(projectileDirXPosition, projectileDirYPosition, 0);
            Vector3 projectileMoveDirection = (projectileVector - startPosition).normalized;
            Vector2 projectileFinalDirection = new Vector2(projectileMoveDirection.x, projectileMoveDirection.y);

            // Rotate the bullet.
            Quaternion newRotation = Quaternion.Euler(Vector3.back * (directionAngle + angle));
            factory.Create(startPosition, newRotation, projectileFinalDirection);

            angle += angleStep;
        }
        /*---------------------------------------------------------------------------------------
        End of quoted code.
        ---------------------------------------------------------------------------------------*/
    }

    /*---------------------------------------------------------------------------------------
    Code adapted from a post in Unity's public forum.
    https://discussions.unity.com/t/calculating-the-angle-of-a-vector2-from-zero/69663/3 
    ---------------------------------------------------------------------------------------*/
    public static float CalculateAngle(Vector2 vector2)
    {
        return 360 - (Mathf.Atan2(vector2.x, vector2.y) * -1 * Mathf.Rad2Deg);
    }
    /*---------------------------------------------------------------------------------------
    End of quoted code.
    ---------------------------------------------------------------------------------------*/
}
