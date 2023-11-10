using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiShotSpread : AbstractShotSpread
{
    private BulletSettings bulletSettings;

    private int roundSize;
    private int roundSpacing;
    private string targetType;
    private int playerId;
    private bool isAlternating;
    private bool alternate;
    private Vector3 lateralDirection;

    public MultiShotSpread(BulletSettings _bulletSettings, string _targetType, int _playerId,
        int _roundSize, int _roundSpacing, bool _isAlternating)
    {
        bulletSettings = _bulletSettings;
        targetType = _targetType;
        playerId = _playerId;
        roundSize = _roundSize;
        roundSpacing = _roundSpacing;
        isAlternating = _isAlternating;
        if (isAlternating)
        {
            alternate = true;
        }
    }

    public override void Fire(Vector3 startPosition, Quaternion rotation, Vector2 direction)
    {
        float spacing = -(roundSpacing * Mathf.Floor(roundSize / 2));
        lateralDirection = new Vector3(direction.y, -direction.x, 0);

        for (int i = 0; i < roundSize; i++)
        {
            Vector3 bulletPosition = startPosition + lateralDirection.normalized * spacing;
            bulletPosition = new Vector3(bulletPosition.x, bulletPosition.y, 0);
            Debug.Log($"bulletPosition: {bulletPosition}");
            spacing += roundSpacing;
            if (isAlternating)
            {
                ExtensionMethods.Instantiate(
                bulletSettings, targetType, playerId, bulletPosition, rotation, direction, alternate);
                alternate = !alternate;
                continue;
            }
            ExtensionMethods.Instantiate(
                bulletSettings, targetType, playerId, bulletPosition, rotation, direction);

        }

    }
    
}
