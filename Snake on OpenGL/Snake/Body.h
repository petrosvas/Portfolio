#pragma once
#include<vector>
#include <algorithm>
#define UP 1
#define DOWN 2
#define LEFT 3
#define RIGHT 4
#define Width 640
#define Height 480

using namespace std;

struct part
{
	float positions[8];
	vector<int> dirs;
};

struct fruit
{
	float positions[8] = {
			(float)160 / Width,(float)160 / Height,
			(float)160 / Width,(float)200 / Height,
			(float)200 / Width,(float)200 / Height,
			(float)200 / Width,(float)160 / Height
	};
	void Generate()
	{
		int maxx = Width / 40, maxy = Height / 40;
		positions[0] = (float)((rand() % (maxx)) * 40) / Width;
		positions[1] = (float)((rand() % (maxy)) * 40) / Height;
		positions[2] = positions[0];
		positions[3] = positions[1] + (float)40 / Height;
		positions[4] = positions[2] + (float)40 / Width;
		positions[5] = positions[3];
		positions[6] = positions[4];
		positions[7] = positions[1];
	}
};

class Body
{
private:
	vector<part> parts;
	int currDir = UP;
public:
	Body();
	void Move(int);
	int GetLength();
	float* GetPosition(int); // int argument is the position of part in the vector for which the float array is returned
	void PartsUp();
};