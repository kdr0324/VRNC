using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class setParent : NetworkBehaviour
{

    

    // Start is called before the first frame update
    void Start()
    {
        transform.SetParent(GameObject.Find("Furniture").transform, true);


        if (name.Contains("Clone"))
        {
            string[] split = name.Split('(');
            name = split[0];
        }
    }

    void Update()
    {
        if (transform.position.y < -15)
        {
            GameObject.Find("LocalPlayer").GetComponent<isLocalPlayer>().CmdDeleteFurniture(gameObject);
        }
    }

}
