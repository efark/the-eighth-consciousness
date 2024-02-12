using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundController : MonoBehaviour
{
    public float speed;
    public Material baseBG;
    public Material ecdBG;
    private Image background;
    private Renderer rdr;

    void Start()
    {
        PlayerController.OnTriggerECD += ChangeColor;
        background = GetComponent<Image>();
        rdr = GetComponent<Renderer>();
    }

    void OnDestroy()
    {
        PlayerController.OnTriggerECD -= ChangeColor;
    }

    void ChangeColor(bool ecdActive)
    {
        Vector2 offset = GetComponent<Renderer>().material.mainTextureOffset;
        GetComponent<Renderer>().material = ecdActive ? ecdBG : baseBG;
        GetComponent<Renderer>().material.mainTextureOffset = offset;
    }

    void Update()
    {
        GetComponent<Renderer>().material.mainTextureOffset += new Vector2(0, speed * Time.deltaTime);
    }
}
