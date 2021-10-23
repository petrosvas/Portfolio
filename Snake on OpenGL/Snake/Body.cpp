#include "Body.h"
#include <iostream>

	Body::Body()
	{
		part head;
		float tmp[8] = {
			(float)0 / Width,(float)0 / Height,
			(float)0 / Width,(float)40 / Height,
			(float)40 / Width,(float)40 / Height,
			(float)40 / Width,(float)0 / Height
		};
		for (int i = 0; i < 8; i++)
		{
			head.positions[i] = tmp[i];
		}
		head.dirs.push_back(UP);
		parts.push_back(head);

		part part1, part2;
		float tmp1[8] = {
			(float)0 / Width,(float)-40 / Height,
			(float)0 / Width,(float)0 / Height,
			(float)40 / Width,(float)0 / Height,
			(float)40 / Width,(float)-40 / Height
		};
		float tmp2[8] = {
			(float)0 / Width,(float)-80 / Height,
			(float)0 / Width,(float)-40 / Height,
			(float)40 / Width,(float)-40 / Height,
			(float)40 / Width,(float)-80 / Height
		};
		for (int i = 0; i < 8; i++)
		{
			part1.positions[i] = tmp1[i];
			part2.positions[i] = tmp2[i];
		}
		part1.dirs.push_back(UP);
		part1.dirs.push_back(UP);
		part2.dirs.push_back(UP);
		part2.dirs.push_back(UP);
		part2.dirs.push_back(UP);
		parts.push_back(part1);
		parts.push_back(part2);

	}

	int Body::GetLength()
	{
		return parts.size();
	}

	float* Body::GetPosition(int i) // int argument is the position of part in the vector for which the float array is returned
	{
		return parts[i].positions;
	}

	void Body::Move(int dir)
	{
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
		currDir = dir;
	}

	void Body::PartsUp()
	{
		part tail;
		int lastPart = parts.size() - 1;
		tail.dirs.push_back(parts[lastPart].dirs[0]);
		for (int i = 0; i < 8; i++)
		{
			tail.positions[i] = parts[lastPart].positions[i];
		}

		for (int dir : parts[lastPart].dirs)
		{
			tail.dirs.push_back(dir);
		}
		parts.push_back(tail);
		lastPart = parts.size() - 1;
		int firstDir = parts[lastPart].dirs[0];

		switch (firstDir)
		{
		case UP:
			for (int j = 1; j < 8; j += 2)
			{
				parts[lastPart].positions[j] -= (float)40 / Height;
			}
			break;
		case DOWN:
			for (int j = 1; j < 8; j += 2)
			{
				parts[lastPart].positions[j] += (float)40 / Height;
			}
			break;
		case LEFT:
			for (int j = 0; j < 8; j += 2)
			{
				parts[lastPart].positions[j] += (float)40 / Width;
			}
			break;
		case RIGHT:
			for (int j = 0; j < 8; j += 2)
			{
				parts[lastPart].positions[j] -= (float)40 / Width;
			}
			break;
		}
	}