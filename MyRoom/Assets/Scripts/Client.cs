using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Client : MonoBehaviour
{
    Socket cli;

    //singletone instance
    public static Client instance;

    public bool isOwner = false;

    //통신 버퍼 사이즈
    private const int s_mtu = 1400;

    //쓰레드
    private Thread sendThread = null;
    private Thread recvThread = null;
    //락
    public object sendLockObject = new object();
    public object recvLockObject = new object();

    public Queue<byte[]> sendTask = new Queue<byte[]>();
    public Queue<byte[]> recvTask = new Queue<byte[]>();


    public string roomIp = null;

    //Task
    enum Task
    {
        NOLOGIN = 0,
        LOGIN = 1,
        CHARACTERSELECT,
        ROOMMAKE,
        ROOMLIST,
        ROOMENTER,
        PLAY
    }

    
    public void Awake()
    {
        Client.instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        cli = GetComponent<LibraryClient>().Connect();
        //thread = new Thread(LoginTask);
        //thread.Start();
    }

    

    public bool Login(string id, string pwd)
    {
        //로그인 정보 전송
        byte[] buffer = new byte[s_mtu];

        //서버에게 로그인 루틴 실행하라고 알림
        buffer[0] = (byte)Task.LOGIN;
        cli.Send(buffer, buffer.Length, SocketFlags.None);
        Array.Clear(buffer, 0, 1);

        //로그인 정보 날림
        //id
        System.Text.Encoding.UTF8.GetBytes(id).CopyTo(buffer, 0);
        cli.Send(buffer, buffer.Length, SocketFlags.None);
        Array.Clear(buffer, 0, buffer.Length);
    

        //password
        System.Text.Encoding.UTF8.GetBytes(pwd).CopyTo(buffer, 0);
        cli.Send(buffer, buffer.Length, SocketFlags.None);
        Array.Clear(buffer, 0, buffer.Length);



        //로그인 성공 여부 받음
        cli.Receive(buffer, buffer.Length, SocketFlags.None);
        if (buffer[0] == 1)
        {
            //성공
            pwd = "Success";
            return true;
        }
        else if(buffer[0] == 0)
        {
            //실패
            pwd = "Fail";
            GameObject.Find("InputFieldID").GetComponent<InputField>().text = "";
            GameObject.Find("InputFieldPWD").GetComponent<InputField>().text = "";
            return false;
        }
        return false;
    }

    public void CharacterSelect(int idx)
    {
        Debug.Log("Character Select");

        byte[] buffer = new byte[s_mtu];

        //서버에게 캐릭터 선택한다고 알림
        buffer[0] = (byte)Task.CHARACTERSELECT;
        cli.Send(buffer, buffer.Length, SocketFlags.None);

        //선택한 캐릭터 정보 보냄
        buffer[0] = (byte)idx;
        cli.Send(buffer, buffer.Length, SocketFlags.None);
    }


    public void NoLogin()
    {
        Debug.Log("NoLogin");
        byte[] buffer = new byte[s_mtu];

        //서버에게 노로그인 루틴 실행하라고 알림
        buffer[0] = (byte)Task.NOLOGIN;
        cli.Send(buffer, buffer.Length, SocketFlags.None);


        /*
         * 
         * 게임 진입
         * 
         * 
         */
    }

    public void RoomMake()
    {
        Debug.Log("Room Make");
        byte[] buffer = new byte[s_mtu];

        //서버에게 방만들기 루틴 실행하라고 알림
        buffer[0] = (byte)Task.ROOMMAKE;
        cli.Send(buffer, buffer.Length, SocketFlags.None);

        //Play 시작///////////////////////////////////////
        isOwner = true;
        //Play();
        
    }

    public string[] RoomList()
    {
        Debug.Log("Room List");
        int n;
        byte[] buffer = new byte[s_mtu];
        string[] rooms;

        //서버에게 방 리스트 루틴 실행하라고 알림
        buffer[0] = (byte)Task.ROOMLIST;
        cli.Send(buffer, buffer.Length, SocketFlags.None);

        //방 몇개 있는지 받음
        cli.Receive(buffer, buffer.Length, SocketFlags.None);
        n = buffer[0];
        rooms = new string[n];

        //방 개수 만큼 방 정보 받음
        for(int i = 0; i < n; i++)
        {
            cli.Receive(buffer, buffer.Length, SocketFlags.None);
            string str = System.Text.Encoding.UTF8.GetString(buffer);
            rooms[i] = str;
            Debug.Log(str);


            Array.Clear(buffer, 0, buffer.Length);
        }

        return rooms;
    }

    public void RoomEnter(int idx)
    {
        Debug.Log("Room Enter");
        Debug.Log(idx);
        byte[] buffer = new byte[s_mtu];

        //서버에게 방 리스트 루틴 실행하라고 알림
        buffer[0] = (byte)Task.ROOMENTER;
        cli.Send(buffer, buffer.Length, SocketFlags.None);

        //방 번호 선택 // 일단 0번 지정, 리스트에 나열하는 순서대로 방 번호
        buffer[0] = (byte)idx;
        //들어가고 방 번호 전달
        cli.Send(buffer, buffer.Length, SocketFlags.None);

        /*
         * 
         * 게임 진입
         * 
         * 
         */
        //Owner IP receive
        cli.Receive(buffer, buffer.Length, SocketFlags.None);

        roomIp = System.Text.Encoding.UTF8.GetString(buffer);
        Debug.Log(roomIp);

        isOwner = false;


        //Play();
    }


    public void Play()
    {

        Debug.Log("Play");
        byte[] buffer = new byte[s_mtu];

        //서버에게 노로그인 루틴 실행하라고 알림
        buffer[0] = (byte)Task.PLAY;
        cli.Send(buffer, buffer.Length, SocketFlags.None);

        sendThread = new Thread(SendMessage);
        recvThread = new Thread(RecvMessage);
        sendThread.Start();
        recvThread.Start();
    }

    public void SendMessage()
    {
        while(true)
        {
            if(sendTask.Count > 0)
            {
                lock (sendLockObject)
                {
                    byte[] pkt = sendTask.Dequeue();
                    
                    cli.Send(pkt);
                }
            }
        }
    }

    public void RecvMessage()
    {
        
        while (true)
        {
            byte[] buffer = new byte[s_mtu];
            cli.Receive(buffer);

            lock (recvLockObject)
            {
                recvTask.Enqueue(buffer);
            }

        }
    }
}
