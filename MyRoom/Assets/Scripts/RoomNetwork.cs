using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RoomNetwork : NetworkManager
{
    // Start is called before the first frame update
    private void Awake()
    {
        //스폰 프리팹 추가
        string FolderName = "Assets\\Resources\\Prefabs";
        System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(FolderName);
        foreach (System.IO.FileInfo File in di.GetFiles())
        {
            if (File.Extension.ToLower().CompareTo(".prefab") == 0)
            {
                string FileNameOnly = File.Name.Substring(0, File.Name.Length - 7);
                string FullFileName = File.FullName;
                
                GameObject prefab = Resources.Load<GameObject>("Prefabs/" + FileNameOnly);
                // spawnPrefabs 리스트에 스폰할 오브젝트를 추가
                spawnPrefabs.Add(prefab);
            }
            
        }
    }
    void Start()
    {
        Debug.Log("Network Manger");
        if (Client.instance == null)
        {
            Debug.Log("Offline");
            StartHost();
            Debug.Log("Start Host Success");
            //if(StartServer())
            //{
            //    Debug.Log("Start Server Success");                
            //}
        }
        else
        {
            if (Client.instance.isOwner)
            {

                StartHost();
                Debug.Log("Start Host Success");

            }
            else
            {
                networkAddress = Client.instance.roomIp;
                StartClient();
                if (!NetworkClient.active)
                {
                    Debug.Log("Client Start");
                }
                
                
            }
        }

        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        //base.OnServerConnect(conn);
        //GameObject charObj = Instantiate(spawnPrefabs[0]);
        //NetworkServer.Spawn(charObj);
        Debug.Log("A client connected to the server: " + conn);
        

    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {

        NetworkServer.DestroyPlayersForConnection(conn);

        if (conn.lastError != NetworkError.Ok)
        {

            if (LogFilter.logError) { Debug.LogError("ServerDisconnected due to error: " + conn.lastError); }

        }

        Debug.Log("A client disconnected from the server: " + conn);

    }

    public override void OnServerReady(NetworkConnection conn)
    {

        NetworkServer.SetClientReady(conn);

        Debug.Log("Client is set to the ready state (ready to receive state updates): " + conn);

    }


    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {

        
        var player = (GameObject)GameObject.Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        player.name = playerPrefab.name;
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);

        Debug.Log("Client has requested to get his player added to the game");


    }

    public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController player)
    {

        if (player.gameObject != null)

            NetworkServer.Destroy(player.gameObject);

    }

    public override void OnServerError(NetworkConnection conn, int errorCode)
    {

        Debug.Log("Server network error occurred: " + (NetworkError)errorCode);

    }

    public override void OnStartHost()
    {

        Debug.Log("Host has started");

    }

    public override void OnStartServer()
    {

        Debug.Log("Server has started");

    }

    public override void OnStopServer()
    {

        Debug.Log("Server has stopped");

    }

    public override void OnStopHost()
    {

        Debug.Log("Host has stopped");

    }

    // Client callbacks

    public override void OnClientConnect(NetworkConnection conn)

    {

        base.OnClientConnect(conn);

        Debug.Log("Connected successfully to server, now to set up other stuff for the client...");

    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {

        StopClient();

        if (conn.lastError != NetworkError.Ok)

        {

            if (LogFilter.logError) { Debug.LogError("ClientDisconnected due to error: " + conn.lastError); }

        }

        Debug.Log("Client disconnected from server: " + conn);

    }

    public override void OnClientError(NetworkConnection conn, int errorCode)
    {

        Debug.Log("Client network error occurred: " + (NetworkError)errorCode);

    }

    public override void OnClientNotReady(NetworkConnection conn)
    {

        Debug.Log("Server has set client to be not-ready (stop getting state updates)");

    }

    public override void OnStartClient(NetworkClient client)
    {

        Debug.Log("Client has started");

    }

    public override void OnStopClient()
    {

        Debug.Log("Client has stopped");

    }

    public override void OnClientSceneChanged(NetworkConnection conn)
    {

        base.OnClientSceneChanged(conn);

        Debug.Log("Server triggered scene change and we've done the same, do any extra work here for the client...");

    }

    

}
