//링커 명령줄에 ws2_32.lib 추가해야 소켓 통신 가능

#include<stdio.h>
#include<stdlib.h>
#include<winsock2.h>
#include <process.h>
#include <Windows.h>

unsigned WINAPI ThreadFunction(void* para)
{
	char message[1400] = { 0, };

	while (1) {
		recv(*(SOCKET*)para, message, sizeof(message), 0);
		printf("%s\n\n\n\n", message);
		Sleep(1000);
	}

	return 0;

}

int main(int argc, char *argv[])

{
	//Socket
	SOCKET sock, clientsock[10];
	WSADATA wsa;
	struct sockaddr_in sockinfo, clientinfo;
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

	clientsize = sizeof(clientinfo);

	

	while (1)
	{
		printf("클라이언트로부터 접속을 기다리고 있습니다...\n");
		clientsock[i] = accept(sock, (SOCKADDR*)&clientinfo, &clientsize);

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
		i++;
	}
	

	//send(clientsock, message, sizeof(message), 0);




	closesocket(sock);

	for (int i = 0; i < 10; i++)
		closesocket(clientsock[i]);

	WSACleanup();

	return 0;
}