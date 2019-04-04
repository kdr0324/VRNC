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

    //통신용 버퍼와 사이즈
    //private byte[] buffer;
    private const int s_mtu = 1400;

    //락
    private object lockObject = new object();
    //쓰레드
    private Thread thread = null;

    // Start is called before the first frame update
    void Start()
    {
        cli = GetComponent<LibraryClient>().Connect();
        //thread = new Thread(LoginTask);
        //thread.Start();
    }

    enum Task {
        NOLOGIN = 0,
        LOGIN = 1,
        ROOMMAKE = 2,
        ROOMLIST = 3,
        ROOMENTER = 4
    }

    

    public void Login()
    {
        //로그인 정보 전송
        string id, pwd;
        byte[] buffer = new byte[s_mtu];

        //서버에게 로그인 루틴 실행하라고 알림
        buffer[0] = (byte)Task.LOGIN;
        cli.Send(buffer, buffer.Length, SocketFlags.None);
        Array.Clear(buffer, 0, 1);

        //로그인 정보 날림
        id = GameObject.Find("InputFieldID").GetComponent<InputField>().text;
        System.Text.Encoding.UTF8.GetBytes(id).CopyTo(buffer, 0);
        cli.Send(buffer, buffer.Length, SocketFlags.None);
        Array.Clear(buffer, 0, buffer.Length);

        pwd = GameObject.Find("InputFieldPWD").GetComponent<InputField>().text;
        System.Text.Encoding.UTF8.GetBytes(pwd).CopyTo(buffer, 0);
        cli.Send(buffer, buffer.Length, SocketFlags.None);
        Array.Clear(buffer, 0, buffer.Length);



        //로그인 성공 여부 받음
        cli.Receive(buffer, buffer.Length, SocketFlags.None);
        if (buffer[0] == 1)
        {
            //성공
            pwd = "Success";
        }
        else if(buffer[0] == 0)
        {
            //실패
            pwd = "Fail";
            GameObject.Find("InputFieldID").GetComponent<InputField>().text = "";
            GameObject.Find("InputFieldPWD").GetComponent<InputField>().text = "";
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
        Debug.Log("Room Make");
        byte[] buffer = new byte[s_mtu];

        //서버에게 방만들기 루틴 실행하라고 알림
        buffer[0] = (byte)Task.ROOMMAKE;
        cli.Send(buffer, buffer.Length, SocketFlags.None);

        /*
         * 
         * 게임 진입
         * 
         * 
         */
    }

    public void RoomList()
    {
        Debug.Log("Room List");
        int n;
        byte[] buffer = new byte[s_mtu];

        //서버에게 방 리스트 루틴 실행하라고 알림
        buffer[0] = (byte)Task.ROOMLIST;
        cli.Send(buffer, buffer.Length, SocketFlags.None);

        //방 몇개 있는지 받음
        cli.Receive(buffer, buffer.Length, SocketFlags.None);
        n = buffer[0];

        //방 개수 만큼 방 정보 받음
        for(int i = 0; i < n; i++)
        {
            cli.Receive(buffer, buffer.Length, SocketFlags.None);
            string str = System.Text.Encoding.UTF8.GetString(buffer) + "의 방";
            Debug.Log(str + "'s Room");


            Array.Clear(buffer, 0, buffer.Length);
        }


    }

    public void RoomEnter()
    {
        Debug.Log("Room Enter");
        byte[] buffer = new byte[s_mtu];

        //서버에게 방 리스트 루틴 실행하라고 알림
        buffer[0] = (byte)Task.ROOMENTER;
        cli.Send(buffer, buffer.Length, SocketFlags.None);

        //방 번호 선택 // 일단 0번 지정, 리스트에 나열하는 순서대로 방 번호
        buffer[0] = 0;
        //들어가고 방 번호 전달
        cli.Send(buffer, buffer.Length, SocketFlags.None);

        /*
         * 
         * 게임 진입
         * 
         * 
         */
    }
}
