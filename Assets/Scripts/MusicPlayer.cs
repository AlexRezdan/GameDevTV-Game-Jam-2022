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
            audioSource.Play();
        }
        //else if (audioSource.isPlaying == music[songToPlayInThisLevel])
        //{
        //    Debug.Log("Song is already playing!");
        //    return;
        //}
        //else
        //{
        //    Debug.Log("Playing new song.");
        //    audioSource.Stop();
        //    audioSource.PlayOneShot(music[songToPlayInThisLevel]);
        //}
    }

    // Comments are from me trying to figure out how to play different songs in some levels without stopping the continuous looping between consecutive levels. Couldn't figure it out, but am leaving this here for future reference.

    private void Update()
    {
        //if (music[songToPlayInThisLevel] != audioSource.clip)
        //{
        //    audioSource.Stop();
        //    audioSource.clip = music[songToPlayInThisLevel];
        //    audioSource.Play();
        //}
    }
}
