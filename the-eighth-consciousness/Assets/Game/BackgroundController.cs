using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    public float speed;
    [SerializeField]
    private Renderer rndr;

    void Update()
    {
        rndr.material.mainTextureOffset += new Vector2(0, speed * Time.deltaTime);
    }
}
