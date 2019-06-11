
#pragma comment(lib, "ws2_32.lib")		//��Ŀ ����ٿ� ws2_32.lib �߰��ؾ� ���� ��� ����

#include<stdio.h>
#include<stdlib.h>
#include<winsock2.h>
#include <process.h>
#include <Windows.h>

#include "GameServer.h"

int cmpRoom(void* data1, void* data2)
{
	return strcmp(((room*)data1)->owner->id, ((room*)data2)->owner->id);
}

int cmpUser(void* data1, void* data2)
{
	return strcmp(((user*)data1)->id, ((user*)data2)->id);
}


//����Ǵ� ������ �κ�
unsigned WINAPI ThreadFunction(void* para)
{
	//������ ���� ��ƾ
	int c = 1;

	//�ְ� ���� �� ����� ���� ũ��� PACKETSIZE(1400)
	char PacketData[PACKETSIZE] = { 0, };
	user* curUser = NULL;
	room* curRoom = NULL;
	while (c) 
	{
		//� �� ���� ���� ���� ���� ( ù ��° ����Ʈ�� ���� ���� )
		c = recv(*(SOCKET*)para, PacketData, PACKETSIZE, 0);

		if (!c)
			break;
		//���� ���� ���� ����
		/*
			0 - Login
		*/
		switch (PacketData[0])
		{
		case NOLOGIN:
			c = 0;
			noLogin(para);
			break;
		case LOGIN:
			curUser = login(para);
			break;
		case SIGNUP:
			printf("Start signUp\n");
			signUp(para);
			break;
		case CHARACTERSELECT:
			characterSelect(para, curUser);
			break;
		case ROOMMAKING:
			curRoom = roomMake(para, curUser);
			break;
		case ROOMLIST:
			roomList(para);
			break;
		case ROOMENTER:
			roomEnter(para, curUser);
			printf("start\n");
			if (curRoom)
			{
				delete_node(&roomlist, curRoom, cmpRoom);
				free(curRoom);
				curRoom = NULL;
			}
			printf("End\n");
			break;
		case PLAY:
			play(para, curUser, curRoom);
			break;
		case SAVE:
			save(para, curUser);
			break;
		case LOAD:
			load(para, curUser);
			break;
		case LABELLOAD:
			labelload(para, curUser); 
			break; 
		}

	}

	printf("���� ����\n");
	//�� ����
	if (curRoom)
	{
		delete_node(&roomlist, curRoom, cmpRoom);
		free(curRoom);
	}

	//���� ����
	if (curUser)
	{
		delete_node(&userlist, curUser, cmpUser);
		free(curUser);
	}
	printf("���� �Ϸ�\n");

	//Ŭ���̾�Ʈ ���� ����
	closesocket(*(SOCKET*)para);
	//�������õ� �޸� ���� �ʿ�
	//�ش� ������ ���Ե� �ڷᱸ�� ��� ���� �ʿ�
	/////////////////////////////



	////////////////////////////


	printf("Socket Close\n");

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

	
	//userlist �ʱ�ȭ
	init_list(&userlist);
	//roomlist �ʱ�ȭ
	init_list(&roomlist);


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
		clientsock[i] = accept(sock, /*(SOCKADDR*)*/&(clientinfo[i]), &clientsize);
		
		
		if (clientsock[i] == INVALID_SOCKET)
			printf(" Ŭ���̾�Ʈ ���� ���� ���� ");


		//������ ȣ��, ThreadFunction�� ȣ���
		printf("thread address : %d\n", clientsock[i] + i);
		
		hThread = (HANDLE)_beginthreadex(NULL, 0, ThreadFunction, (void*)(clientsock + i), 0, &threadID);
		if (0 == hThread)
		{
			puts("_beginthreadex() error");
			return -1;
		}


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