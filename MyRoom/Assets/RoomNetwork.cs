using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RoomNetwork : NetworkManager
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Network Manger");
        if(Client.instance.isOwner)
        {
            NetworkManager.singleton.StartHost();
        }
        else
        {
            NetworkManager.singleton.networkAddress = Client.instance.roomIp;
            NetworkManager.singleton.StartClient();
            Debug.Log("Client Start");
            //Client.instance.roomIp;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
