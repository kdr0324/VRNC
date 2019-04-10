#pragma once


#ifndef _FURNITURE_H_
#define _FURNITURE_H_


typedef struct furniture {
	float px, py, pz;
	float rx, ry, rz, w;
	char name[20];
} furniture;


void printFurniture(furniture* f);


#endif //FURNITURE