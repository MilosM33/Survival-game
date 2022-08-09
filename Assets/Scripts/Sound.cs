using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string id;
    public AudioClip audio;
    public AudioSource audioSource;
    public bool loop;
    [Range(0,1)]
    public float volume;
}
