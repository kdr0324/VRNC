#include "GameServer.h"

#include<stdio.h>
#include<stdlib.h>
#include<string.h>
#include<winsock2.h>
#include <Windows.h>

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
	if(strcmp(id, "") || strcmp(password, ""))
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
	printf("1");
	room* target = get_idx(&roomlist, roomidx);
	

	//전달받은 방의 게스트 리스트에 요청한 유저 추가한다.
	push_back(&(target->guestlist), curUser);
	printf("2");
	//디버깅용 방 정보 출력
	printRoom(target);

	/*
	방에 입장한 유저에게 방의 정보를 넘겨주어야 한다.
	방의 정보를 어떻게 표현할지 게임로직에 표현된다.
	잘 구현해보도록 하자.
	*/

	return target;
}

void play(void *sock, user* curUser, room* curRoom)
{
	char packetData[PACKETSIZE] = { 0, };
	int recvDone = 0;
	int recvCnt = 0;
	int recvIndex = 0;
	printf("***************************\n");
	printf("Play Start\n");
	furniture *f = malloc(sizeof(furniture));
	f->px = 1.0;
	f->py = 2.0;
	f->pz = 1.0;
	f->rx = 0.0;
	f->ry = 0.0;
	f->rz = 0.0;
	f->w = 0.0;
	strcpy_s(f->name,sizeof(f->name),"bed_1");

	while (1)
	{
		byte* buf = malloc(10240);
		printf("Wait Receive\n");

		//받음
		while (!recvDone) 
		{
			recvCnt = recv(*((SOCKET *)sock), &buf[recvIndex], sizeof(buf), 0);
			if (recvCnt) {
				recvIndex += recvCnt;
				if (recvIndex == sizeof(buf))
					recvDone = 1;
			}
		}
		//recvCnt = recv(*((SOCKET *)sock), &buf[recvIndex], sizeof(buf), 0);

		node* cur = curRoom->guestlist.head->next;
		while (cur) {
			send((*((user*)cur->value)->sock), buf, sizeof(buf), 0);
			cur = cur->next;
		}

		Sleep(1000);
		//send(*((SOCKET *)sock), buf, sizeof(buf), 0);
		

		/*printf("Sending...\n");
		send(*((SOCKET *)sock), f, sizeof(furniture), 0);
		printFurniture(f);
		Sleep(1000);*/
	}

}