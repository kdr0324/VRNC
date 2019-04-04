#pragma once
#ifndef _GAMESERVER_H_

#define PACKETSIZE 1400

struct _user {
	char id[PACKETSIZE];
	char password[PACKETSIZE];
} user;

void login(void *sock);

#endif // !_GAMESERVER_H_
