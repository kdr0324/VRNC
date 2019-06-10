#pragma once
#include "Furniture.h"


void printFurniture(furniture* f)
{
	printf("name: %s\n", f->name);
	printf("Position : (%lf, %lf, %lf)\n", f->px, f->py, f->pz);
	printf("Qutanion : (%lf, %lf, %lf, %lf)\n", f->rx, f->ry, f->rz, f->w);
	
}