#include "GameServer.h"


#include<stdio.h>
#include<stdlib.h>
#include<winsock2.h>
#include <Windows.h>

void login(void *sock) {
	//ID, PASSWORD 확인
	char packetData[PACKETSIZE] = { 0, };
	char id[PACKETSIZE] = { 0, };
	char password[PACKETSIZE] = { 0, };

	//ID 받음
	recv(*(SOCKET*)sock, id, PACKETSIZE, 0);
	printf("%s\n", id);


	//비밀번호 받음
	recv(*(SOCKET*)sock, password, PACKETSIZE, 0);
	printf("%s\n", password);

	//로그인 성공 -- DB에 계정정보 물어보는 코드 작성 필요 ///////////////	
	//일단 123 / 123 으로 테스트
	if (!strcmp(id, "123") && !strcmp(password, "123"))
	{
		//성공했다고 알림
		packetData[0] = 1;
		send(*(SOCKET*)sock, packetData, PACKETSIZE, 0);
		printf("Login Success\n");

		//유저 정보 추가 ( 현재는 단일변수이지만 리스트형식으로 유저 관리 필요)
		memcpy(user.id, id, strlen(id));
		memcpy(user.password, password, strlen(password));
	}
	else {
		//실패했다고 알림
		packetData[0] = 0;
		send(*(SOCKET*)sock, packetData, PACKETSIZE, 0);
		printf("Login Fail\n");
	}

	return 0;
}