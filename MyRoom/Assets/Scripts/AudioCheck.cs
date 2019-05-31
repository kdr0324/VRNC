using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCheck : MonoBehaviour
{
    public AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        audio = GameObject.Find("Room").GetComponent<AudioSource>();

    }

    // Update is called once per frame
    public void mute()
    {
        if(audio.mute)
        {
            audio.mute = false;

        }
        else{
            audio.mute = true;
        }
    }
}
