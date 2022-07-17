using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : Singleton<SFXManager>
{
    public enum SFXType
    {
        dealDamage,
        heal,
        die,
        win,
        lose
    }

    //This list should line up with the order of the enum above, otherwise the right sound effect may not be played
    [SerializeField] private List<AudioSource> soundEffectList;

    public void PlayAudio(SFXType soundEffectType)
    {
        AudioSource sourceToPlay = soundEffectList[(int)soundEffectType];
        if(sourceToPlay.isPlaying)
            return; //no duplicate FX

        sourceToPlay.Play();
    }
}
