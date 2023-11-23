using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractEnemyController : MonoBehaviour
{
    protected int hp;
    public abstract int HP
    {
        get;
        set;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
