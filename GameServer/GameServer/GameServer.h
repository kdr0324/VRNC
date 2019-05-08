#pragma once

#ifndef _GAMESERVER_H_
#define _GAMESERVER_H_

#include "List.h"
#include "User.h"
#include "GameRoom.h"
#include "Furniture.h"

#define PACKETSIZE 1400

list userlist;
list roomlist;

enum {
	NOLOGIN,
	LOGIN,
	CHARACTERSELECT,
	ROOMMAKING,
	ROOMLIST,
	ROOMENTER,
	PLAY,
	SAVE,
	LOAD
};

void noLogin(void *sock);
user* login(void *sock);
void characterSelect(void *sock, user* curUser);


room* roomMake(void *sock, user* curUser);
room* roomEnter(void *sock, user* curUser);
void roomList(void *sock);

void play(void *sock, user* curUser, room* curRoom);
void save(void *sock);
void load(void *sock);




#endif // !_GAMESERVER_H_


