﻿using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;


public class TransportUDP : MonoBehaviour {

	//
	// 소켓 접속 관련.
	//

	// 클라이언트와의 접속용 소켓.
	private Socket			m_socket = null;

	// 송신 버퍼.
	private PacketQueue		m_sendQueue;
	
	// 수신 버퍼.
	private PacketQueue		m_recvQueue;
	
	// 서버 플래그.	
	private bool	 		m_isServer = false;

	// 접속 플래그.
	private	bool			m_isConnected = false;

	//
	// 이벤트 관련 멤버 변수.
	//

	// 이벤트 통지 델리게이트.
	public delegate void 	EventHandler(NetEventState state);

	private EventHandler	m_handler;

	//
	// 스레드 관련 멤버 변수.
	//

	// 스레드 실행 플래그.
	protected bool			m_threadLoop = false;
	
	protected Thread		m_thread = null;

	private static int 		s_mtu = 1400;


	// Use this for initialization
	void Start ()
    {

        // 송수신 버퍼를 만듭니다.
        m_sendQueue = new PacketQueue();
        m_recvQueue = new PacketQueue();	
	}
	
	// Update is called once per frame
	void Update ()
    {
	}

	// 대기시작.
	public bool StartServer(int port, int connectionNum)
	{
        Debug.Log("StartServer called.!");

        // 리스닝 소켓을 생성합니다. 
        try {
			// 소켓을 생성합니다.
			m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			// 사용할 포트 번호를 할당합니다.
			m_socket.Bind(new IPEndPoint(IPAddress.Any, port));
        }
        catch {
			Debug.Log("StartServer fail");
            return false;
        }

        m_isServer = true;

        return LaunchThread();
    }

	// 대기 종료 .
    public void StopServer()
    {
		m_threadLoop = false;
        if (m_thread != null) {
            m_thread.Join();
            m_thread = null;
        }

        Disconnect();

		if (m_socket != null) {
			m_socket.Close();
			m_socket = null;
        }

        m_isServer = false;

        Debug.Log("Server stopped.");
    }


    // 접속.
    public bool Connect(string address, int port)
    {
        Debug.Log("TransportTCP connect called.");

		if (m_socket != null) {
            return false;
        }

		bool ret = false;
        try {
			m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            m_socket.NoDelay = true;
            m_socket.Connect(address, port);
			ret = LaunchThread();
		}
        catch {
            m_socket = null;
        }

		if (ret == true) {
			m_isConnected = true;
			Debug.Log("Connection success.");
		}
		else {
			m_isConnected = false;
			Debug.Log("Connect fail");
		}

        if (m_handler != null) {
            // 접속 결과를 통지합니다. 
			NetEventState state = new NetEventState();
			state.type = NetEventType.Connect;
			state.result = (m_isConnected == true) ? NetEventResult.Success : NetEventResult.Failure;
            m_handler(state);
			Debug.Log("event handler called");
        }

        return m_isConnected;
    }

	// 끊기.
    public void Disconnect() {
        m_isConnected = false;

        if (m_socket != null) {
            // 소켓 클로즈.
            m_socket.Shutdown(SocketShutdown.Both);
            m_socket.Close();
            m_socket = null;
        }

        // 끊기를 통지합니다.
        if (m_handler != null) {
			NetEventState state = new NetEventState();
			state.type = NetEventType.Disconnect;
			state.result = NetEventResult.Success;
			m_handler(state);
        }
    }

    // 송신처리.
    public int Send(byte[] data, int size)
	{
		if (m_sendQueue == null) {
			return 0;
		}

        return m_sendQueue.Enqueue(data, size);
    }

    // 수신처리.
    public int Receive(ref byte[] buffer, int size)
	{
		if (m_recvQueue == null) {
			return 0;
		}

        return m_recvQueue.Dequeue(ref buffer, size);
    }

	// 이벤트 통지 함수 등록.
    public void RegisterEventHandler(EventHandler handler)
    {
        m_handler += handler;
    }

	// 이벤트 통지 함수 삭제.
    public void UnregisterEventHandler(EventHandler handler)
    {
        m_handler -= handler;
    }

	// 스레드 실행 함수.
	bool LaunchThread()
	{
		try {
			// Dispatch용 스레드 시작.
			m_threadLoop = true;
			m_thread = new Thread(new ThreadStart(Dispatch));
			m_thread.Start();
		}
		catch {
			Debug.Log("Cannot launch thread.");
			return false;
		}
		
		return true;
	}

	// 스레드의 송수신 처리.
    public void Dispatch()
	{
		Debug.Log("Dispatch thread started.");

		while (m_threadLoop) {

			// 클라이언트와의 송수신을 처리합니다.
			if (m_socket != null && m_isConnected == true) {

	            // 송신처리.
	            DispatchSend();

	            // 수신처리.
	            DispatchReceive();
	        }

			Thread.Sleep(5);
		}

		Debug.Log("Dispatch thread ended.");
    }

	// 스레드의 송수신처리.
    void DispatchSend()
	{
        try {
            // 송신처리.
            if (m_socket.Poll(0, SelectMode.SelectWrite)) {
				byte[] buffer = new byte[s_mtu];

                int sendSize = m_sendQueue.Dequeue(ref buffer, buffer.Length);
                while (sendSize > 0) {
                    m_socket.Send(buffer, sendSize, SocketFlags.None);
                    sendSize = m_sendQueue.Dequeue(ref buffer, buffer.Length);
                }
            }
        }
        catch {
            return;
        }
    }

	// 스레드의 수신처리.
    void DispatchReceive()
	{
        // 수신처리.
        try {
            while (m_socket.Poll(0, SelectMode.SelectRead)) {
				byte[] buffer = new byte[s_mtu];

                int recvSize = m_socket.Receive(buffer, buffer.Length, SocketFlags.None);
                if (recvSize == 0) {
                    // 끊기.
                    Debug.Log("Disconnect recv from client.");
                    Disconnect();
                }
                else if (recvSize > 0) {
                    m_recvQueue.Enqueue(buffer, recvSize);
                }
            }
        }
        catch {
            return;
        }
    }

	// 서버인지 확인.
	public bool IsServer() {
		return m_isServer;
	}
	
    // 접속 확인.
    public bool IsConnected() {
        return m_isConnected;
    }

}
