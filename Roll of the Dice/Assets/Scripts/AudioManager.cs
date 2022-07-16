using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private bool playOnStart;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clipToPlay;
    
    private bool playing = false;

    // Start is called before the first frame update
    void Start()
    {
        if(playOnStart)
            StartMusic();
    }

    void StartMusic()
    {
        if (playing)
            return;

        audioSource.clip = clipToPlay;
        audioSource.Play();

        playing = true;
    }
}
