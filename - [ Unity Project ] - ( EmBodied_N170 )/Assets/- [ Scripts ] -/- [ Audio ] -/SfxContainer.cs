using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SfxContainer
{
    [SerializeField] private AudioClip _goSFX;
    public AudioClip GoSfx => _goSFX;
}
