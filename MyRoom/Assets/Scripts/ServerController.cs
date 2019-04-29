using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ServerController : NetworkBehaviour
{
    public bool hostStarted = false;

    public NetworkManager netManager;
    private const short chatMessage = 131;

    // Use this for initialization
    void Start()
    {

        if (!netManager)
        {
            //Grab the NetworkManager componenet
            netManager = GetComponent<NetworkManager>();
        }
    }

    public void RunHost()
    {
        if (!hostStarted)
        {
            netManager.maxConnections = 4;
            netManager.networkPort = 7777;
            netManager.StartHost();

            if (NetworkServer.active)
            {
                Debug.Log("NetworkServer is active!");
            }

            NetworkServer.RegisterHandler(chatMessage, ServerReceiveMessage);
            NetworkServer.RegisterHandler(MsgType.Connect, ConnectRequest);
            NetworkServer.RegisterHandler(MsgType.Disconnect, DisconnectRequest);

        }
        else
        {
            netManager.StopHost();
            hostStarted = false;
        }
    }

    private void ServerReceiveMessage(NetworkMessage netMsg)
    {
        NetMessage tMsg = new NetMessage();
        var tempMsg = netMsg.ReadMessage<NetMessage>();
        tMsg.name = tempMsg.name;
        tMsg.id = tempMsg.id;
        tMsg.message = tempMsg.message;

        //Sending to all connected clients
        NetworkServer.SendToAll(chatMessage, tMsg);
    }

    //Server Function
    private void ConnectRequest(NetworkMessage netMsg)
    {
        Debug.Log("User Connected from IP: " + netMsg.conn.address + ", ID: " + netMsg.conn.connectionId);
    }

    private void DisconnectRequest(NetworkMessage netMsg)
    {
        Debug.Log("Client Disconnected from IP: " + netMsg.conn.address + ", ID: " + netMsg.conn.connectionId);
    }

    public class NetMessage : MessageBase
    {
        public string name;
        public int id;
        public string message;
    }
}
