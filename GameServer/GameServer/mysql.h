#pragma once

#include<mysql.h> // mysql�� �����ϱ� ���� �� �ʿ��� �ش� ����

// database�� �����ϱ� ���� �⺻���� ����
#define HOST "192.168.1.84" // ������ host �ּ�
#define USER "root" // ����� �̸�
#define PASS "qwe123" // �н�����
#define NAME "server_db" // ������ �����ͺ��̽� �̸�

int server_login(char* id, char* password);
int server_signUp(char* id, char* password);
int data_save(char* id, char* saveData, int dataLen);
char* data_load(char* id, char* result); 