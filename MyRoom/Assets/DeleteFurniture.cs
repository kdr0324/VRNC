using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using HTC.UnityPlugin.Vive;

public class DeleteFurniture : MonoBehaviour
{
    public ViveRoleProperty viveRole;
    public ControllerButton RightTrigger;
    public AudioClip deleteSound;

    Vector3 pos;
    Quaternion rot;

    // Start is called before the first frame update
    void Start()
    {
        RightTrigger = ControllerButton.Trigger;
        pos = transform.localPosition;
        rot = transform.localRotation;
    }


    private void OnCollisionEnter(Collision collision)
    {
        
        
        if(collision.transform.parent.name == "Furniture" )
        {
            //Destroy(collision.gameObject);
            transform.root.GetComponent<isLocalPlayer>().CmdDeleteFurniture(collision.gameObject);
            AudioSource audio = GameObject.Find("Room").GetComponent<AudioSource>();
            audio.clip = deleteSound;
            audio.Play();


        }
    }

    public void SetOriginPos()
    {
        
        transform.localPosition = pos;
        transform.localRotation = rot;
    }
}
