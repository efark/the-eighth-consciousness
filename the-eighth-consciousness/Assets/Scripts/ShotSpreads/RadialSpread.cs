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

    public override void Fire(Vector3 startPosition, Quaternion rotation)
    {
        /*---------------------------------------------------------------------------------------
        The following code was adapted from ivuecode's RadialBulletSpread repository in Github:
        https://github.com/ivuecode/RadialBulletSpread/blob/master/Assets/RadialBulletController.cs
        ---------------------------------------------------------------------------------------*/

        float angleStep = spreadAngle / roundSize;
        float angle = -(angleStep * Mathf.Floor(roundSize / 2));
        Vector3 eulerAngles = rotation.eulerAngles;

        for (int i = 0; i < roundSize; i++)
        {
            // Direction calculations.
            float projectileDirXPosition = startPosition.x + Mathf.Sin(((eulerAngles.y + angle) * Mathf.PI) / 180) * radius;
            float projectileDirYPosition = startPosition.y + Mathf.Cos(((eulerAngles.y + angle) * Mathf.PI) / 180) * radius;

            // Create vectors.
            Vector3 projectileVector = new Vector3(projectileDirXPosition, projectileDirYPosition, 0);
            Vector3 projectileMoveDirection = (projectileVector - startPosition).normalized;
            Vector2 projectileFinalDirection = new Vector2(projectileMoveDirection.x, projectileMoveDirection.y);

            /*
             // Default parameters:
        GameObject original, Vector3 position, Quaternion rotation,
        // Inherited parameters:
        string targetType, int player, float speed, int damage, float ttl, Vector2 direction             
             */

            /*ExtensionMethods.Instantiate(
                bulletSettings.prefab, startPosition, Quaternion.identity,
                targetType, playerId, bulletSettings.speed, bulletSettings.damage, bulletSettings.ttl,
                projectileFinalDirection
                );
            */
            Quaternion newRotation = Quaternion.Euler(Vector3.forward * -angle);
            ExtensionMethods.Instantiate(
                bulletSettings, targetType, playerId, startPosition, newRotation, projectileFinalDirection);

            angle += angleStep;
        }
        /*---------------------------------------------------------------------------------------
        End of quoted code.
        ---------------------------------------------------------------------------------------*/
    }

}
