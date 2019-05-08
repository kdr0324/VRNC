

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


int data_save(char* id, char* saveData, int dataLen)
{
	
	char * query = query = malloc(1024 + dataLen); // ������ ����;
	int left =0, right = 0;
	for (int i = 0; i < dataLen; i++)
	{
		if (saveData[i] == '"')
		{
			//saveData[i] = '$';
		}
		else if (saveData[i] == '{')
		{
			left++;
		}
		else if (saveData[i] == '}')
		{
			right++;
			if (left == right)
			{
				saveData[i + 1] = '\0';
				break;
			}

		}
	}

	
	sprintf_s(query, 1024 + dataLen, "insert into room_data values ('%s', '%s');", id, saveData);
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

	if (len != 0)
	{
		fprintf(stderr, "Mysql query error : %s", mysql_error(conn_ptr));
		return 0;
	}
	res = mysql_store_result(conn_ptr); // ������ ��� ���� MYSQL_RES ������ ����
	//if (res == NULL) {
	//	printf("failed\n");
	//	return 0;
	//}
	//// ���� ��� �� ���
	//while ((row = mysql_fetch_row(res)) != NULL) { // �� ROW �� ��� �´�
	//	printf("%s %s %s \n", row[0], row[1], row[2]); // ��� �� ���
	//}

	// row�� id, pw�� �� -> �α��� �������п��� ��ȯ


	// �Ҵ� �� �޸� ����
	mysql_free_result(res);
	mysql_close(conn_ptr);
	free(query);

	return 1;
}


MYSQL* data_load(char* id)
{

	char query[256]; // ������ ����
	
	sprintf_s(query, sizeof(query), "select data from room_data where id='%s';", id);
	
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

	if (len != 0)
	{
		fprintf(stderr, "Mysql query error : %s", mysql_error(conn_ptr));
		return 0;
	}


	return conn_ptr;
}

