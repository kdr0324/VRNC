
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

    byte[] getBytes(PacketData str)
    {
        int size = Marshal.SizeOf(str);
        byte[] arr = new byte[size];

        IntPtr ptr = Marshal.AllocHGlobal(size);
        Marshal.StructureToPtr(str, ptr, true);
        Marshal.Copy(ptr, arr, 0, size);
        Marshal.FreeHGlobal(ptr);
        return arr;
    }


    PacketData fromBytes(byte[] arr)
    {
        PacketData str = new PacketData();

        int size = Marshal.SizeOf(str);
        IntPtr ptr = Marshal.AllocHGlobal(size);

        Marshal.Copy(arr, 0, ptr, size);

        str = (PacketData)Marshal.PtrToStructure(ptr, str.GetType());
        Marshal.FreeHGlobal(ptr);

        return str;
    }
}