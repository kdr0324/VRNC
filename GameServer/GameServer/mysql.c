

#include "mysql.h"
#include <stdio.h>



int server_login(char* id, char* password)
{
	char query[256]; // ������ ����
	//printf("%s, %s", id, password);
	sprintf_s(query,sizeof(query), "select * from login_data where id= '%s' and pw='%s';", id, password);
	printf("%s", query);
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
	if (res == NULL) {
		printf("failed\n");
		return 0; 
	}
	//// ���� ��� �� ���
	//while ((row = mysql_fetch_row(res)) != NULL) { // �� ROW �� ��� �´�
	//	printf("%s %s %s \n", row[0], row[1], row[2]); // ��� �� ���
	//}

	// row�� id, pw�� �� -> �α��� �������п��� ��ȯ


	// �Ҵ� �� �޸� ����
	mysql_free_result(res);
	mysql_close(conn_ptr);

	return 1;
}