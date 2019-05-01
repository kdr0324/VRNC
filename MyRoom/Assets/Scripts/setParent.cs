using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class setParent : NetworkBehaviour
{
    private AudioSource audio;
    public AudioClip clickSound;
    

    // Start is called before the first frame update
    void Start()
    {
        transform.SetParent(GameObject.Find("Furniture").transform, true);
 
        this.audio = this.gameObject.AddComponent<AudioSource>();
        AudioClip clip = Resources.Load("Sounds/DM-CGS-02") as AudioClip;
        audio.clip = clip;
        


        //audio.playOnAwake = true;
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}
   
}
