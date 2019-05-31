using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Photon.Voice.Unity.UtilityScripts;

public class RoomNetwork : NetworkManager
{
    public GameObject[] Character;
    private int playerCount = 0;

    // Start is called before the first frame update
    //가구 프리펩을 스폰가능한 프리펩 지정
    private void Awake()
    {
        //스폰 프리팹 추가
        object[] t0 = Resources.LoadAll("Prefabs");
        for (int i = 0; i < t0.Length; i++)
        {
            GameObject t1 = (GameObject)(t0[i]);
            spawnPrefabs.Add(t1);

        }
        for (int i=0; i< Character.Length; i++)
        {
            spawnPrefabs.Add(Character[i]);
        }
            
    }
    void Start()
    {
        Debug.Log("Network Manger");
        

        //로그인 서버와 연결이 되지 않은 경우
        if (Client.instance == null)
        {
            //OffLine
            Debug.Log("Offline Mode");
            //Host Start 시작, 캐릭터 프리펩 생성위해서            
            Debug.Log(networkAddress);
            StartHost();
            
            
        }
        //연결이 된 경우
        else
        {
            //방장인 경우
            if (Client.instance.isOwner)
            {
                //Host Start
                
                networkAddress = Client.instance.roomIp;
                Debug.Log(networkAddress);
                StartHost();
                Debug.Log("Start Host Success");

            }
            //방장이 아닌 경우
            else
            {
                //받은 IP 출력
                Debug.Log(Client.instance.roomIp);
                //Client instance roomIp를 네트워크 설정에 저장
                networkAddress = Client.instance.roomIp;

                //받은 IP에 맞게 Client Start
                StartClient();
                if (!NetworkClient.active)
                {
                    Debug.Log("Client Start");
                }
            }

            //음성 채팅 연결
            //방 이름을 IP로 받아서 음성 채팅 방에 연결한다.
            GetComponent<ConnectAndJoin>().RoomName = Client.instance.UserID;
            GetComponent<ConnectAndJoin>().ConnectNow();
            
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void JoinOrCreateRoom(string roomname)
    {
        roomname = roomname.Trim();
        ConnectAndJoin caj = this.GetComponent<ConnectAndJoin>();
        if (caj == null) return;

        //Debug.LogFormat("JoinOrCreateRoom() roomname: {0}", roomname);

        if (string.IsNullOrEmpty(roomname))
        {
            caj.RoomName = string.Empty;
            caj.RandomRoom = true;
            //caj.HideRoom = false;
        }
        else
        {
            caj.RoomName = roomname;
            caj.RandomRoom = false;
            //caj.HideRoom = true;
        }
        this.GetComponent<Photon.Voice.Unity.VoiceConnection>().Client.OpLeaveRoom(false);
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)
    {
        //캐릭터 타입 전달 위해 사용하는 메시지
        SpawnMessage message = new SpawnMessage();
        //Read를 위해 Deserialize
        message.Deserialize(extraMessageReader);

        //message 에서 실제 VrPlayer 인지 받음
        int character = message.CharacterType;

        //playerCount에 맞는 스폰포인트 지정
        Transform spawnPoint = this.startPositions[playerCount];

        //새로운 플레이어 생성
        GameObject newPlayer;
        //VrPlayer인 경우와 아닌 경우

        newPlayer = (GameObject)Instantiate(Character[character], spawnPoint.position, spawnPoint.rotation);


        //새로 스폰된 플레이어를 모든 클라이언트에 알림
        NetworkServer.AddPlayerForConnection(conn, newPlayer, playerControllerId);

        //플레이어 카운트 추가
        playerCount++;
    }

    //서버에서 플레이어 지울때
    public override void OnServerRemovePlayer(NetworkConnection conn, UnityEngine.Networking.PlayerController player)
    {
        //플레이어 지움
        base.OnServerRemovePlayer(conn, player);
        playerCount--;
    }

    //클라이언트 측에서 연결할 때
    public override void OnClientConnect(NetworkConnection conn)
    {
        //메시지 생성
        SpawnMessage extraMessage = new SpawnMessage();
        //VrPlayer인지 아닌지 메시지 전달
        if(Client.instance != null)
            extraMessage.CharacterType = Client.instance.charType;
        else
            extraMessage.CharacterType = 0;
        //메시지 전달
        ClientScene.AddPlayer(client.connection, 0, extraMessage);
    }
    //public override void OnServerConnect(NetworkConnection conn)
    //{
    //    //base.OnServerConnect(conn);
    //    //GameObject charObj = Instantiate(spawnPrefabs[0]);
    //    //NetworkServer.Spawn(charObj);
    //    Debug.Log("A client connected to the server: " + conn);

    //}

    public override void OnServerDisconnect(NetworkConnection conn)
    {

        NetworkServer.DestroyPlayersForConnection(conn);

        if (conn.lastError != NetworkError.Ok)
        {

            if (LogFilter.logError) { Debug.LogError("ServerDisconnected due to error: " + conn.lastError); }

        }

        Debug.Log("A client disconnected from the server: " + conn);

    }

    //public override void OnServerReady(NetworkConnection conn)
    //{

    //    NetworkServer.SetClientReady(conn);

    //    Debug.Log("Client is set to the ready state (ready to receive state updates): " + conn);

    //}


    //public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    //{


    //    var player = (GameObject)GameObject.Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
    //    player.name = playerPrefab.name;
    //    NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);

    //    Debug.Log("Client has requested to get his player added to the game");


    //}

    //public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController player)
    //{

    //    if (player.gameObject != null)

    //        NetworkServer.Destroy(player.gameObject);

    //}

    //public override void OnServerError(NetworkConnection conn, int errorCode)
    //{

    //    Debug.Log("Server network error occurred: " + (NetworkError)errorCode);

    //}

    //public override void OnStartHost()
    //{

    //    Debug.Log("Host has started");

    //}

    //public override void OnStartServer()
    //{

    //    Debug.Log("Server has started");

    //}

    //public override void OnStopServer()
    //{

    //    Debug.Log("Server has stopped");

    //}

    //public override void OnStopHost()
    //{

    //    Debug.Log("Host has stopped");

    //}

    //// Client callbacks

    //public override void OnClientConnect(NetworkConnection conn)

    //{

    //    base.OnClientConnect(conn);

    //    Debug.Log("Connected successfully to server, now to set up other stuff for the client...");

    //}

    //public override void OnClientDisconnect(NetworkConnection conn)
    //{

    //    StopClient();

    //    if (conn.lastError != NetworkError.Ok)

    //    {

    //        if (LogFilter.logError) { Debug.LogError("ClientDisconnected due to error: " + conn.lastError); }

    //    }

    //    Debug.Log("Client disconnected from server: " + conn);

    //}

    //public override void OnClientError(NetworkConnection conn, int errorCode)
    //{

    //    Debug.Log("Client network error occurred: " + (NetworkError)errorCode);

    //}

    //public override void OnClientNotReady(NetworkConnection conn)
    //{

    //    Debug.Log("Server has set client to be not-ready (stop getting state updates)");

    //}

    //public override void OnStartClient(NetworkClient client)
    //{

    //    Debug.Log("Client has started");

    //}

    //public override void OnStopClient()
    //{

    //    Debug.Log("Client has stopped");

    //}

    //public override void OnClientSceneChanged(NetworkConnection conn)
    //{

    //    base.OnClientSceneChanged(conn);

    //    Debug.Log("Server triggered scene change and we've done the same, do any extra work here for the client...");

    //}



}
