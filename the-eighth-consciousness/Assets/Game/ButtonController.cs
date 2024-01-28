using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{

    private AudioSource source;

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void ButtonSound()
    {
        source.Play();
    }
}
