using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager Instance;
    
    [SerializeField] AudioSource _audioSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("SoundFXManager already exists, destroying duplicate");
            Destroy(gameObject);
        }
    }
    
    public void PlaySoundFXClip(AudioClip audioClip, Transform transform, float volume = 1f)
    {
        GameObject soundFX = new GameObject("SoundFX");
        soundFX.transform.position = transform.position;
        AudioSource audioSource = soundFX.AddComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();
        float clipLength = audioSource.clip.length;
        Destroy(soundFX, clipLength);
    }
    public void PlaySoundFXClipWithVariation(AudioClip[] audioClip, Transform transform, float volume = 1f)
    {
        int randomIndex = UnityEngine.Random.Range(0, audioClip.Length);
        
        GameObject soundFX = new GameObject("SoundFX");
        soundFX.transform.position = transform.position;
        AudioSource audioSource = soundFX.AddComponent<AudioSource>();
        audioSource.clip = randomIndex < audioClip.Length ? audioClip[randomIndex] : audioClip[0];
        audioSource.volume = volume;
        audioSource.Play();
        float clipLength = audioSource.clip.length;
        Destroy(soundFX, clipLength);
    }
    
}
