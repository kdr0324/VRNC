#pragma once

#include "list.h"
#include "User.h"

#ifndef _GAMEROOM_H_
#define _GAMEROOM_H_


typedef struct room {
	user* owner;
	//list guestlist;
} room;

room* createRoom(user* owner);
//void addUser(user* user);
void printRoom(room* a);
void printGuest(list* guestlist);


#endif