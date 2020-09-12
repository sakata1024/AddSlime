using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    AudioSource audioSource;
    public SoundData SEData;
    public SoundData BGMData;

    static SoundPlayer _instance;
    public static SoundPlayer Instance
    {
        get
        {
            if (_instance == null) Debug.LogAssertion("soundPlayer instance is null");
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            _instance = this;
            audioSource = GetComponent<AudioSource>();
        }
    }

    public void PlayBGM(string soundName)
    {
        AudioClipData clipData = BGMData.Find(soundName);
        audioSource.volume = clipData.volume;
        audioSource.clip = clipData.clip;
        audioSource.Play();
    }

    public void StopBGM()
    {
        audioSource.Stop();
    }

    public void PlaySE(string soundName)
    {
        AudioClipData clipData = SEData.Find(soundName);
        audioSource.PlayOneShot(clipData.clip, clipData.volume);
    }

    
}
