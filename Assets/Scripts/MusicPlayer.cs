using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[System.Serializable]
//public class AudioSounds
//{
//    public AudioSounds audioSounds;
//    public AudioClip[] audioSoundVariations;
//}

public class MusicPlayer : MonoBehaviour
{
    public AudioClip[] music;

    public int songToPlayInThisLevel;

    private AudioSource audioSource;

    // Singleton Instantion
    private static MusicPlayer instance;
    public static MusicPlayer Instance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType<MusicPlayer>();
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);

        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        audioSource.clip = music[songToPlayInThisLevel];

        if (!audioSource.isPlaying)
        {
            Debug.Log("Playing for the first time.");
            audioSource.Play();
        }
        else if (audioSource.isPlaying == music[songToPlayInThisLevel])
        {
            Debug.Log("Song is already playing!");
            return;
        }
        else
        {
            Debug.Log("Playing new song.");
            audioSource.Stop();
            audioSource.PlayOneShot(music[songToPlayInThisLevel]);
        }
    }
}
