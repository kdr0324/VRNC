#pragma once

#include<mysql.h> // mysql�� �����ϱ� ���� �� �ʿ��� �ش� ����

// database�� �����ϱ� ���� �⺻���� ����
#define HOST "localhost" // ������ host �ּ�
#define USER "root" // ����� �̸�
#define PASS "qwe123" // �н�����
#define NAME "server_db" // ������ �����ͺ��̽� �̸�

int server_login(char* id, char* password);
int data_save(char* id, char* saveData, int dataLen);
MYSQL* data_load(char* id);