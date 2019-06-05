using Photon.Voice.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceChattingCheck : MonoBehaviour
{
    public bool audio;

    // Start is called before the first frame update

    private void Awake()
    {
        audio = GameObject.Find("RoomNetworkManager").GetComponent<VoiceConnection>().enabled;

    }

    private void OnEnable()
    {
        //Voice Chatting ON
        if (audio)
        {
            GetComponent<VRUICheckbox>().checkmark.canvasRenderer.SetAlpha(1f);
        }
        //Voice Chatting OFF
        else
        {
            GetComponent<VRUICheckbox>().checkmark.canvasRenderer.SetAlpha(0f);
        }
    }




    // Update is called once per frame
    public void mute()
    {
        //소리 OFF
        if (audio)
        {
            audio = false;
            GameObject.Find("RoomNetworkManager").GetComponent<VoiceConnection>().enabled = false;
            GetComponent<VRUICheckbox>().isOn = false;

        }
        //소리 ON
        else
        {
            audio = true;
            GameObject.Find("RoomNetworkManager").GetComponent<VoiceConnection>().enabled = true;
            GetComponent<VRUICheckbox>().isOn = true;
            
        }
    }
}
