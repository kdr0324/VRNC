//링커 명령줄에 ws2_32.lib 추가해야 소켓 통신 가능

#include<stdio.h>
#include<stdlib.h>
#include<winsock2.h>
#include <process.h>
#include <Windows.h>

#include "GameServer.h"

int USER[10];
char message[1400] = { 0, };

typedef struct _recvPkt {
	float x, y, z;
}recvPkt;


//실행되는 쓰레드 부분
unsigned WINAPI ThreadFunction(void* para)
{
	//주고 받을 때 사용할 버퍼 크기는 PACKETSIZE(1400)
	char PacketData[PACKETSIZE] = { 0, };
	while (1) 
	{
		//어떤 일 할지 먼저 전달 받음 ( 첫 번째 바이트로 전달 받음 )
		recv(*(SOCKET*)para, PacketData, PACKETSIZE, 0);

		//전달 받은 내용 실행
		/*
			0 - Login
		*/
		switch (PacketData[0])
		{
		case 0:
			login(para);
			break;
		}

	}

}

int main(int argc, char *argv[])

{
	//Socket
	SOCKET sock, clientsock[10];
	WSADATA wsa;
	struct sockaddr_in sockinfo, clientinfo[10];
	int clientsize, i = 0;
	char message[1400] = { 0, };

	//Thread
	HANDLE hThread;
	unsigned threadID;
	int num = 0;


	if (WSAStartup(MAKEWORD(2, 2), &wsa) != 0)
		printf("초기화 실패\n");


	//소켓 생성
	sock = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);

	if (sock == INVALID_SOCKET)
		printf("소켓 생성 실패\n");

	memset(&sockinfo, 0, sizeof(sockinfo));

	//소켓 정보 설정, IP, Port 등
	sockinfo.sin_family = AF_INET;
	sockinfo.sin_port = htons(12345);
	sockinfo.sin_addr.s_addr = htonl(INADDR_ANY);

	//bind
	if (bind(sock, (SOCKADDR*)&sockinfo, sizeof(sockinfo)) == SOCKET_ERROR)
		printf(" bind 실패 ");

	//listen
	if (listen(sock, 5) == SOCKET_ERROR)
		printf(" 대기열 실패 ");

	clientsize = sizeof(clientinfo[0]);


	//반복해서 accept 되는 client 마다 연결
	while (1)
	{
		//accept
		printf("클라이언트로부터 접속을 기다리고 있습니다...\n");
		clientsock[i] = accept(sock, (SOCKADDR*)&(clientinfo[i]), &clientsize);
		

		if (clientsock[i] == INVALID_SOCKET)
			printf(" 클라이언트 소켓 연결 실패 ");


		//쓰레드 호출, ThreadFunction이 호출됨
		printf("thread address : %d\n", clientsock + i);
		hThread = (HANDLE)_beginthreadex(NULL, 0, ThreadFunction, (void*)(clientsock + i), 0, &threadID);
		if (0 == hThread)
		{
			puts("_beginthreadex() error");
			return -1;
		}

		//쓰레드 관리를 위해 ID 저장
		//*****리스트 자료구조를 통해 유저 및 쓰레드를 동시에 관리할 수 있게 수정 필요****
		USER[i] = threadID;

		//유저 체크
		i++;
	}


	//소켓 종료 체크
	//실제로 실행안되는 부분 
	closesocket(sock);

	for (int i = 0; i < 10; i++)
		closesocket(clientsock[i]);

	WSACleanup();

	return 0;
}