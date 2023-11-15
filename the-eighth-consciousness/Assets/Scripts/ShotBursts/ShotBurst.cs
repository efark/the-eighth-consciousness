using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
The ShotBurst class controls the shots as they are fired in a quick sequence. 
*/
public class ShotBurst: AbstractShotBurst
{
    private float offset;
    private int size;
    private float fireRate;
    private AbstractSpread shotSpread;

    public ShotBurst(float _offset, int _size, float _fireRate, AbstractSpread _shotSpread)
    { 
        offset = _offset;
        size = _size;
        fireRate = _fireRate;
        shotSpread = _shotSpread;
    }

    public override IEnumerator Fire(Vector3 startPosition, Quaternion rotation, Vector2 direction)
    {
        // Wait for offset
        yield return new WaitForSeconds(offset);
        for (int i = 0; i < size; i++)
        {
            if (i > 0)
            {
                yield return new WaitForSeconds(fireRate);
            }
            shotSpread.Create(startPosition, rotation, direction);
        }

    }

}
