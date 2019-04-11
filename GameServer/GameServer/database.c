//#define SOCKET int		// 이거 해주어야 한다고 하더라고
#pragma comment(lib, "ws2_32.lib")


#include<stdio.h>
#include<string.h>
#include<mysql.h> // MySQL을 연결하기 위해 꼭 필요한 해더 파일



// Database에 연결하기 위한 기본적인 정보
#define HOST "localhost" // 연결할 host 주소
#define USER "root" // 사용자 이름
#define PASS "qwe123" // 패스워드
#define NAME "server_db" // 접속할 데이터베이스 이름

int main(void) {

	char* query = "select * from login_data"; // 실행할 쿼리
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

	// 쿼리 결과 값 출력
	while ((row = mysql_fetch_row(res)) != NULL) { // 한 ROW 씩 얻어 온다
		printf("%s %s %s \n", row[0], row[1], row[2]); // 결과 값 출력
	}

	// 할당 된 메모리 해제
	mysql_free_result(res);
	mysql_close(conn_ptr);

	return 0;
}