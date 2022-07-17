using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopingAudioManager : Singleton<AudioManager>
{
    [SerializeField] private bool playOnStart;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip nonLoopedClip;
    [SerializeField] private AudioClip loopedClip;
    
    private bool playing = false;

    // Start is called before the first frame update
    void Start()
    {
        if(playOnStart)
            StartMusic();
    }

    public void StartMusic()
    {
        if (playing)
            return;

        audioSource.clip = nonLoopedClip;
        audioSource.Play();

        playing = true;

        Invoke("PlayLoop", nonLoopedClip.length);
    }

    private void PlayLoop()
    {
        audioSource.clip = loopedClip;
        audioSource.Play();
        audioSource.loop = true;
    }
}
