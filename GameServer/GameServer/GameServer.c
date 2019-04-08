#include "GameServer.h"

#include<stdio.h>
#include<stdlib.h>
#include<string.h>
#include<winsock2.h>
#include <Windows.h>

user* login(void *sock) {
	//ID, PASSWORD Ȯ��
	char packetData[PACKETSIZE] = { 0, };
	char id[PACKETSIZE] = { 0, };
	char password[PACKETSIZE] = { 0, };

	//ID ����
	recv(*(SOCKET*)sock, id, PACKETSIZE, 0);
	printf("%s\n", id);


	//��й�ȣ ����
	recv(*(SOCKET*)sock, password, PACKETSIZE, 0);
	printf("%s\n", password);

	//�α��� ���� -- DB�� �������� ����� �ڵ� �ۼ� �ʿ� ///////////////	
	//�ϴ� 123 / 123 ���� �׽�Ʈ
	//if (!strcmp(id, "123") && !strcmp(password, "123"))
	if(strcmp(id, "") || strcmp(password, ""))
	{
		//Ŭ���̾�Ʈ���� �α��� �����ߴٰ� �˸�
		packetData[0] = 1;
		send(*(SOCKET*)sock, packetData, PACKETSIZE, 0);
		printf("Login Success\n");

		//user ��� ����		
		user* newUser = createUser(id, password);
		//userlist�� ��� �߰�
		push_back(&userlist, newUser);
		printUser(get_idx(&userlist, 0));

		return newUser;
	}
	else {
		//�����ߴٰ� �˸�
		packetData[0] = 0;
		send(*(SOCKET*)sock, packetData, PACKETSIZE, 0);
		printf("Login Fail\n");
		return NULL;
	}
}

void noLogin(void *sock)
{
	printf("NoLogin\n");
}

void characterSelect(void *sock, user* curUser)
{
	char packetData[PACKETSIZE] = { 0, };

	recv(*(SOCKET*)sock, packetData, PACKETSIZE, 0);
	curUser->character = (int)packetData[0];
	printf("%s�� �� ĳ���� : %d\n", curUser->id, curUser->character);
}


void roomMake(void *sock, user* curUser)
{
	printf("RoomMake\n");
	//��û�� ������ ���� ����
	room* newRoom = createRoom(curUser);
	//���� ���� �� ����Ʈ�� ����
	push_back(&roomlist, newRoom);
	printRoom(newRoom);
	//�� ���� ���
	//
}

void roomList(void *sock)
{
	char packetData[PACKETSIZE] = { 0, };

	printf("RoomList\n");

	//�� ���� ����
	packetData[0] = roomlist.size;
	send(*(SOCKET*)sock, packetData, PACKETSIZE, 0);

	for (int i = 0; i < roomlist.size; i++) {
		room* temp = get_idx(&roomlist, i);
		strcpy_s(packetData, sizeof(packetData), temp->owner->id);
		printf("%s\n", packetData);
		send(*(SOCKET*)sock, packetData, PACKETSIZE, 0);
		memset(packetData, 0, PACKETSIZE);
	}
}


void roomEnter(void *sock, user* curUser)
{
	char packetData[PACKETSIZE] = { 0, };
	int roomidx;


	//�� ��ȣ ����
	recv(*(SOCKET*)sock, packetData, PACKETSIZE, 0);
	roomidx = packetData[0];

	printf("room Enter %d\n", roomidx);
	//���޹��� �� ��� �޾ƿ�
	printf("1");
	room* target = get_idx(&roomlist, roomidx);
	

	//���޹��� ���� �Խ�Ʈ ����Ʈ�� ��û�� ���� �߰��Ѵ�.
	push_back(&(target->guestlist), curUser);
	printf("2");
	//������ �� ���� ���
	printRoom(target);

	/*
	�濡 ������ �������� ���� ������ �Ѱ��־�� �Ѵ�.
	���� ������ ��� ǥ������ ���ӷ����� ǥ���ȴ�.
	�� �����غ����� ����.
	*/

}