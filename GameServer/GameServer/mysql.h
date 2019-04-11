#pragma once

#include<mysql.h> // mysql을 연결하기 위해 꼭 필요한 해더 파일

// database에 연결하기 위한 기본적인 정보
#define HOST "localhost" // 연결할 host 주소
#define USER "root" // 사용자 이름
#define PASS "qwe123" // 패스워드
#define NAME "server_db" // 접속할 데이터베이스 이름

int server_login(char* id, char* password);
