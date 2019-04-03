//��Ŀ ����ٿ� ws2_32.lib �߰��ؾ� ���� ��� ����

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
		printf("�ʱ�ȭ ����\n");

	sock = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);

	if (sock == INVALID_SOCKET)
		printf("���� ���� ����\n");

	memset(&sockinfo, 0, sizeof(sockinfo));

	sockinfo.sin_family = AF_INET;
	sockinfo.sin_port = htons(12345);
	sockinfo.sin_addr.s_addr = htonl(INADDR_ANY);

	if (bind(sock, (SOCKADDR*)&sockinfo, sizeof(sockinfo)) == SOCKET_ERROR)
		printf(" bind ���� ");


	if (listen(sock, 5) == SOCKET_ERROR)
		printf(" ��⿭ ���� ");

	clientsize = sizeof(clientinfo);

	

	while (1)
	{
		printf("Ŭ���̾�Ʈ�κ��� ������ ��ٸ��� �ֽ��ϴ�...\n");
		clientsock[i] = accept(sock, (SOCKADDR*)&clientinfo, &clientsize);

		if (clientsock[i] == INVALID_SOCKET)
			printf(" Ŭ���̾�Ʈ ���� ���� ���� ");


		//������ ȣ��
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