using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ClientController : MonoBehaviour
{
    public string hostIP;
    public int hostPort;
    bool clientStarted;

    NetworkClient netClient;
    public NetworkManager netManager;
    private const short chatMessage = 131;

    // Use this for initialization
    void Start()
    {
        if (!netManager)
        {
            //Grab the NetworkManager componenet
            netManager = GetComponent<NetworkManager>();
            Debug.Log("Getting NetManager...");
        }

        Application.runInBackground = true;
        DontDestroyOnLoad(transform.gameObject);

        Debug.Log("Finished start procedures...");
    }

    public void RunClient()
    {
        if (!clientStarted)
        {
            //Start Client and connect to server port
            clientStarted = true;
            netClient = new NetworkClient();

            netManager.networkAddress = hostIP;
            netManager.networkPort = 7777;
            netClient = netManager.StartClient();

            netClient.RegisterHandler(chatMessage, ReceiveMessage);
            netClient.RegisterHandler(MsgType.Connect, OnConnected);
            netClient.RegisterHandler(MsgType.Disconnect, DisconnectRequest);

            Debug.Log("Finished starting client... to + " + hostIP);
        }
    }

    public void ReceiveMessage(NetworkMessage netMsg)
    {
        //reading message
        NetMessage tMsg = new NetMessage();
        var tempMsg = netMsg.ReadMessage<NetMessage>();
        tMsg.name = tempMsg.name;
        tMsg.id = tempMsg.id;
        tMsg.message = tempMsg.message;

        Debug.Log(tMsg.name + ": " + tMsg.message);
    }

    //Client Function
    private void OnConnected(NetworkMessage netMsg)
    {
        Debug.Log("Connected to server at IP: " + hostIP + ", Port: " + hostPort);
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

    //void Update()
    //{
    //    //Start client
    //    if (!clientStarted)
    //    {
    //        RunClient();
    //    }
    //}
}
