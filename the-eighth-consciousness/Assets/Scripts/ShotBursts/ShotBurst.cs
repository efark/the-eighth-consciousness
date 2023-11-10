using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotBurst: MonoBehaviour
{
    public float offset;
    public float size;
    public float fireRate;

    public void Fire()
    {
        StartCoroutine(fire());
    }

    private IEnumerator fire()
    {
        // Wait for offset
        yield return new WaitForSeconds(offset);
        for (int i = 0; i < size; i++)
        {
            if (i > 0)
            {
                yield return new WaitForSeconds(fireRate);
            }
            Debug.Log("Fire");
        }
    }
}
