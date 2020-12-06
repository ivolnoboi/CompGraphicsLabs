#include "GL\glew.h" 
#include "GL\freeglut.h"
#include <iostream>
//! Переменные с индентификаторами ID 
//! //! ID шейдерной программы
GLuint Program;
//! ID атрибута
GLint  Attrib_vertex;
//! ID юниформ переменной цвета 
GLint Unif_color;
//! Проверка ошибок OpenGL, если есть то вывод в консоль тип ошибки 
void checkOpenGLerror() {

	GLenum errCode;
	if ((errCode = glGetError()) != GL_NO_ERROR)
		std::cout << "OpenGl error! - " << gluErrorString(errCode) << "\n";
}
//! Инициализация шейдеров 
void initShader()
{
	//! Исходный код шейдеров 
	const char* vsSource =
		"attribute vec2 coord;\n"
		"mat2 rot(in float a) {return mat2(cos(a), sin(a), -sin(a), cos(a));}\n"
		"void main() {\n"
		"vec2 pos = rot(3.14*0.25)*coord;\n"
		"  gl_Position = vec4(pos, 0.0, 1.0);\n"
		"}\n";
	const char* fsSource =
		"uniform vec4 color;\n"
		"void main() {\n"
		"  gl_FragColor = color;\n"
		"}\n";
	//! Переменные для хранения идентификаторов шейдеров 
	GLuint vShader, fShader;
	//! Создаем вершинный шейдер
	vShader = glCreateShader(GL_VERTEX_SHADER);
	//! Передаем исходный код 
	glShaderSource(vShader, 1, &vsSource, NULL);
	//! Компилируем шейдер 
	glCompileShader(vShader);
	//! Создаем фрагментный шейдер
	fShader = glCreateShader(GL_FRAGMENT_SHADER);
	//! Передаем исходный код 
	glShaderSource(fShader, 1, &fsSource, NULL);
	//! Компилируем шейдер 
	glCompileShader(fShader);
	//! Создаем программу и прикрепляем шейдеры к ней 
	Program = glCreateProgram();
	glAttachShader(Program, vShader);
	glAttachShader(Program, fShader);
	//! Линкуем шейдерную программу 
	glLinkProgram(Program);
	//! Проверяем статус сборки
	int link_ok;
	glGetProgramiv(Program, GL_LINK_STATUS, &link_ok); if (!link_ok)
	{
		std::cout << "error attach shaders \n";
		return;
	}

	///! Вытягиваем ID атрибута из собранной программы
	const char* attr_name = "coord";
	Attrib_vertex = glGetAttribLocation(Program, attr_name);
	if (Attrib_vertex == -1)
	{
		std::cout << "could not bind attrib " << attr_name << std::endl; return;
	}

	//! Вытягиваем ID юниформ
	const char* unif_name = "color";
	Unif_color = glGetUniformLocation(Program, unif_name); if (Unif_color == -1)
	{
		std::cout << "could not bind uniform " << unif_name << std::endl; return;
	}
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
void resizeWindow(int width, int height) {
	glViewport(0, 0, width, height);
}
//! Отрисовка 
void render1() {
	glClear(GL_COLOR_BUFFER_BIT);
	//! Устанавливаем шейдерную программу текущей 
	glUseProgram(Program);
	static float red[4] = { 1.0f, 0.0f, 0.0f, 1.0f };
	//! Передаем юниформ в шейдер 
	glUniform4fv(Unif_color, 1, red);
	glBegin(GL_QUADS);
	glVertex2f(-0.5f, -0.5f);
	glVertex2f(-0.5f, 0.5f);
	glVertex2f(0.5f, 0.5f);
	glVertex2f(0.5f, -0.5f);
	glEnd();
	glFlush();
	//! Отключаем шейдерную программу 
	glUseProgram(0);
	checkOpenGLerror();
	glutSwapBuffers();
}

int main(int argc, char** argv)
{
	glutInit(&argc, argv);
	glutInitDisplayMode(GLUT_DEPTH | GLUT_RGBA | GLUT_ALPHA | GLUT_DOUBLE);
	glutInitWindowSize(600, 600);
	glutCreateWindow("Simple shaders");
	glClearColor(0, 0, 1, 0);
	//! Обязательно перед инициализацией шейдеров 
	GLenum glew_status = glewInit();
	if (GLEW_OK != glew_status)
	{
		//! GLEW не проинициализировалась
		std::cout << "Error: " << glewGetErrorString(glew_status) << "\n"; return 1;
	}
	//! Проверяем доступность OpenGL 2.0 
	if (!GLEW_VERSION_2_0)
	{
		//! OpenGl 2.0 оказалась не доступна
		std::cout << "No support for OpenGL 2.0 found\n"; return 1;
	}
	//! Инициализация шейдеров 
	initShader();
	glutReshapeFunc(resizeWindow);
	glutDisplayFunc(render1);
	glutMainLoop();
	//! Освобождение ресурсов 
	freeShader();
}