//링커 명령줄에 ws2_32.lib 추가해야 소켓 통신 가능

#include<stdio.h>
#include<stdlib.h>
#include<winsock2.h>
#include <process.h>
#include <Windows.h>

int USER[10];
char message[1400] = { 0, };

typedef struct _recvPkt {
	float x, y, z;
}recvPkt;

unsigned WINAPI ThreadFunction(void* para)
{

	char Dummy[1400] = { 0, };
	int i, c = 0;
	printf("Thread Start %d / %d\n", GetCurrentThreadId());
	while (1) {
		//if host
		if (USER[0] == GetCurrentThreadId()) {
			c = recv(*(SOCKET*)para, message, sizeof(recvPkt), 0);
			printf("%lf, %lf, %lf  \n",
				((recvPkt*)(message))->x, ((recvPkt*)(message))->y, ((recvPkt*)(message))->z);
			
		}
		else {
			c = recv(*(SOCKET*)para, Dummy, sizeof(recvPkt), 0);	
		}
		
		
		send(*(SOCKET*)para, message, sizeof(recvPkt), 0);
		//((recvPkt*)(message))->x += 0.01f;
		//((recvPkt*)(message))->y += 0.01f;
		//((recvPkt*)(message))->z += 0.01f;

	/*	for (int i = 0; i < 10; i++) {
			if(USER[i])
				send(*(SOCKET*)para, message, sizeof(recvPkt), 0);
		}*/
		

		Sleep(17);
	}

	printf("연결 해제");
	return 0;

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

	sock = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);

	if (sock == INVALID_SOCKET)
		printf("소켓 생성 실패\n");

	memset(&sockinfo, 0, sizeof(sockinfo));

	sockinfo.sin_family = AF_INET;
	sockinfo.sin_port = htons(12345);
	sockinfo.sin_addr.s_addr = htonl(INADDR_ANY);

	if (bind(sock, (SOCKADDR*)&sockinfo, sizeof(sockinfo)) == SOCKET_ERROR)
		printf(" bind 실패 ");


	if (listen(sock, 5) == SOCKET_ERROR)
		printf(" 대기열 실패 ");

	clientsize = sizeof(clientinfo[0]);

	

	while (1)
	{
		printf("클라이언트로부터 접속을 기다리고 있습니다...\n");
		clientsock[i] = accept(sock, (SOCKADDR*)&(clientinfo[i]), &clientsize);
		

		if (clientsock[i] == INVALID_SOCKET)
			printf(" 클라이언트 소켓 연결 실패 ");


		//쓰레드 호출
		printf("thread address : %d\n", clientsock + i);
		hThread = (HANDLE)_beginthreadex(NULL, 0, ThreadFunction, (void*)(clientsock + i), 0, &threadID);
		if (0 == hThread)
		{
			puts("_beginthreadex() error");
			return -1;
		}
		USER[i] = threadID;
		i++;
	}


	closesocket(sock);

	for (int i = 0; i < 10; i++)
		closesocket(clientsock[i]);

	WSACleanup();

	return 0;
}