#include "GameServer.h"


#include<stdio.h>
#include<stdlib.h>
#include<winsock2.h>
#include <Windows.h>

void login(void *sock) {
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
	if (!strcmp(id, "123") && !strcmp(password, "123"))
	{
		//�����ߴٰ� �˸�
		packetData[0] = 1;
		send(*(SOCKET*)sock, packetData, PACKETSIZE, 0);
		printf("Login Success\n");

		//���� ���� �߰� ( ����� ���Ϻ��������� ����Ʈ�������� ���� ���� �ʿ�)
		memcpy(user.id, id, strlen(id));
		memcpy(user.password, password, strlen(password));
	}
	else {
		//�����ߴٰ� �˸�
		packetData[0] = 0;
		send(*(SOCKET*)sock, packetData, PACKETSIZE, 0);
		printf("Login Fail\n");
	}

	return 0;
}