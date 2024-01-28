using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundController : MonoBehaviour
{
    public float speed;
    public Material baseBG;
    public Material ecdBG;
    // private bool isGrayscale;
    private Image background;
    private Renderer rdr;

    void Start()
    {
        // isGrayscale = false;
        PlayerController.OnTriggerECD += ChangeColor;
        background = GetComponent<Image>();
        rdr = GetComponent<Renderer>();
    }

    void ChangeColor(bool ecdActive)
    {
        Vector2 offset = rdr.material.mainTextureOffset;
        rdr.material = ecdActive ? ecdBG : baseBG;
        rdr.material.mainTextureOffset = offset;
    }

    void Update()
    {
        rdr.material.mainTextureOffset += new Vector2(0, speed * Time.deltaTime);
    }
}
