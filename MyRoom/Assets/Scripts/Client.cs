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
    public string roomIp = null;

    public bool isConnect = false;


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

    public int charType = 0;
    public int roomType = 0; 



    

    //Task
    enum Task
    {
        NOLOGIN = 0,
        LOGIN = 1,
        SIGNUP,
        CHARACTERSELECT,
        ROOMMAKE,
        ROOMLIST,
        ROOMENTER,
        PLAY,
        SAVE,
        LOAD,
        LABELLOAD,
        
    }

    
    public void Awake()
    {
        Client.instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //게임서버와 연결
        cli = GetComponent<LibraryClient>().Connect();

        //연결 성공 혹은 실패 체크
        if (cli == null)
            isConnect = false;
        else
        {
            isConnect = true;
        }
            



        //thread = new Thread(LoginTask);
        //thread.Start();
    }

    public bool Login(string id, string pwd)
    {
        if (isConnect)
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
                //pwd = "Success";
                return true;
            }
            else if (buffer[0] == 0)
            {
                //실패
                //pwd = "Fail";
                //GameObject.Find("InputFieldID").GetComponent<InputField>().text = "";
                //GameObject.Find("InputFieldPWD").GetComponent<InputField>().text = "";
                return false;
            }
            return false;
        }
        else
        {
            return true;
        }
    }

    public bool SignUp(string id, string pwd)
    {
        if (isConnect)
        {
            Debug.Log("isConnect");
            byte[] buffer = new byte[s_mtu];

            //서버에게 회원가입 루틴 실행하라고 알림
            buffer[0] = (byte)Task.SIGNUP;
            cli.Send(buffer, buffer.Length, SocketFlags.None);
            Array.Clear(buffer, 0, 1);

            //회원 가입 정보 날림
            //id
            System.Text.Encoding.UTF8.GetBytes(id).CopyTo(buffer, 0);
            cli.Send(buffer, buffer.Length, SocketFlags.None);
            Array.Clear(buffer, 0, buffer.Length);

            //password
            System.Text.Encoding.UTF8.GetBytes(pwd).CopyTo(buffer, 0);
            cli.Send(buffer, buffer.Length, SocketFlags.None);
            Array.Clear(buffer, 0, buffer.Length);

            //회원가입 성공 여부 받음
            cli.Receive(buffer, buffer.Length, SocketFlags.None);
            if (buffer[0] == 1)
            {
                //성공
                //pwd = "Success";
                return true;
            }
            else if (buffer[0] == 0)
            {
                //실패
                //pwd = "Fail";
                //GameObject.Find("InputFieldID").GetComponent<InputField>().text = "";
                //GameObject.Find("InputFieldPWD").GetComponent<InputField>().text = "";
                return false;
            }
            return false;
        }
        else
        {
            return true;
        }
    }

    

    public void CharacterSelect(int idx)
    {
        charType = idx;
        if (isConnect)
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
        if (isConnect)
        {
            byte[] buffer = new byte[s_mtu];

            //서버에게 방만들기 루틴 실행하라고 알림
            buffer[0] = (byte)Task.ROOMMAKE;
            cli.Send(buffer, buffer.Length, SocketFlags.None);

            //Play 시작///////////////////////////////////////
            isOwner = true;


            //Play();
        }
        else
        {
            isOwner = true;
        }
        
    }

    public string[] RoomList()
    {
        if (isConnect)
        {
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
            for (int i = 0; i < n; i++)
            {
                cli.Receive(buffer, buffer.Length, SocketFlags.None);
                string str = System.Text.Encoding.UTF8.GetString(buffer);
                rooms[i] = str;
                Debug.Log(str);


                Array.Clear(buffer, 0, buffer.Length);
            }

            return rooms;
        }
        else
        {
            return null;
        }
    }

    public void RoomEnter(int idx)
    {
        if (isConnect)
        {

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

            //GetComponent<ClientController>().hostIP = roomIp;
            //Play();
        }
        else
        {
            isOwner = false;
        }
    }


    public void Play()
    {
        if (isConnect)
        {

            byte[] buffer = new byte[s_mtu];

            //서버에게 노로그인 루틴 실행하라고 알림
            buffer[0] = (byte)Task.PLAY;
            cli.Send(buffer, buffer.Length, SocketFlags.None);

            sendThread = new Thread(SendMessage);
            recvThread = new Thread(RecvMessage);
            sendThread.Start();
            recvThread.Start();
        }
    }

    public void Save(string jsonData, int idx)
    {
        if (isConnect)
        {
            byte[] buffer = new byte[s_mtu];

            //서버에게 노로그인 루틴 실행하라고 알림
            buffer[0] = (byte)Task.SAVE;
            cli.Send(buffer, buffer.Length, SocketFlags.None);

            //[DATA] 얼만큼의 길이를 보낼건지 보냄
            byte[] intBytes = BitConverter.GetBytes(jsonData.Length);
            intBytes.CopyTo(buffer, 0);
            cli.Send(buffer, buffer.Length, SocketFlags.None);

            //실제 데이터 길이만큼 보냄
            buffer = System.Text.Encoding.UTF8.GetBytes(jsonData);
            cli.Send(buffer, buffer.Length, SocketFlags.None);

            //[index] 얼만큼의 길이를 보낼건지 보냄
            buffer = new byte[s_mtu];

            intBytes = BitConverter.GetBytes(idx);
            intBytes.CopyTo(buffer, 0);
            cli.Send(buffer, buffer.Length, SocketFlags.None);

            ////[scrshot] 얼만큼의 길이를 보낼건지 보냄
            //intBytes = BitConverter.GetBytes(scrshot.Length);
            //intBytes.CopyTo(buffer, 0);
            //cli.Send(buffer, buffer.Length, SocketFlags.None);

            ////[scrshot] 실제 데이터 길이만큼 보냄
            ////buffer = System.Text.Encoding.UTF8.GetBytes(scrshot);

            //Debug.Log(System.Text.Encoding.UTF8.GetString(scrshot));
            //cli.Send(scrshot, scrshot.Length, SocketFlags.None);
        }

    }

    
    public string[] Label_Load()
    {

        string[] result = null;
        result = new string[4];

        if (isConnect)
        {
            byte[] buffer = new byte[s_mtu];

            //서버에게 노로그인 루틴 실행하라고 알림
            buffer[0] = (byte)Task.LABELLOAD;
            cli.Send(buffer, buffer.Length, SocketFlags.None);

            for (int i = 0; i < 4; i++)
            {
                cli.Receive(buffer, buffer.Length, SocketFlags.None);
                result[i] = System.Text.Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                Array.Clear(buffer, '\0', buffer.Length);
            }
            
        }
        return result; 
    }

    public string Load(int idx)
    {
        string result= null;
        if (isConnect)
        {
            byte[] buffer = new byte[s_mtu];

            //서버에게 노로그인 루틴 실행하라고 알림
            buffer[0] = (byte)Task.LOAD;
            cli.Send(buffer, buffer.Length, SocketFlags.None);

            buffer[0] = (byte)idx;
            cli.Send(buffer, buffer.Length, SocketFlags.None);


            int len;
            byte[] recvData = null;

            cli.Receive(buffer, buffer.Length, SocketFlags.None);
            len = BitConverter.ToInt32(buffer, 0);
            Debug.Log(len + "=================");

            if (len == -1) return result;

            recvData = new byte[len];
            cli.Receive(recvData, len, SocketFlags.None);


            if (recvData != null)
            {
                System.Text.Encoding.UTF8.GetString(recvData, 0, recvData.Length);
                result = System.Text.Encoding.UTF8.GetString(recvData);
                Debug.Log(result);
            }
            return result; 
        }
        return result; 
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
