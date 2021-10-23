#include <GL/glew.h>
#include <iostream>
#include <GLFW/glfw3.h>
#include <ctime>
#include <Windows.h>
#include <CONIO.H>
#include "Body.h"
#include "Enemy.h"

static GLenum CompileShader(GLenum type, const std::string& source)
{
	GLenum id = glCreateShader(type);
	const char* newSource = &source[0];
	glShaderSource(id, 1, &newSource, NULL);
	glCompileShader(id);
	return id;
}

static GLenum NewShader(const std::string& vertex, const std::string& fragment)
{
	GLenum pr = glCreateProgram();
	GLenum vs = CompileShader(GL_VERTEX_SHADER, vertex);
	GLenum fs = CompileShader(GL_FRAGMENT_SHADER, fragment);

	glAttachShader(pr, vs);
	glAttachShader(pr, fs);
	glLinkProgram(pr);
	glValidateProgram(pr);
	glDeleteShader(vs);
	glDeleteShader(fs);
	return pr;
}

int main()
{
	Enemy en1, en2;
	float ranx, rany;
	fruit fr{};
	fr.Generate();
	char chars = 0;
	bool ch = 0;
	int finalDir = UP;

	GLFWwindow* window;

	if (!glfwInit())
		return -1;


	window = glfwCreateWindow(Width, Height, "Hello World", NULL, NULL);
	if (!window)
	{
		glfwTerminate();
		return -1;
	}

	glfwMakeContextCurrent(window);

	GLenum err = glewInit();
	if (err != GLEW_OK)
	{
		std::cout << "Error!\n";
	}

	Body b;

	GLenum buffer;
	glGenBuffers(1, &buffer);
	glBindBuffer(GL_ARRAY_BUFFER, buffer);

	glEnableVertexAttribArray(0);
	glVertexAttribPointer(0, 2, GL_FLOAT, GL_FALSE, sizeof(float) * 2, 0);

	std::string vertex =
		"#version 330 \n layout(location = 0) in vec4 position; \n"
		"void main(){ \n"
		"gl_Position = position; \n"
		"}";

	std::string fragmentRed =
		"#version 330 \n layout(location = 0) out vec4 color; \n"
		"void main(){ \n"
		"color = vec4(1.0,0.0,0.0,1.0); \n"
		"}";

	std::string fragmentGreen =
		"#version 330 \n layout(location = 0) out vec4 color; \n"
		"void main(){ \n"
		"color = vec4(0.0,1.0,0.0,1.0); \n"
		"}";

	std::string fragmentDarkGreen =
		"#version 330 \n layout(location = 0) out vec4 color; \n"
		"void main(){ \n"
		"color = vec4(0.0,0.7,0.0,1.0); \n"
		"}";

	std::string fragmentBlue =
		"#version 330 \n layout(location = 0) out vec4 color; \n"
		"void main(){ \n"
		"color = vec4(0.0,0.0,1.0,1.0); \n"
		"}";

	GLenum shaderRed = NewShader(vertex, fragmentRed);
	GLenum shaderGreen = NewShader(vertex, fragmentGreen);
	GLenum shaderDarkGreen = NewShader(vertex, fragmentDarkGreen);
	GLenum shaderBlue = NewShader(vertex, fragmentBlue);

	while (!glfwWindowShouldClose(window))
	{
		ch = _kbhit();

		glClear(GL_COLOR_BUFFER_BIT);
		glUseProgram(shaderDarkGreen);

		float tmp[8];
		float* ptr = b.GetPosition(0);

		if (ptr[0] <= -0.999 || ptr[0] >= 0.999)
		{
			cout << "You lost! You got out of bounds\n";
			break;
		}
		else if (ptr[1] <= -0.999 || ptr[1] >= 0.999)
		{
			cout << "You lost!\n";
			break;
		}
		else
		{
			bool lost = false;
			for (int i = 0; i < en1.GetLength(); i++)
			{
				float* t = en1.GetPosition(i);
				if (abs(ptr[0] - t[0]) < 0.0001 && abs(ptr[1] - t[1]) < 0.0001)
				{
					cout << "You lost. You hit an enemy snake!!\n" << endl;
					lost = true;
					break;
				}
			}


			for (int i = 0; i < en2.GetLength(); i++)
			{
				float* t = en2.GetPosition(i);
				if (abs(ptr[0] - t[0]) < 0.0001 && abs(ptr[1] - t[1]) < 0.0001)
				{
					cout << "You lost. You hit an enemy snake!!\n" << endl;
					lost = true;
					break;
				}
			}
			if (lost)
				break;
		}

		bool lost = false;
		for (int i = 1; i < b.GetLength(); i++)
		{
			float* t = b.GetPosition(i);
			if (abs(ptr[0] - t[0]) < 0.0001 && abs(ptr[1] - t[1]) < 0.0001)
			{
				cout << "You lost. You hit your body!!\n" << endl;
				lost = true;
				break;
			}
		}
		if (lost)
			break;

		cout << ptr[0] << ", " << ptr[1] << endl;

		for (int i = 0; i < 8; i++)
		{
			tmp[i] = ptr[i];
		}
		glBufferData(GL_ARRAY_BUFFER, 8 * sizeof(float), tmp, GL_STATIC_DRAW);
		glDrawArrays(GL_POLYGON, 0, 4);

		glUseProgram(shaderGreen);

		for (int i = 1; i < b.GetLength(); i++)
		{
			float tmp[8];
			float* ptr = b.GetPosition(i);
			for (int i = 0; i < 8; i++)
			{
				tmp[i] = ptr[i];
			}
			glBufferData(GL_ARRAY_BUFFER, 8 * sizeof(float), tmp, GL_STATIC_DRAW);
			glDrawArrays(GL_POLYGON, 0, 4);
		}

		glUseProgram(shaderBlue);

		float t[8];
		float* p = fr.positions;
		for (int i = 0; i < 8; i++)
		{
			t[i] = p[i];
		}
		glBufferData(GL_ARRAY_BUFFER, 8 * sizeof(float), t, GL_STATIC_DRAW);
		glDrawArrays(GL_POLYGON, 0, 4);

		if (abs(tmp[0] - t[0]) < 0.0001 && abs(tmp[1] - t[1]) < 0.0001)
		{
			cout << "Fruit eaten!!\n" << endl;
			b.PartsUp();
			fr.Generate();
		}


		if (ch)
		{
			chars = _getch();
			if (chars == 'w' || chars == 'W')
			{
				finalDir = UP;
			}
			else if (chars == 'a' || chars == 'A')
			{
				finalDir = LEFT;
			}
			else if (chars == 's' || chars == 'S')
			{
				finalDir = DOWN;
			}
			else if (chars == 'd' || chars == 'D')
			{
				finalDir = RIGHT;
			}
		}
		else
		{
			chars = 0;
		}

		b.Move(finalDir);



		glUseProgram(shaderRed);

		for (int i = 0; i < en1.GetLength(); i++)
		{
			float tmp[8];

			float* ptr = en1.GetPosition(i);
			for (int i = 0; i < 8; i++)
			{
				tmp[i] = ptr[i];
			}

			glBufferData(GL_ARRAY_BUFFER, 8 * sizeof(float), tmp, GL_STATIC_DRAW);
			glDrawArrays(GL_POLYGON, 0, 4);
		}

		en1.Move();

		for (int i = 0; i < en2.GetLength(); i++)
		{
			float tmp[8];

			float* ptr = en2.GetPosition(i);
			for (int i = 0; i < 8; i++)
			{
				tmp[i] = ptr[i];
			}

			glBufferData(GL_ARRAY_BUFFER, 8 * sizeof(float), tmp, GL_STATIC_DRAW);
			glDrawArrays(GL_POLYGON, 0, 4);
		}

		en2.Move();

		glfwSwapBuffers(window);

		glfwPollEvents();

		Sleep(500);
	}

	glfwTerminate();
	cin.get();
	return 0;
}
