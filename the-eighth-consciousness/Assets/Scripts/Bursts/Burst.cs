using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
The ShotBurst class controls the shots as they are fired in a quick sequence. 
*/
public class Burst : AbstractBurst
{
    private AbstractSpread spread;
    private float offset;
    private int size;
    private float fireRate;

    public Burst(AbstractSpread _spread, float _offset, int _size, float _fireRate)
    {
        spread = _spread;
        offset = _offset;
        size = _size;
        fireRate = _fireRate;
    }

    public override void Fire(Vector3 startPosition, Quaternion rotation, Vector2 direction)
    {
        StartCoroutine(fire(startPosition, rotation, direction));
    }

    public IEnumerator fire(Vector3 startPosition, Quaternion rotation, Vector2 direction)
    {
        // Wait for offset
        yield return new WaitForSeconds(offset);
        for (int i = 0; i < size; i++)
        {
            if (i > 0)
            {
                yield return new WaitForSeconds(fireRate);
            }
            spread.Create(startPosition, rotation, direction);
        }

    }

}
