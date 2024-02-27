using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.ToLower() == "enemy" ||
            other.gameObject.tag.ToLower() == "enemybullet" ||
            other.gameObject.tag.ToLower() == "playerbullet")
        {
            Destroy(other.gameObject);
            return;
        }
    }
}
