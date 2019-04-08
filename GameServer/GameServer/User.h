#ifndef _USER_H_
#define _USER_H_


#define PACKETSIZE 1400

typedef struct user {
	char id[PACKETSIZE];
	char password[PACKETSIZE];
	
	int character;
} user;

user* createUser(char *id, char* password);
void printUser(user *a);

#endif // !_USER_H_
