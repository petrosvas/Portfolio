#include "Enemy.h"
#include <iostream>

Enemy::Enemy()
{
	int num = rand() % 3 + 3;

	part head;

	float w = (float)(rand() % (Width / 40 * 2) - Width / 40) / (Width / 40);
	float h = (float)(rand() % (Height / 40 * 2) - Height / 40) / (Height / 40);

	head.positions[0] = (float)0 / Width;
	head.positions[1] = (float)0 / Height;
	head.positions[2] = (float)0 / Width;
	head.positions[3] = (float)40 / Height;
	head.positions[4] = (float)40 / Width;
	head.positions[5] = (float)40 / Height;
	head.positions[6] = (float)40 / Width;
	head.positions[7] = (float)0 / Height;

	for (int i = 0; i < 8; i += 2)
	{
		head.positions[i] = (float)head.positions[i] + w;
		head.positions[i + 1] = (float)head.positions[i + 1] + h;
	}


	head.dirs.push_back(rand() % 4 + 1);
	parts.push_back(head);

	

	for (int a = 1; a < num; a++)
	{
		part p;
		p.positions[1] = parts[0].positions[1];
		p.positions[3] = parts[0].positions[3];
		p.positions[5] = parts[0].positions[5];
		p.positions[7] = parts[0].positions[7];
		p.positions[0] = parts[0].positions[0];
		p.positions[2] = parts[0].positions[2];
		p.positions[4] = parts[0].positions[4];
		p.positions[6] = parts[0].positions[6];

		for (int j = 0; j < 8; j += 2)
		{
			p.positions[j] += (float)40 * a / Width;
		}

		for (int i = 0; i < a; i++)
		{
			p.dirs.push_back(LEFT);
		}
		p.dirs.push_back(parts[0].dirs[0]);
		parts.push_back(p);
	}
}

void Enemy::Print()
{
	for (int i = 0; i < parts.size(); i++)
	{
		cout << i << ": " << endl;
		for (int j = 0; j < parts[i].dirs.size(); j++)
		{
			cout << parts[i].dirs[j] << ", ";
		}
		cout << endl;
	}
}

void Enemy::Move()
{
	int dir = rand() % 4 + 1;

	for (int i = 0; i < parts.size(); i++)
	{
		switch (parts[i].dirs[0])
		{
		case UP:
			for (int j = 1; j < 8; j += 2)
			{
				parts[i].positions[j] += (float)40 / Height;
			}
			break;
		case DOWN:
			for (int j = 1; j < 8; j += 2)
			{
				parts[i].positions[j] -= (float)40 / Height;
			}
			break;
		case LEFT:
			for (int j = 0; j < 8; j += 2)
			{
				parts[i].positions[j] -= (float)40 / Width;
			}
			break;
		case RIGHT:
			for (int j = 0; j < 8; j += 2)
			{
				parts[i].positions[j] += (float)40 / Width;
			}
			break;
		}
		if (i == 0)
		{
			parts[i].dirs.erase(parts[i].dirs.begin());
			parts[i].dirs.push_back(dir);
		}
		else
		{
			parts[i].dirs.erase(parts[i].dirs.begin());
			parts[i].dirs.push_back(parts[i - 1].dirs[parts[i - 1].dirs.size() - 1]);
		}
	}

}