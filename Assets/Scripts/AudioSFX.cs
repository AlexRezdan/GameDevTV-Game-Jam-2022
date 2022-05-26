using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioSounds
{
    public AudioSounds audioSounds;
    public AudioClip[] audioSoundVariations;
}

public class AudioSFX : MonoBehaviour
{
    public AudioSounds[] audioSoundEffects;

    // Singleton Instantion
    private static AudioSFX instance;
    public static AudioSFX Instance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType<AudioSFX>();
            return instance;
        }
    }
}
