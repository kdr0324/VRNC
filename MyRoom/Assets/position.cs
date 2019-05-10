using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using HTC.UnityPlugin.Vive;
public class position : MonoBehaviour
{

    public ViveRoleProperty viveRole;
    public ControllerButton RightTrigger;

    
    // Start is called before the first frame update
    void Start()
    {
        RightTrigger = ControllerButton.Trigger;
        
        
    }

 
    public void SetOriginPos()
    {
        if(transform.parent.name == "Furniture")
        {
           
            if(transform.position.y < -10f)
            {
                Debug.Log("position.y is low");
                transform.position = new Vector3(0, 0, 0);
            }
        }
    }
}
