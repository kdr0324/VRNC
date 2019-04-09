
// TCP로 통신할 때는 정의를 유효하게 하여 주십시오.
#define USE_TRANSPORT_TCP

using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System;

public class LibraryClient : MonoBehaviour {

    private Socket m_socket;

    struct PacketData
    {
        public float x, y, z;
    };


    // 접속할 곳의 IP주소.
    public string m_address = "127.0.0.1";
    // 접속할 곳의 포트 번호.
    public const int m_port = 12345;

	public const int 	m_mtu = 1400;

	private bool isSelected = false;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public Socket Connect()
    {
        m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        m_socket.NoDelay = true;
        m_socket.SendBufferSize = 0;
        m_socket.Connect(m_address, m_port);

        return m_socket;
    }

    public void Disconnect()
    {
        m_socket.Disconnect(true);
    }

    public void SendPacket()
    {

    }

    public void RecvPacket()
    {

    }


}