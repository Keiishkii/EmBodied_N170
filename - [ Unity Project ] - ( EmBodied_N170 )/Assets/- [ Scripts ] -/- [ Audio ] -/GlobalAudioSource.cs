using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.IO;
using UnityEngine;

public class GlobalAudioSource : MonoBehaviour
{
    private static GlobalAudioSource _instance;
    public static GlobalAudioSource Instance => _instance ?? (_instance = FindObjectOfType<GlobalAudioSource>());

    public SfxContainer SfxContainer;
    
    private AudioSource _audioSource;


    
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    

    public void PlayOneShot(AudioClip audioClip)
    {
        _audioSource.volume = 1.0f;
        _audioSource.PlayOneShot(audioClip);
    }
    
    public void PlayOneShot(AudioClip audioClip, float volume)
    {
        _audioSource.volume = volume;
        _audioSource.PlayOneShot(audioClip);
    }
}
