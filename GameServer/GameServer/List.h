#ifndef _LIST_H_
#define _LIST_H_


typedef struct node {
	struct node* next;
	void* value;
} node;

typedef struct list {
	node* head;
	node* last;
	int size;
} list;

void init_list(list* mylist);
void push_back(list* mylist, void* value);
void* get_idx(list* mylist, int idx);
int count_target(list* mylist, int target);
void clear_list(list* mylist);
void free_node(node* current_node);
void* find_node(list* mylist, int(*find)(void*));


#endif