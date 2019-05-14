#define _WINSOCK_DEPRECATED_NO_WARNINGS
#include "GameServer.h"

#include<stdio.h>
#include<stdlib.h>
#include<string.h>
#include<winsock2.h>
#include <Windows.h>
#include <mysql.h>

user* login(void *sock) {
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
	//if (!strcmp(id, "123") && !strcmp(password, "123"))
	//if(1)
	//if(strcmp(id, "") || strcmp(password, ""))
	if(server_login(id, password))
	{
		//클라이언트에게 로그인 성공했다고 알림
		packetData[0] = 1;
		send(*(SOCKET*)sock, packetData, PACKETSIZE, 0);
		printf("Login Success\n");

		//user 노드 생성		
		user* newUser = createUser(id, password);
		newUser->sock = sock;
		//userlist에 노드 추가
		push_back(&userlist, newUser);
		//유저 정보 출력
		printUser(newUser);
		//printUser(get_idx(&userlist, 0));

		return newUser;
	}
	else {
		//실패했다고 알림
		packetData[0] = 0;
		send(*(SOCKET*)sock, packetData, PACKETSIZE, 0);
		printf("Login Fail\n");
		return NULL;
	}
}

void signUp(void *sock) {
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

	
	if (server_signUp(id, password))
	{
		//클라이언트에게 회원가입 성공했다고 알림
		packetData[0] = 1;
		send(*(SOCKET*)sock, packetData, PACKETSIZE, 0);
		printf("SignUp Success (%s, %s)\n", id, password);
	}
	else {
		//실패했다고 알림
		packetData[0] = 0;
		send(*(SOCKET*)sock, packetData, PACKETSIZE, 0);
		printf("SignUp Fail\n");
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
	printf("%s가 고른 캐릭터 : %d\n", curUser->id, curUser->character);
}


room* roomMake(void *sock, user* curUser)
{
	printf("RoomMake\n");
	//요청한 유저의 방을 만듬
	room* newRoom = createRoom(curUser);
	//만든 방을 방 리스트에 넣음
	push_back(&roomlist, newRoom);
	//방 정보 출력
	printRoom(newRoom);
	
	return newRoom;
	
}

void roomList(void *sock)
{
	char packetData[PACKETSIZE] = { 0, };

	printf("RoomList\n");

	//방 갯수 전송
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


room* roomEnter(void *sock, user* curUser)
{
	char packetData[PACKETSIZE] = { 0, };
	int roomidx;


	//방 번호 받음
	recv(*(SOCKET*)sock, packetData, PACKETSIZE, 0);
	roomidx = packetData[0];

	printf("room Enter %d\n", roomidx);
	//전달받은 방 노드 받아옴
	room* target = get_idx(&roomlist, roomidx);
	

	//전달받은 방의 게스트 리스트에 요청한 유저 추가한다.
	push_back(&(target->guestlist), curUser);
	//디버깅용 방 정보 출력
	printRoom(target);

	/*
	방에 입장한 유저에게 방의 정보를 넘겨주어야 한다.
	방의 정보를 어떻게 표현할지 게임로직에 표현된다.
	잘 구현해보도록 하자.
	*/

	struct sockaddr_in addr;
	char name[256];
	int addr_size = sizeof(struct sockaddr_in);

	getpeername(*(target->owner->sock), (struct sockaddr *)&addr, &addr_size);
	strncpy_s(packetData, 16, inet_ntoa(addr.sin_addr), 16);
	printf("Owner IP Send : %s", packetData);

	//방 주인 IP 전달
	send(*(SOCKET*)sock, packetData, PACKETSIZE, 0);



	return target;
}

void save(void *sock, user* curUser)
{
	char packetData[PACKETSIZE] = { 0, };
	int dataLen = 0;


	recv(*(SOCKET*)sock, packetData, PACKETSIZE, 0);
	dataLen = *((int *)packetData);

	printf("recv data length %d\n", dataLen);

	char *saveData = malloc(dataLen); //db table에 id는 로그인 정보 data에 savedata의 데이터가 삽입-
	// 쿼리문 생성 => Id, data 모두 파악 가능하므로 쿼리문으로 db에 전달하여 저장
	int result = recv(*(SOCKET*)sock, saveData, dataLen, 0);
	if (result == dataLen)
	{
		printf("recv Success %s\n", saveData);
	}

	printf("call data_save function\n");
	//쿼리문 발송
	data_save(curUser->id, saveData, dataLen);
	
	
	//에러나서 죽여놓음 왜 죽는지 모르겠음
	//free(saveData);
}

void load(void *sock, user* curUser)
{
	char packetData[PACKETSIZE] = { 0, };
	memset(packetData, 0, PACKETSIZE);

	char result[65535];
	data_load(curUser->id, result);

	int len;

	printf("%s", result);
	

	if (result != NULL) {
		len = strlen(result);
		printf("%d", len);
		*((int *)packetData) = len;
		send(*(SOCKET*)sock, packetData, PACKETSIZE, 0);

		send(*(SOCKET*)sock, result, len, 0);
	}
	else {
		len = -1;
		*((int *)packetData) = len;
		send(*(SOCKET*)sock, packetData, PACKETSIZE, 0);
	}
}

void play(void *sock, user* curUser, room* curRoom)
{
	char packetData[PACKETSIZE] = { 0, };
	int recvDone = 0;
	int recvCnt = 0;
	int recvIndex = 0;


	while (1)
	{
		byte* buf = malloc(10240);
		recvDone = 0;
		recvCnt = 0;
		recvIndex = 0;

		printf("Wait Receive\n");

		//recv
		while (!recvDone) 
		{
			recvCnt = recv(*((SOCKET *)sock), &buf[recvIndex], PACKETSIZE, 0);
			if (recvCnt) {
				recvIndex += recvCnt;
			}
			else {
				recvDone = 1;
			}
		}
		printf("recvIndex : %d\n", recvIndex);
		//recvCnt = recv(*((SOCKET *)sock), &buf[recvIndex], sizeof(buf), 0);


		//send
		node* cur = curRoom->guestlist.head->next;
		while (cur) {
			send((*((user*)cur->value)->sock), buf, PACKETSIZE, 0);
			cur = cur->next;
		}

		Sleep(1000);

	}

}