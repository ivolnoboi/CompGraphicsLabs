#include <GL/glew.h>
#include <gl/GL.h>   // GL.h header file    
#include <gl/GLU.h> // GLU.h header file     
#include <gl/freeglut.h>
#include <gl/glaux.h>
#include <glm/trigonometric.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <iostream>
#include "GL/SOIL.h"
using namespace std;

GLint Program;

GLint Unif_matrix;

glm::mat4 Matrix_projection;

//! Вершина 
struct vertex
{
	GLfloat x;
	GLfloat y;
	GLfloat z;
};

//! Функция печати лога шейдера 
void shaderLog(unsigned int shader)
{
	int   infologLen = 0;
	int   charsWritten = 0;
	char* infoLog;
	glGetShaderiv(shader, GL_INFO_LOG_LENGTH, &infologLen);
	if (infologLen > 1)
	{
		infoLog = new char[infologLen];
		if (infoLog == NULL)
		{
			std::cout << "ERROR: Could not allocate InfoLog buffer\n";
			exit(1);
		}
		glGetShaderInfoLog(shader, infologLen, &charsWritten, infoLog);
		std::cout << "InfoLog: " << infoLog << "\n\n\n";
		delete[] infoLog;
	}
}


//! Проверка ошибок OpenGL, если есть то вывод в консоль тип ошибки 
void checkOpenGLerror()
{
	GLenum errCode;
	if ((errCode = glGetError()) != GL_NO_ERROR)
		std::cout << "OpenGl error! - " << gluErrorString(errCode);
}

//! Инициализация шейдеров 
void initShader()
{
	//! Исходный код шейдеров  	
	const char* vsSource =
		"#version 330 core\n"
		"layout(location = 0) in vec3 position;\n"
		"layout(location = 1) in vec3 color;\n"
		"layout(location = 2) in vec2 texCoord;\n"
		"uniform mat4 matrix;\n"
		"out vec3 ourColor;\n"
		"out vec2 TexCoord;\n"
		"void main()\n"
		"{\n"
		"gl_Position = matrix * vec4(position, 0.5f);\n"
		"ourColor = color;\n"
		"TexCoord = texCoord;\n"
		"}\n";
	const char* fsSource =
		"in vec3 ourColor;\n"
		"in vec2 TexCoord;\n"
		"out vec4 color;\n"
		"uniform sampler2D ourTexture1;\n"
		"uniform sampler2D ourTexture2;\n"
		"void main() {\n"
		//"color = texture(ourTexture1, TexCoord);\n"  //1 текстура 
		//"color = texture(ourTexture1, TexCoord)  * vec4(ourColor, 1.0f);\n" //  1 текстура + цвет 
		//" color = mix(texture(ourTexture1, TexCoord), texture(ourTexture2, TexCoord), 0.5);\n" // 2 текстуры
		" color = mix(texture(ourTexture1, TexCoord), texture(ourTexture2, TexCoord), 0.5) * vec4(ourColor, 1.0f);\n" // 2 текстуры + цвет
		"}\n";
	//! Переменные для хранения идентификаторов шейдеров 
	GLuint vShader, fShader;
	//! Создаем вершинный шейдер 
	vShader = glCreateShader(GL_VERTEX_SHADER);
	//! Передаем исходный код  
	glShaderSource(vShader, 1, &vsSource, NULL);

	//! Компилируем шейдер  	
	glCompileShader(vShader);

	std::cout << "vertex shader \n";
	shaderLog(vShader);

	//! Создаем фрагментный шейдер 
	fShader = glCreateShader(GL_FRAGMENT_SHADER);
	//! Передаем исходный код 
	glShaderSource(fShader, 1, &fsSource, NULL);
	//! Компилируем шейдер  	
	glCompileShader(fShader);
	std::cout << "fragment shader \n";  	shaderLog(fShader);

	//! Создаем программу и прикрепляем шейдеры к ней 
	Program = glCreateProgram();  	glAttachShader(Program, vShader);  	glAttachShader(Program, fShader);

	//! Линкуем шейдерную программу  	
	glLinkProgram(Program);

	//! Проверяем статус сборки 
	int link_ok;
	glGetProgramiv(Program, GL_LINK_STATUS, &link_ok);  	if (!link_ok)
	{
		std::cout << "error attach shaders \n";
		return;
	}
	///! Вытягиваем ID атрибута из собранной программы   	
	/*const char* attr_name = "coord";
	Attrib_vertex = glGetAttribLocation(Program, attr_name);
	if (Attrib_vertex == -1)
	{
		std::cout << "could not bind attrib " << attr_name << std::endl;
		return;
	}*/

	///! Вытягиваем ID атрибута из собранной программы   	
	/*attr_name = "color";
	Attrib_color = glGetAttribLocation(Program, attr_name);
	if (Attrib_color == -1)
	{
		std::cout << "could not bind attrib " << attr_name << std::endl;
		return;
	}*/

	const char* attr_name = "matrix";
	Unif_matrix = glGetUniformLocation(Program, attr_name);

	checkOpenGLerror();
}

// Load and create a texture 
GLuint texture1;
GLuint texture2;
void text()
{

	// ====================
	// Texture 1
	// ====================
	glGenTextures(1, &texture1);
	glBindTexture(GL_TEXTURE_2D, texture1); // All upcoming GL_TEXTURE_2D operations now have effect on our texture object
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT);	
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
	// Load, create texture and generate mipmaps
	int width, height;
	unsigned char* image = SOIL_load_image("img/list.jpg", &width, &height, 0, SOIL_LOAD_RGB);
	glTexImage2D(GL_TEXTURE_2D, 0, GL_RGB, width, height, 0, GL_RGB, GL_UNSIGNED_BYTE, image);
	glGenerateMipmap(GL_TEXTURE_2D);
	SOIL_free_image_data(image);
	glBindTexture(GL_TEXTURE_2D, 0); 

	// ===================
	// Texture 2
	// ===================
	glGenTextures(1, &texture2);
	glBindTexture(GL_TEXTURE_2D, texture2);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
	// Load, create texture and generate mipmaps
	image = SOIL_load_image("img/awesomeface.png", &width, &height, 0, SOIL_LOAD_RGB);
	glTexImage2D(GL_TEXTURE_2D, 0, GL_RGB, width, height, 0, GL_RGB, GL_UNSIGNED_BYTE, image);
	glGenerateMipmap(GL_TEXTURE_2D);
	SOIL_free_image_data(image);
	glBindTexture(GL_TEXTURE_2D, 0);
}

GLuint VBO, VAO, EBO;
//! Инициализация VBO 
void initVBO()
{
	GLfloat vertices[] = {

		// лицевая грань
		// Positions          // Colors           // Texture Coords
		 0.5f,  0.5f, 0.0f,   1.0f, 0.0f, 0.0f,   1.0f, 1.0f, //0 = 7 // Top Right
		 0.5f, -0.5f, 0.0f,   0.0f, 1.0f, 0.0f,   1.0f, 0.0f, //1 = 8 // Bottom Right
		-0.5f, -0.5f, 0.0f,   0.0f, 0.0f, 1.0f,   0.0f, 0.0f, //2 = 12 // Bottom Left
		-0.5f,  0.5f, 0.0f,   1.0f, 1.0f, 0.0f,   0.0f, 1.0f, //3 = 11 // Top Left 

		// верхняя крышка
		 // Positions          // Colors           // Texture Coords
		 0.5f,  0.5f, -1.0f,   1.0f, 0.5f, 0.0f,   1.0f, 0.0f, //4
		-0.5f,  0.5f, -1.0f,   1.0f, 1.0f, 0.5f,   0.0f, 0.0f, //5

		// правая боковая крышка
		0.5f,  -0.5f, -1.0f,   1.0f, 0.5f, 0.5f,  0.0f, 0.0f, //6 = 10
		0.5f,  0.5f, 0.0f,   1.0f, 0.0f, 0.0f,   1.0f, 1.0f, //7 = 0
		0.5f, -0.5f, 0.0f,   0.0f, 1.0f, 0.0f,  0.0f, 1.0f, //8 = 1

		// Дальняя крышка
		-0.5f,  -0.5f, -1.0f,   1.0f, 0.0f, 1.0f,  0.0f, 1.0f,  // 9
		0.5f,  -0.5f, -1.0f,    1.0f, 0.5f, 0.5f,  1.0f, 1.0f,// 10 = 6

		// Левая крышка 
		-0.5f, 0.5f, 0.0f,   1.0f, 1.0f, 0.0f,   1.0f, 0.0f, // 11 = 3
		-0.5f,  -0.5f, 0.0f,  0.0f, 0.0f, 1.0f,     1.0f, 1.0f, // 12 = 2
	};
	GLuint indices[] = { 

		// лицевая грань
		0, 1, 3, 
		1, 2, 3,  

		// верхняя крышка
		0, 3, 5, 
		0, 4, 5, 

		// правая крышка
		7, 8, 6,
		7, 6, 4,

		// дальняя крышка
		5, 9, 10,
		5, 10, 4,

		// Нижняя крышка
		2, 9, 1,
		9, 10, 1,

		//левая крышка
		11, 12, 9,
		11, 9, 5
	};

	glGenVertexArrays(1, &VAO);
	glGenBuffers(1, &VBO);
	glGenBuffers(1, &EBO);

	glBindVertexArray(VAO);

	glBindBuffer(GL_ARRAY_BUFFER, VBO);
	glBufferData(GL_ARRAY_BUFFER, sizeof(vertices), vertices, GL_STATIC_DRAW);

	glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, EBO);
	glBufferData(GL_ELEMENT_ARRAY_BUFFER, sizeof(indices), indices, GL_STATIC_DRAW);



	checkOpenGLerror();
}

//! Освобождение шейдеров 
void freeShader()
{
	//! Передавая ноль, мы отключаем шейдрную программу 
	glUseProgram(0);
	//! Удаляем шейдерную программу  
	glDeleteProgram(Program);
}

//! Освобождение буфера
void freeVBO()
{
	glBindBuffer(GL_ARRAY_BUFFER, 0);
	glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);
/*	glDeleteBuffers(1, &VBO_vertex);
	glDeleteBuffers(1, &VBO_element);
	glDeleteBuffers(1, &VBO_color);*/

}
double angle_x = 0;

void resizeWindow(int width, int height)
{
	glViewport(0, 0, width, height);
}


//! Отрисовка 
void render()
{
	angle_x += 0.0005;

	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

	glm::mat4 Projection = glm::perspective(glm::radians(45.0f), 4.0f / 3.0f, 0.1f, 100.0f);
	glm::mat4 View = glm::lookAt(glm::vec3(4, 3, 3), glm::vec3(0, 0, 0), glm::vec3(0, 1, 0));

	glm::mat4 rotate_x = { 1.0f, 0.0f, 0.0f, 0.0f,
						   0.0f, glm::cos(angle_x), -glm::sin(angle_x), 0.0f,
						   0.0f, glm::sin(angle_x), glm::cos(angle_x), 0.0f,
						   0.0f, 0.0f, 0.0f, 1.0f };

	glm::mat4 rotate_y = { glm::cos(angle_x), 0.0f, glm::sin(angle_x), 0.0f,
					   0.0f, 1, 0, 0.0f,
					   -glm::sin(angle_x),0, glm::cos(angle_x), 0.0f,
					   0.0f, 0.0f, 0.0f, 1.0f };

	Matrix_projection = Projection * View * rotate_y * rotate_x;

	//! Устанавливаем шейдерную программу текущей 
	glUseProgram(Program);
	glUniformMatrix4fv(Unif_matrix, 1, GL_FALSE, &Matrix_projection[0][0]);

	// Position attribute
	glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 8 * sizeof(GLfloat), (GLvoid*)0);
	glEnableVertexAttribArray(0);
	// Color attribute
	glVertexAttribPointer(1, 3, GL_FLOAT, GL_FALSE, 8 * sizeof(GLfloat), (GLvoid*)(3 * sizeof(GLfloat)));
	glEnableVertexAttribArray(1);
	// TexCoord attribute
	glVertexAttribPointer(2, 2, GL_FLOAT, GL_FALSE, 8 * sizeof(GLfloat), (GLvoid*)(6 * sizeof(GLfloat)));
	glEnableVertexAttribArray(2);

	glBindVertexArray(0); // Unbind VAO

	// Bind Textures using texture units
	glActiveTexture(GL_TEXTURE0);
	glBindTexture(GL_TEXTURE_2D, texture1);
	glUniform1i(glGetUniformLocation(Program, "ourTexture1"), 0);

	glActiveTexture(GL_TEXTURE1);
	glBindTexture(GL_TEXTURE_2D, texture2);
	glUniform1i(glGetUniformLocation(Program, "ourTexture2"), 1);

	// Draw container
	glBindVertexArray(VAO);
	glDrawElements(GL_TRIANGLES, 6 * 6, GL_UNSIGNED_INT, 0);
	glBindVertexArray(0);
	/*glUniformMatrix4fv(Unif_matrix, 1, GL_FALSE, &Matrix_projection[0][0]);

	glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, VBO_element);

	//! Включаем массив атрибутов
	glEnableVertexAttribArray(Attrib_vertex);
	//! Подключаем VBO
	glBindBuffer(GL_ARRAY_BUFFER, VBO_vertex);
	//! Указывая pointer 0 при подключенном буфере, мы указываем что данные в 	VBO
	glVertexAttribPointer(Attrib_vertex, 3, GL_FLOAT, GL_FALSE, 0, 0);


	glEnableVertexAttribArray(Attrib_color);
	glBindBuffer(GL_ARRAY_BUFFER, VBO_color);
	glVertexAttribPointer(Attrib_color, 3, GL_FLOAT, GL_FALSE, 0, 0);

	//! Передаем данные на видеокарту(рисуем)
	glDrawElements(GL_TRIANGLES,      // mode
		Indices_count,    // count
		GL_UNSIGNED_INT,   // type
		0);

	//! Отключаем массив атрибутов
	glDisableVertexAttribArray(Attrib_vertex);

	//Отключаем массив атрибутов
	glDisableVertexAttribArray(Attrib_color);*/

	glFlush();

	checkOpenGLerror();

	glutSwapBuffers();

	//! Отключаем шейдерную программу  
	glUseProgram(0);
}


int main(int argc, char** argv)
{
	setlocale(0, "");
	glutInit(&argc, argv);
	glutInitDisplayMode(GLUT_DEPTH | GLUT_RGBA | GLUT_ALPHA | GLUT_DOUBLE);
	glutInitWindowSize(1000, 800);
	glutCreateWindow("Simple shaders");
	glEnable(GL_DEPTH_TEST);
	glDepthFunc(GL_LESS);

	//! Обязательно перед инициализацией шейдеров 
	GLenum glew_status = glewInit();
	if (GLEW_OK != glew_status)
	{
		//! GLEW не проинициализировалась  	 	
		std::cout << "Error: " << glewGetErrorString(glew_status) << "\n";
		return 1;
	}

	//! Проверяем доступность OpenGL 2.0  	
	if (!GLEW_VERSION_2_0)
	{
		//! OpenGl 2.0 оказалась не доступна  	 	
		std::cout << "No support for OpenGL 2.0 found\n";
		return 1;
	}

	//! Инициализация  
	glClearColor(0, 0, 0, 0);

	text();
	initVBO();
	initShader();
	glutReshapeFunc(resizeWindow);
	glutIdleFunc(render);
	glutDisplayFunc(render);
	glutMainLoop();

	//! Освобождение ресурсов  
	freeShader();
	freeVBO();
}

