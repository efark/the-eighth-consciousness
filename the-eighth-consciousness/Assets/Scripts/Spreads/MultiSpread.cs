using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiSpread : AbstractSpread
{
    private ObjectFactory factory;

    private int groupSize;
    private int internalSpacing;
    private bool isAlternating;
    private bool alternate;
    private Vector3 lateralDirection;

    public MultiSpread(ObjectFactory _factory, int _groupSize, int _internalSpacing, bool _isAlternating)
    {
        this.factory= _factory;
        this.groupSize = _groupSize;
        this.internalSpacing = _internalSpacing;
        this.isAlternating = _isAlternating;
        if (isAlternating)
        {
            alternate = true;
        }
    }

    public override void Create(Vector3 startPosition, Quaternion rotation, Vector2 direction)
    {
        float spacing = -(internalSpacing * Mathf.Floor(groupSize / 2));
        lateralDirection = new Vector3(direction.y, -direction.x, 0);

        for (int i = 0; i < groupSize; i++)
        {
            Vector3 itemPosition = startPosition + lateralDirection.normalized * spacing;
            itemPosition = new Vector3(itemPosition.x, itemPosition.y, 0);
            // Debug.Log($"bulletPosition: {bulletPosition}");
            spacing += internalSpacing;
            if (isAlternating)
            {
                // additionals.alternate = alternate;
                alternate = !alternate;
            }
            factory.Create(itemPosition, rotation, direction);
        }

    }

}
