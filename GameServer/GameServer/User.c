#include "User.h"
#include <stdlib.h>


//유저 노드를 생성한다 (id, password) 
user* createUser(char *id, char* password)
{
	user* newUser = (user*)malloc(sizeof(user));
	if (id != NULL)
	{
		strcpy(newUser->id, id, strlen(id));
		strcpy(newUser->password, password, strlen(password));
	}
	return newUser;
}

void printUser(user* a)
{
	printf("-----------------------------------\n");
	printf("User Info\n");
	printf("id : %s\n", a->id);
	printf("password : %s\n", a->password);
	printf("-----------------------------------\n");
}

int findUser(user *a)
{
	return 1;
}