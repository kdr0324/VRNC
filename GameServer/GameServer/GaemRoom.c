#include "GameRoom.h"
#include <stdlib.h>

room* createRoom(user* owner)
{
	room* newRoom = (room*)malloc(sizeof(room));
	newRoom->owner = owner;
	init_list(&(newRoom->guestlist));
	push_back(&(newRoom->guestlist), owner);

	return newRoom;
}


void printRoom(room* a)
{
	printf("-----------------------------------\n");
	printf("Room Info\n");
	printf("owner : %s\n", (a->owner)->id);
	printf("-----------------------------------\n");
	printf("Guest Info\n");
	printGuest(&(a->guestlist));
	printf("-----------------------------------\n");
}


void printGuest(list* guestlist) 
{
	node* cur = guestlist->head->next;
	while (cur)
	{
		printf("guest name : %s\n", ((user*)(cur->value))->id);
		cur = cur->next;
	}
}