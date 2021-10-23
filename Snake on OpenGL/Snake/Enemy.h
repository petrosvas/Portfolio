#pragma once
#include "Body.h"

class Enemy
{
private:
	vector<part> parts;
public:
	Enemy();
	void Move();
	inline int GetLength() { return parts.size(); }
	inline float* GetPosition(int i) { return parts[i].positions; }
	void Print();
};