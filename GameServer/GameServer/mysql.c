

#include "mysql.h"
#include <stdio.h>



int server_login(char* id, char* password)
{
	char query[256]; // 실행할 쿼리
	//printf("%s, %s", id, password);
	sprintf_s(query,sizeof(query), "select * from login_data where id= '%s' and pw='%s';", id, password);
	printf("%s", query);
	int len;

	MYSQL* conn_ptr; // MySQL과의 연결을 담당
	MYSQL_RES* res; // 쿼리에 대한 결과를 받는 변수
	MYSQL_ROW row; // 쿼리에 대한 실제 데이터 값이 들어있는 변수

	conn_ptr = mysql_init(NULL); // 초기화
	if (!conn_ptr) {
		printf("mysql_init failed!!\n");
	}

	// MySQL에 연결
	conn_ptr = mysql_real_connect(conn_ptr, HOST, USER, PASS, NAME, 3306, (char*)NULL, 0);

	if (conn_ptr) {
		printf("연결 성공\n");
	}
	else {
		printf("연결 실패\n");
	}

	// 쿼리 실행
	// mysql_query() 실행 후 반환값이 0이어야 성공
	len = mysql_query(conn_ptr, query);

	
	res = mysql_store_result(conn_ptr); // 전속한 결과 값을 MYSQL_RES 변수에 저장
	if (res == NULL) {
		printf("failed\n");
		return 0; 
	}
	//// 쿼리 결과 값 출력
	//while ((row = mysql_fetch_row(res)) != NULL) { // 한 ROW 씩 얻어 온다
	//	printf("%s %s %s \n", row[0], row[1], row[2]); // 결과 값 출력
	//}

	// row랑 id, pw를 비교 -> 로그인 성공실패여부 반환

	int result = mysql_num_rows(res);
	// 할당 된 메모리 해제
	mysql_free_result(res);
	mysql_close(conn_ptr);

	return result;
}

int server_signUp(char* id, char* password)
{
	char query[256]; // 실행할 쿼리
	//printf("%s, %s", id, password);
	sprintf_s(query, sizeof(query), "insert into login_data values ('%s', '%s');", id, password);
	printf("%s", query);
	int len;

	MYSQL* conn_ptr; // MySQL과의 연결을 담당
	MYSQL_RES* res; // 쿼리에 대한 결과를 받는 변수
	MYSQL_ROW row; // 쿼리에 대한 실제 데이터 값이 들어있는 변수

	//mysql 초기화
	conn_ptr = mysql_init(NULL); // 초기화
	if (!conn_ptr) {
		printf("mysql_init failed!!\n");
	}

	// MySQL에 연결
	conn_ptr = mysql_real_connect(conn_ptr, HOST, USER, PASS, NAME, 3306, (char*)NULL, 0);

	if (conn_ptr) {
		printf("연결 성공\n");
	}
	else {
		printf("연결 실패\n");
		return 0;
	}

	// 쿼리 실행
	// mysql_query() 실행 후 반환값이 0이어야 성공
	len = mysql_query(conn_ptr, query);

	if (len != 0)
		return 0;

	// 할당 된 메모리 해제
	mysql_close(conn_ptr);

	return 1;
}

int data_save(char* id, char* saveData, int dataLen, int idx)
{
	
	char * query = query = malloc(1024 + dataLen); // 실행할 쿼리;
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

	
	sprintf_s(query, 1024 + dataLen ,
		"insert into room_data values('%s', '%d', '%s', CURRENT_TIMESTAMP, '') on duplicate key update Data = values (Data), Savetime = values(Savetime), Screenshot = values(Screenshot);",
		id, idx, saveData);
	printf("%s", query);
	int len;

	MYSQL* conn_ptr; // MySQL과의 연결을 담당
	MYSQL_RES* res; // 쿼리에 대한 결과를 받는 변수
	MYSQL_ROW row; // 쿼리에 대한 실제 데이터 값이 들어있는 변수

	conn_ptr = mysql_init(NULL); // 초기화
	if (!conn_ptr) {
		printf("mysql_init failed!!\n");
	}

	// MySQL에 연결
	conn_ptr = mysql_real_connect(conn_ptr, HOST, USER, PASS, NAME, 3306, (char*)NULL, 0);

	if (conn_ptr) {
		printf("연결 성공\n");
	}
	else {
		printf("연결 실패\n");
	}

	// 쿼리 실행
	// mysql_query() 실행 후 반환값이 0이어야 성공
	len = mysql_query(conn_ptr, query);

	if (len != 0)
	{
		fprintf(stderr, "Mysql query error : %s", mysql_error(conn_ptr));
		return 0;
	}
	res = mysql_store_result(conn_ptr); // 전속한 결과 값을 MYSQL_RES 변수에 저장
	//if (res == NULL) {
	//	printf("failed\n");
	//	return 0;
	//}
	//// 쿼리 결과 값 출력
	//while ((row = mysql_fetch_row(res)) != NULL) { // 한 ROW 씩 얻어 온다
	//	printf("%s %s %s \n", row[0], row[1], row[2]); // 결과 값 출력
	//}

	// row랑 id, pw를 비교 -> 로그인 성공실패여부 반환


	// 할당 된 메모리 해제
	mysql_free_result(res);
	mysql_close(conn_ptr);
	free(query);

	return 1;
}


char* data_load(char* id, char * result, int idx)
{

	char query[256]; // 실행할 쿼리
	
	sprintf_s(query, sizeof(query), "select data from room_data where ID='%s' AND Num=%d", id, idx);
	
	printf("%s", query);
	int len;

	MYSQL* conn_ptr; // MySQL과의 연결을 담당
	MYSQL_RES* res; // 쿼리에 대한 결과를 받는 변수
	MYSQL_ROW row; // 쿼리에 대한 실제 데이터 값이 들어있는 변수

	conn_ptr = mysql_init(NULL); // 초기화
	if (!conn_ptr) {
		printf("mysql_init failed!!\n");
	}

	// MySQL에 연결
	conn_ptr = mysql_real_connect(conn_ptr, HOST, USER, PASS, NAME, 3306, (char*)NULL, 0);

	if (conn_ptr) {
		printf("연결 성공\n");
	}
	else {
		printf("연결 실패\n");
	}

	// 쿼리 실행
	// mysql_query() 실행 후 반환값이 0이어야 성공
	len = mysql_query(conn_ptr, query);

	if (len != 0)
	{
		fprintf(stderr, "Mysql query error : %s", mysql_error(conn_ptr));
		return 0;
	}
	res = mysql_store_result(conn_ptr); // 전속한 결과 값을 MYSQL_RES 변수에 저장
	if (res == NULL) {
		printf("failed\n");
		return 0;
	}
	// 쿼리 결과 값 출력

	

	if ((row = mysql_fetch_row(res)) != NULL) { // 한 ROW 씩 얻어 온다
		//result = malloc(strlen(row[0])+1);
		printf("len == %d", strlen(row[0]));

		strncpy_s(result, 65535, row[0], strlen(row[0]));
		printf("%s \n", result); // 결과 값 출력
		
	}
	else { 
		return NULL; 
	}

	// 할당 된 메모리 해제
	//mysql_free_result(res);
	mysql_close(conn_ptr);
	
	return result;
}
char* label_load(char* id, char * result, int idx)
{

	char query[256]; // 실행할 쿼리

	sprintf_s(query, sizeof(query), "select Savetime from room_data where id='%s' and Num=%d;", id, idx);

	printf("%s", query);
	int len;

	MYSQL* conn_ptr; // MySQL과의 연결을 담당
	MYSQL_RES* res; // 쿼리에 대한 결과를 받는 변수
	MYSQL_ROW row; // 쿼리에 대한 실제 데이터 값이 들어있는 변수

	conn_ptr = mysql_init(NULL); // 초기화
	if (!conn_ptr) {
		printf("mysql_init failed!!\n");
	}

	// MySQL에 연결
	conn_ptr = mysql_real_connect(conn_ptr, HOST, USER, PASS, NAME, 3306, (char*)NULL, 0);

	if (conn_ptr) {
		printf("연결 성공\n");
	}
	else {
		printf("연결 실패\n");
	}

	// 쿼리 실행
	// mysql_query() 실행 후 반환값이 0이어야 성공
	len = mysql_query(conn_ptr, query);

	if (len != 0)
	{
		fprintf(stderr, "Mysql query error : %s", mysql_error(conn_ptr));
		return 0;
	}
	res = mysql_store_result(conn_ptr); // 전속한 결과 값을 MYSQL_RES 변수에 저장
	if (res == NULL) {
		printf("failed\n");
		return 0;
	}
	// 쿼리 결과 값 출력



	if ((row = mysql_fetch_row(res)) != NULL) { // 한 ROW 씩 얻어 온다
		//result = malloc(strlen(row[0])+1);
		printf("len == %d", strlen(row[0]));

		strncpy_s(result, 50, row[0], strlen(row[0]));
		printf("%s \n", result); // 결과 값 출력

	}
	else {
		return NULL;
	}

	// 할당 된 메모리 해제
	//mysql_free_result(res);
	mysql_close(conn_ptr);

	return result;

}