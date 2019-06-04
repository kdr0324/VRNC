using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource audio;

    public AudioClip paperSound;



    void Start()
    {
        audio = gameObject.GetComponent<AudioSource>();
    }

    void Mute()
    {
        if (audio.mute)
            audio.mute = true;
        else
            audio.mute = false;
    }
    void SetSound(AudioClip audioClip)
    {
        audio.clip = audioClip;
    }


}