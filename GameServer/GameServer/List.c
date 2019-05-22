#include "list.h"
#include <stdlib.h>

void init_list(list* mylist) {
	mylist->head = (node*)malloc(sizeof(node));
	mylist->head->next = NULL;
	mylist->last = mylist->head;
	mylist->size = 0;
}

void push_back(list* mylist, void* value) {
	node* new_node = (node*)malloc(sizeof(node));
	new_node->value = value;
	new_node->next = NULL;
	mylist->last->next = new_node;
	mylist->last = new_node;
	mylist->size++;
}

void* get_idx(list* mylist, int idx) {
	node* pre_node = mylist->head;
	while (idx--) {
		pre_node = pre_node->next;
	}
	return pre_node->next->value;
}

int count_target(list* mylist, int target) {
	int count = 0;
	for (node* pre_node = mylist->head; pre_node->next != NULL; pre_node = pre_node->next) {
		if (pre_node->next->value == target) {
			count++;
		}
	}
	return count;
}

void clear_list(list* mylist) {
	free_node(mylist->head);
	init_list(mylist);
}

void free_node(node* current_node) {
	if (current_node->next != NULL) {
		free_node(current_node->next);
	}
	free(current_node);
}

int find_node(list* mylist,void *data, int(*find)(void*, void*)){
	int count = 0;
	for (node* pre_node = mylist->head; pre_node->next != NULL; pre_node = pre_node->next) {
		if (!find(pre_node->next->value, data)) { 
			return 0;
		}
	}
	return 1;

}

void* delete_node(list* mylist, void *data, int(*find)(void*, void*)) {
	node* cur;
	void* del;
	for (node* pre_node = mylist->head; pre_node->next != NULL; pre_node = pre_node->next) {
		if (!find(pre_node->next->value, data)) {
			cur = pre_node->next;
			pre_node->next = cur->next;
			del = cur->value;
			free_node(cur);
			return del;
		}
	}
	return NULL;
}