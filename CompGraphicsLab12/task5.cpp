 #include <GL/glew.h>
#include <gl/GL.h>   // GL.h header file    
#include <gl/GLU.h> // GLU.h header file     
#include <gl/freeglut.h>
#include <glm/trigonometric.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <iostream>
using namespace std;

GLint Program;
//ID атрибута вершин 
GLint Attrib_vertex;

//ID атрибута цветов 
GLint Attrib_color;

//ID Vertex Buffer Object 
GLuint VBO_vertex;

//ID Vertex Buffer Object 
GLuint VBO_color;

//ID VBO for element indices 
GLuint VBO_element;

//Количество индексов 
GLint Indices_count;

GLint Unif_matrix;

glm::mat4 Matrix_projection; // = glm::perspective(45.0f, 4/3.0f, 1.0f, 200.0f);;

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
		"attribute vec3 coord;\n"
		"attribute vec3 color;\n"
		"out vec3 var_color;\n"
		"uniform mat4 matrix;\n"
		"void main() {\n"
		"gl_Position = matrix * vec4(coord , 1.0);\n"
		"var_color = color;\n"
		"}\n";
	const char* fsSource =
		"in vec3 var_color;\n"
		"void main() {\n"
		"gl_FragColor = vec4(var_color , 1.0);\n"
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
	const char* attr_name = "coord";
	Attrib_vertex = glGetAttribLocation(Program, attr_name);
	if (Attrib_vertex == -1)
	{
		std::cout << "could not bind attrib " << attr_name << std::endl;
		return;
	}

	///! Вытягиваем ID атрибута из собранной программы   	
	attr_name = "color";
	Attrib_color = glGetAttribLocation(Program, attr_name);
	if (Attrib_color == -1)
	{
		std::cout << "could not bind attrib " << attr_name << std::endl;
		return;
	}

	attr_name = "matrix";
	Unif_matrix = glGetUniformLocation(Program, attr_name);

	checkOpenGLerror();
}
//! Инициализация VBO 
void initVBO()
{
	GLfloat vertices[] =
	{
		-1.0f , -1.0f , -1.0f ,
		1.0f , -1.0f , -1.0f ,
		1.0f , 1.0f , -1.0f ,
		-1.0f , 1.0f , -1.0f ,
		-1.0f , -1.0f , 1.0f ,
		1.0f , -1.0f , 1.0f ,
		1.0f , 1.0f , 1.0f ,
		-1.0f , 1.0f , 1.0f
	};

	GLfloat colors[] = {
	  1.0f , 0.5f , 1.0f ,
	  1.0f , 0.5f , 0.5f ,
	  0.5f , 0.5f , 1.0f ,
	  0.0f , 1.0f , 1.0f ,
	  1.0f , 0.0f , 1.0f ,
	  1.0f , 1.0f , 0.0f ,
	  1.0f , 0.0f , 1.0f ,
	  0.0f , 1.0f , 1.0f
	};


	GLint indices[] = {
0, 4, 5, 
0, 5, 1,
1, 5, 6,
1, 6, 2,
2, 6, 7,
2, 7, 3,
3, 7, 4, 
3, 4, 0,
4, 7, 6, 
4, 6, 5,
3, 0, 1, 
3, 1, 2
	};
	Indices_count = sizeof(indices) / sizeof(indices[0]);

	//! Передаем вершины в буфер
	glGenBuffers(1, &VBO_vertex);
	glBindBuffer(GL_ARRAY_BUFFER, VBO_vertex); //! Вершины нашего куба 
	glBufferData(GL_ARRAY_BUFFER, sizeof(vertices), vertices, GL_STATIC_DRAW);

	glGenBuffers(1, &VBO_color);
	glBindBuffer(GL_ARRAY_BUFFER, VBO_color);
	glBufferData(GL_ARRAY_BUFFER, sizeof(colors), colors, GL_STATIC_DRAW);

	glGenBuffers(1, &VBO_element);
	glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, VBO_element);
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
	glDeleteBuffers(1, &VBO_vertex);
	glDeleteBuffers(1, &VBO_element);
	glDeleteBuffers(1, &VBO_color);

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

	//glm::mat4 Model = glm::mat4(1.0f);
	Matrix_projection = Projection * View/* * Model */* rotate_x;

	//! Устанавливаем шейдерную программу текущей 
	glUseProgram(Program);
	glUniformMatrix4fv(Unif_matrix, 1, GL_FALSE, &Matrix_projection[0][0]);

	glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, VBO_element);

	/*static float red[4] = { 1.0f, 0.0f, 0.0f, 1.0f };
	//! Передаем юниформ в шейдер
	glUniform4fv(Unif_color, 1, red);*/

	//! Включаем массив атрибутов 
	glEnableVertexAttribArray(Attrib_vertex);
	//! Подключаем VBO  	
	glBindBuffer(GL_ARRAY_BUFFER, VBO_vertex);
	//! Указывая pointer 0 при подключенном буфере, мы указываем что данные в 	VBO
	glVertexAttribPointer(Attrib_vertex, 3 , GL_FLOAT, GL_FALSE, 0, 0);

	//! Включаем массив атрибутов 
	//glEnableVertexAttribArray(1);
	//! Подключаем VBO  	

	//! Указывая pointer 0 при подключенном буфере, мы указываем что данные в 	VBO
	//glVertexAttribPointer(1, 3 /*2*/, GL_FLOAT, GL_FALSE, 0, 0);
	glEnableVertexAttribArray(Attrib_color);
	glBindBuffer(GL_ARRAY_BUFFER, VBO_color);
	glVertexAttribPointer(Attrib_color, 3, GL_FLOAT, GL_FALSE, 0, 0);

	/*//! Включаем массив атрибутов
	glEnableVertexAttribArray(2);
	//! Подключаем VBO
	glBindBuffer(GL_ARRAY_BUFFER, colorbuffer);
	//! Указывая pointer 0 при подключенном буфере, мы указываем что данные в 	VBO
	glVertexAttribPointer(2, 3 , GL_FLOAT, GL_FALSE, 0, 0);*/





	//! Передаем данные на видеокарту(рисуем)  	
	//glDrawArrays(GL_TRIANGLES, 0, 12 * 4);
	glDrawElements(GL_TRIANGLES,      // mode
		Indices_count,    // count
		GL_UNSIGNED_INT,   // type
		0);
	//! Отключаем VBO  	
	//glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);
	//! Отключаем массив атрибутов 
	glDisableVertexAttribArray(Attrib_vertex);

	//Отключаем массив атрибутов 
	glDisableVertexAttribArray(Attrib_color);

	//glFlush();


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

