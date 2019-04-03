using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class SocketSampleTCP : MonoBehaviour
{
	class _presentBox
    {
        public float x, y, z;
    };
	// 접속할 곳의 IP주소.
	private string			m_address = "127.0.0.1";
	// 접속할 곳의 포트 번호.
	private const int 		m_port = 12345;

	// 리스닝 소켓.
	private Socket			m_listener = null;

	// 통신용 변수.
	private Socket			m_socket = null;

	// 상태. 
	private State			m_state;

	// 상태정의. 
	private enum State
	{
		SelectHost = 0,
		StartListener,
		AcceptClient,
		ServerCommunication,
		StopListener,
		ClientCommunication,
		Endcommunication,
	}


	// Use this for initialization
	void Start ()
	{
		m_state = State.ClientCommunication;
        

        Debug.Log("[TCP]Start client communication.");

        // 서버에 접속.
        m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        m_socket.NoDelay = true;
        m_socket.SendBufferSize = 0;
        m_socket.Connect(m_address, m_port);

    }
	
	// Update is called once per frame
	void Update ()
	{


        
        switch (m_state) {

		case State.ClientCommunication:
			ClientProcess();
			break;

		default:
			break;
		}
	}


	void ClientProcess()
	{
        // 메시지 송신.
        byte[] buffer = new byte[1400];/* = System.Text.Encoding.UTF8.GetBytes("Hello, this is client.\n");*/
        _presentBox presentBox = new _presentBox();
        presentBox.x = transform.position.x;
        presentBox.y = transform.position.y;
        presentBox.z = transform.position.z;

        String str = String.Format("{0:F5},{1:F5},{2:F5}", presentBox.x, presentBox.y, presentBox.z);
        buffer = System.Text.Encoding.UTF8.GetBytes(str);
        Debug.Log(str);
        Debug.Log(str.Length);
        m_socket.Send(buffer, buffer.Length, SocketFlags.None);

        /* = System.Text.Encoding.UTF8.GetBytes("Hello, this is client.\n");*/
        //m_socket.Send(buffer, buffer.Length, SocketFlags.None);
    }

}
