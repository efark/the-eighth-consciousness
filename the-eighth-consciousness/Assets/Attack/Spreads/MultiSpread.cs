using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiSpread : AbstractSpread
{
    private ObjectFactory factory;

    private int groupSize;
    private int internalSpacing;
    private Vector3 lateralDirection;
    private bool isCentered;

    public MultiSpread(ObjectFactory _factory, int _groupSize, int _internalSpacing, bool _isCentered)
    {
        this.factory = _factory;
        this.groupSize = _groupSize;
        this.internalSpacing = _internalSpacing;
        this.isCentered = _isCentered;
    }

    public override void Create(Vector3 startPosition, Quaternion rotation, Vector2 direction)
    {
        float spacing = 0;
        if (isCentered)
        {
            spacing = -(internalSpacing * Mathf.Floor(groupSize / 2));
        }
        lateralDirection = new Vector3(direction.y, -direction.x, 0);

        for (int i = 0; i < groupSize; i++)
        {
            Debug.Log($"lateralDirection: {lateralDirection}");
            Vector3 itemPosition = startPosition + lateralDirection.normalized * spacing;
            itemPosition = new Vector3(itemPosition.x, itemPosition.y, 0);
            // Debug.Log($"bulletPosition: {bulletPosition}");
            spacing += internalSpacing;
            factory.Create(itemPosition, rotation, direction);
        }

    }

}
