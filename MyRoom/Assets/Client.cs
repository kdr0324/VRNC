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
    private int s_mtu = 1400;

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
        LOGIN = 0,
    }

    

    public void Login()
    {
        //로그인 정보 전송
        string id, pwd;
        byte[] buffer = new byte[1400];

        //서버에게 로그인 루틴 실행하라고 알림
        buffer[0] = (byte)Task.LOGIN;
        cli.Send(buffer, buffer.Length, SocketFlags.None);
        Array.Clear(buffer, 0, buffer.Length);

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

}
