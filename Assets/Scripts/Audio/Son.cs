using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Son
{
    public string tag;
    public AudioClip clip;
    [HideInInspector] public AudioSource source;

    [Range(0f, 1f)] public float volume = .5f;
    [Range(-3f, 3f)] public float pitch = 1f;

    public bool loop, playOnAwake;

    
}
