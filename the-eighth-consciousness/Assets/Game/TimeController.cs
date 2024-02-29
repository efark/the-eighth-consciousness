using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*---------------------------------------------------------------------------------------
Code taken and adapted from:
https://sharpcoderblog.com/blog/unity-3d-slow-motion-effect-script
---------------------------------------------------------------------------------------*/
public class TimeController : MonoBehaviour
{
    public float slowMoScale = 0.5f;
    public float cooldown = 2f;
    private bool isActive;
    private float countdown;
    [System.Serializable]
    public class AudioSourceData
    {
        public AudioSource audioSource;
        public float defaultPitch;
    }

    AudioSourceData[] audioSources;

    // Start is called before the first frame update
    void Start()
    {
        isActive = false;
        //Find all AudioSources in the Scene and save their default pitch values
        AudioSource[] audios = FindObjectsOfType<AudioSource>();
        audioSources = new AudioSourceData[audios.Length];

        for (int i = 0; i < audios.Length; i++)
        {
            AudioSourceData tmpData = new AudioSourceData();
            tmpData.audioSource = audios[i];
            tmpData.defaultPitch = audios[i].pitch;
            audioSources[i] = tmpData;
        }

        PlayerController.OnTriggerECD += SlowMotionEffect;
        PlayerController.CheckECD += CheckECDStatus;
    }

    void Update()
    {
        if (!isActive && countdown > 0)
        {
            countdown = Mathf.Clamp(countdown - Time.deltaTime, 0, cooldown);
        }
    }

    public bool CheckECDStatus()
    {
        return (!isActive && countdown == 0f);
    }

    public void SlowMotionEffect(bool newStatus)
    {
        isActive = newStatus;
        Time.timeScale = isActive ? slowMoScale : 1;
        countdown = isActive ? 0f : 2f;
        for (int i = 0; i < audioSources.Length; i++)
        {
            if (audioSources[i].audioSource)
            {
                audioSources[i].audioSource.pitch = audioSources[i].defaultPitch * Time.timeScale;
            }
        }
    }

    private void OnDestroy()
    {
        SlowMotionEffect(false);
    }
}
