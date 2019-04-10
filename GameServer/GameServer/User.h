#ifndef _USER_H_
#define _USER_H_

#include<winsock2.h>

#define PACKETSIZE 1400

typedef struct user {
	char id[PACKETSIZE];
	char password[PACKETSIZE];
	SOCKET *sock;
	int character;
} user;

user* createUser(char *id, char* password);
void printUser(user *a);

#endif // !_USER_H_
