#pragma once

#include<mysql.h> // mysql�� �����ϱ� ���� �� �ʿ��� �ش� ����

// database�� �����ϱ� ���� �⺻���� ����
#define HOST "localhost" // ������ host �ּ�
#define USER "root" // ����� �̸�
#define PASS "qwe123" // �н�����
#define NAME "server_db" // ������ �����ͺ��̽� �̸�

int server_login(char* id, char* password);
