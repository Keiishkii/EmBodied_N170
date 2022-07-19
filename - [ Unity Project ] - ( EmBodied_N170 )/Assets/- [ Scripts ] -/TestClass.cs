using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    [SerializeField] private AudioSource source;

    [ContextMenu("Play Sound")]
    public void PlaySound()
    {
        source.Play();
    }
}