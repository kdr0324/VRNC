//��Ŀ ����ٿ� ws2_32.lib �߰��ؾ� ���� ��� ����

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


//����Ǵ� ������ �κ�
unsigned WINAPI ThreadFunction(void* para)
{
	//�ְ� ���� �� ����� ���� ũ��� PACKETSIZE(1400)
	char PacketData[PACKETSIZE] = { 0, };
	while (1) 
	{
		//� �� ���� ���� ���� ���� ( ù ��° ����Ʈ�� ���� ���� )
		recv(*(SOCKET*)para, PacketData, PACKETSIZE, 0);

		//���� ���� ���� ����
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
		printf("�ʱ�ȭ ����\n");


	//���� ����
	sock = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);

	if (sock == INVALID_SOCKET)
		printf("���� ���� ����\n");

	memset(&sockinfo, 0, sizeof(sockinfo));

	//���� ���� ����, IP, Port ��
	sockinfo.sin_family = AF_INET;
	sockinfo.sin_port = htons(12345);
	sockinfo.sin_addr.s_addr = htonl(INADDR_ANY);

	//bind
	if (bind(sock, (SOCKADDR*)&sockinfo, sizeof(sockinfo)) == SOCKET_ERROR)
		printf(" bind ���� ");

	//listen
	if (listen(sock, 5) == SOCKET_ERROR)
		printf(" ��⿭ ���� ");

	clientsize = sizeof(clientinfo[0]);


	//�ݺ��ؼ� accept �Ǵ� client ���� ����
	while (1)
	{
		//accept
		printf("Ŭ���̾�Ʈ�κ��� ������ ��ٸ��� �ֽ��ϴ�...\n");
		clientsock[i] = accept(sock, (SOCKADDR*)&(clientinfo[i]), &clientsize);
		

		if (clientsock[i] == INVALID_SOCKET)
			printf(" Ŭ���̾�Ʈ ���� ���� ���� ");


		//������ ȣ��, ThreadFunction�� ȣ���
		printf("thread address : %d\n", clientsock + i);
		hThread = (HANDLE)_beginthreadex(NULL, 0, ThreadFunction, (void*)(clientsock + i), 0, &threadID);
		if (0 == hThread)
		{
			puts("_beginthreadex() error");
			return -1;
		}

		//������ ������ ���� ID ����
		//*****����Ʈ �ڷᱸ���� ���� ���� �� �����带 ���ÿ� ������ �� �ְ� ���� �ʿ�****
		USER[i] = threadID;

		//���� üũ
		i++;
	}


	//���� ���� üũ
	//������ ����ȵǴ� �κ� 
	closesocket(sock);

	for (int i = 0; i < 10; i++)
		closesocket(clientsock[i]);

	WSACleanup();

	return 0;
}