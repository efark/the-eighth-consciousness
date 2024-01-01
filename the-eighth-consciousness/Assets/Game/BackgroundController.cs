using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    public float speed;
    [SerializeField]
    private Renderer renderer;

    void Update()
    {
        renderer.material.mainTextureOffset += new Vector2(0, speed * Time.deltaTime);
    }
}
