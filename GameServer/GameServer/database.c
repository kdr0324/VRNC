//#define SOCKET int		// �̰� ���־�� �Ѵٰ� �ϴ����
#pragma comment(lib, "ws2_32.lib")


#include<stdio.h>
#include<string.h>
#include<mysql.h> // MySQL�� �����ϱ� ���� �� �ʿ��� �ش� ����



// Database�� �����ϱ� ���� �⺻���� ����
#define HOST "localhost" // ������ host �ּ�
#define USER "root" // ����� �̸�
#define PASS "qwe123" // �н�����
#define NAME "server_db" // ������ �����ͺ��̽� �̸�

int main(void) {

	char* query = "select * from login_data"; // ������ ����
	int len;

	MYSQL* conn_ptr; // MySQL���� ������ ���
	MYSQL_RES* res; // ������ ���� ����� �޴� ����
	MYSQL_ROW row; // ������ ���� ���� ������ ���� ����ִ� ����

	conn_ptr = mysql_init(NULL); // �ʱ�ȭ
	if (!conn_ptr) {
		printf("mysql_init failed!!\n");
	}

	// MySQL�� ����
	conn_ptr = mysql_real_connect(conn_ptr, HOST, USER, PASS, NAME, 3306, (char*)NULL, 0);

	if (conn_ptr) {
		printf("���� ����\n");
	}
	else {
		printf("���� ����\n");
	}

	// ���� ����
	// mysql_query() ���� �� ��ȯ���� 0�̾�� ����
	len = mysql_query(conn_ptr, query);

	res = mysql_store_result(conn_ptr); // ������ ��� ���� MYSQL_RES ������ ����

	// ���� ��� �� ���
	while ((row = mysql_fetch_row(res)) != NULL) { // �� ROW �� ��� �´�
		printf("%s %s %s \n", row[0], row[1], row[2]); // ��� �� ���
	}

	// �Ҵ� �� �޸� ����
	mysql_free_result(res);
	mysql_close(conn_ptr);

	return 0;
}