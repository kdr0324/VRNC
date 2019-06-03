using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCheck : MonoBehaviour
{
    public AudioSource audio;

    // Start is called before the first frame update

    private void Awake()
    {
        audio = GameObject.Find("Room").GetComponent<AudioSource>();

    }

    private void OnEnable()
    {
        if (audio.mute)
        {
            GetComponent<VRUICheckbox>().checkmark.canvasRenderer.SetAlpha(0f);
        }
        //소리 OFF
        else
        {
            GetComponent<VRUICheckbox>().checkmark.canvasRenderer.SetAlpha(1f);
        }
    }




    // Update is called once per frame
    public void mute()
    {
        //소리 ON
        if(audio.mute)
        {
            audio.mute = false;
            GetComponent<VRUICheckbox>().isOn = true;

        }
        //소리 OFF
        else
        {
            GetComponent<VRUICheckbox>().isOn = false;
            audio.mute = true;
        }
    }
}
