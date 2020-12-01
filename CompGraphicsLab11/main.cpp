#include "GL/SOIL.h"
#include "GL/glew.h"
#include "GL/freeglut.h"
#include <iostream>
#include <vector>
#define _USE_MATH_DEFINES // for C++
#include <math.h>
using namespace std;

GLuint floorTexture, ballTexture, ballTexture2, ballTexture3, carTexture, windscreenTexture;

float rotateX = 0, rotateY = 0, rotateZ = 0;

double angle = 0.000001;

// для вращения ёлки
double angle_tree = 0;

// для гирлянды
float shine = 0.0;
float is_increase = true;

struct Car {
	//Центр машинки
	float positionX = 0;
	float positionZ = 0;
	bool enabledLight = false;
};

//объект машины, хранит ее позицию и состояние фар
Car car;

struct PointF {
	float x, y, z;

	PointF(float x, float y, float z) : x(x), y(y), z(z) { }
};

struct Light {
	PointF position;
	bool enabled = false;

	Light(float x, float y, float z) : position(PointF(x, y, z)) { }
};

//позиции фонарей
vector<Light> lights = {
	Light(-300.0, 150.0, -300.0),
	Light(-300.0, 150.0, 300.0),
	Light(300.0, 150.0, -300.0),
	Light(300.0, 150.0, 300.0)
};

struct Material
{
	GLfloat ambient[4] = { 0.2, 0.2, 0.2, 1.0 }; // рассеянный цвет материала(цвет материала в тени)
	GLfloat diffuse[4] = { 0.8, 0.8, 0.8, 1.0 }; // цвет диффузного отражения материала
	GLfloat specular[4] = { 0.0, 0.0, 0.0, 1.0 }; // цвет зеркального отражения материала
	GLfloat emission[4] = { 0.0, 0.0, 0.0, 1.0 }; // интенсивность излучаемого света материала
	float shininess = 0; // степень зеркального отражения материала (в диапазоне от 0 до 128)

	Material() {}

	void set_ambient(float R, float G, float B, float A)
	{
		ambient[0] = R;
		ambient[1] = G;
		ambient[2] = B;
		ambient[3] = A;
	}

	void set_diffuse(float R, float G, float B, float A)
	{
		diffuse[0] = R;
		diffuse[1] = G;
		diffuse[2] = B;
		diffuse[3] = A;
	}

	void set_specular(float R, float G, float B, float A)
	{
		specular[0] = R;
		specular[1] = G;
		specular[2] = B;
		specular[3] = A;
	}

	void set_emission(float R, float G, float B, float A)
	{
		emission[0] = R;
		emission[1] = G;
		emission[2] = B;
		emission[3] = A;
	}

	void set_shininess(float shine)
	{
		shininess = shine;
	}
};


//загрузить текстуры
void loadTextures()
{
	auto textureFlags = SOIL_FLAG_MIPMAPS | SOIL_FLAG_INVERT_Y | SOIL_FLAG_NTSC_SAFE_RGB | SOIL_FLAG_COMPRESS_TO_DXT;
	floorTexture = SOIL_load_OGL_texture("img/floor.jpg", SOIL_LOAD_AUTO, SOIL_CREATE_NEW_ID, textureFlags);
	ballTexture = SOIL_load_OGL_texture("img/ball.jpg", SOIL_LOAD_AUTO, SOIL_CREATE_NEW_ID, textureFlags);
	ballTexture2 = SOIL_load_OGL_texture("img/ball2.jpg", SOIL_LOAD_AUTO, SOIL_CREATE_NEW_ID, textureFlags);
	ballTexture3 = SOIL_load_OGL_texture("img/ball3.jpg", SOIL_LOAD_AUTO, SOIL_CREATE_NEW_ID, textureFlags);
	carTexture = SOIL_load_OGL_texture("img/car.jpg", SOIL_LOAD_AUTO, SOIL_CREATE_NEW_ID, textureFlags);
	windscreenTexture = SOIL_load_OGL_texture("img/window.jpg", SOIL_LOAD_AUTO, SOIL_CREATE_NEW_ID, textureFlags);
}

// Задать материал
void set_material(const Material& material)
{
	glMaterialfv(GL_FRONT, GL_AMBIENT, material.ambient);
	glMaterialfv(GL_FRONT, GL_DIFFUSE, material.diffuse);
	glMaterialfv(GL_FRONT, GL_SPECULAR, material.specular);
	glMaterialfv(GL_FRONT, GL_EMISSION, material.emission);
	glMaterialf(GL_FRONT, GL_SHININESS, material.shininess);
}

void drawRoad() {
	glBindTexture(GL_TEXTURE_2D, floorTexture);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST);
	glTexEnvf(GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, GL_MODULATE);

	glEnable(GL_TEXTURE_2D);

	glBegin(GL_QUADS);


		glTexCoord2f(0.0, 0.0); glVertex3f(-400.0 , 0.0, -400.0);
		glTexCoord2f(0.0, 1.0); glVertex3f(-400.0 , 0.0, 400.0);
		glTexCoord2f(1.0, 1.0); glVertex3f(400.0 , 0.0, 400.0);
		glTexCoord2f(1.0, 0.0); glVertex3f(400.0 , 0.0, -400.0 );

	glEnd();

	glDisable(GL_TEXTURE_2D);
}

void drawLights() {
	//Рисуем прожектор на камере
	/*GLfloat light0Pos[] = { 0, 400, 350, 1 };
	GLfloat light0Direction[] = { 0, -400, -350 };
	glLightfv(GL_LIGHT0, GL_POSITION, light0Pos);
	glLightfv(GL_LIGHT0, GL_SPOT_DIRECTION, light0Direction);
	glLightf(GL_LIGHT0, GL_SPOT_CUTOFF, 90);*/

	//Рисуем фонари
	for (int i = 0; i < lights.size(); i++) {

		PointF pos = lights[i].position;
		//GLfloat lightPos[] = { 0, 0, 0, 1 };
		//GLfloat lightColor[] = { 1, 1, 1, 1 };

		//Рисуем основания фонарей (цилиндры)
		//glColor3f(0.5, 0.5, 0.1 );
		glColor3f(0.396, 0.263, 0.129); // выбираем коричневый цвет
		glPushMatrix();
		glTranslatef(pos.x, pos.y, pos.z);
		glRotatef(90, 1.0, 0.0, 0.0);
		glutSolidCylinder(5, 150, 8, 8);
		glPopMatrix();

		//Рисуем лампы в виде сфер
		glColor3f(1, 1, 1);
		if (lights[i].enabled)
		glMaterialfv(GL_FRONT, GL_EMISSION, new GLfloat[4]{ 0.5, 0.5, 0.1, 1.0 });
		glPushMatrix();
		glTranslatef(pos.x, pos.y + 18, pos.z);
		glutSolidSphere(18, 12, 12);
		glPopMatrix();
		glMaterialfv(GL_FRONT, GL_EMISSION, new GLfloat[4]{ 0.0, 0.0, 0.0, 1.0 });

		//навешиваем свет
		glPushMatrix();
		//glLoadIdentity();
		//glTranslatef(pos.x, pos.y /*+ 25*/, pos.z);


		GLfloat noLightMaterial[] = { 0, 0, 0, 1 };
		GLfloat lightMaterial[] = { 0.7, 0.7, 0.7, 0 };
		GLfloat light3_specular[] = { 1,1,1 };
		GLfloat light3_diffuse[] = { 0.55, 0.48, 0.3 };
		GLfloat light3_position[] = { pos.x, pos.y, pos.z,  1.0 };
		GLfloat light3_spot_direction[] = { -pos.x, 100 - pos.y, -pos.z };
		//{-1, 0 , 0};

			//glEnable(GL_LIGHT1 + i);
		glLightfv(GL_LIGHT1 + i, GL_SPECULAR, light3_specular);
		glLightfv(GL_LIGHT1 + i, GL_DIFFUSE, light3_diffuse);
		glLightfv(GL_LIGHT1 + i, GL_POSITION, light3_position);
		glLightf(GL_LIGHT1 + i, GL_SPOT_CUTOFF, 60);
		glLightfv(GL_LIGHT1 + i, GL_SPOT_DIRECTION, light3_spot_direction);
		glLightf(GL_LIGHT1 + i, GL_SPOT_EXPONENT, 5.0);

		/*if (lights[i].enabled) glMaterialfv(GL_FRONT, GL_EMISSION, lightMaterial);
		else glMaterialfv(GL_FRONT, GL_EMISSION, noLightMaterial);*/


		glPopMatrix();
	}
}

// Конус для одного яруса ёлки
void drawConeTree(float yPos, float radius, float height)
{
	glPushMatrix();
	glTranslatef(0.0, yPos, 0.0);
	glRotatef(angle_tree, 0.0, 1.0, 0.0);
	glRotatef(-90, 1.0, 0.0, 0.0);
	glutSolidCone(radius, height, 15, 15);
	glPopMatrix();
}

// Рисование ёлки
void draw_fir_tree()
{
	angle_tree += 0.05f; // для поворота

	// Цилиндр (ствол)
	glPushMatrix();
	glRotatef(angle_tree, 0.0, 1.0, 0.0);
	glRotatef(-90, 1.0, 0.0, 0.0);
	glColor3f(0.396, 0.263, 0.129); // выбираем коричневый цвет
	glutSolidCylinder(15, 50, 12, 12);
	glPopMatrix();

	glColor3f(0.0, 1.0, 0.0); // выбираем зелёный цвет

	drawConeTree(50, 80, 120); 	// Конус (нижний ярус)
	drawConeTree(140, 60, 90);	// Конус (средний ярус)
	drawConeTree(210, 40, 70);	// Конус (верхний ярус)
}

// Один шарик на ёлку
void draw_ball(int size, float distanceX, float distanceZ, float height, GLuint texture = 0)
{

	glPushMatrix();

	glRotatef(angle_tree, 0.0, 1.0, 0.0);
	glTranslatef(distanceX, height, distanceZ);

	// Если текстура есть
	if (texture != 0)
	{
		GLUquadricObj* quadObj;
		quadObj = gluNewQuadric();
		glBindTexture(GL_TEXTURE_2D, texture);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST);
		glTexEnvf(GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, GL_MODULATE);

		glEnable(GL_TEXTURE_2D);
		gluQuadricTexture(quadObj, GL_TRUE);
		gluSphere(quadObj, size, 32, 32);
		glDisable(GL_TEXTURE_2D);
	}
	else // без текстуры
	{
		glutSolidSphere(size, 32, 32);
	}

	glPopMatrix();
}

// Рисование новогодних шариков на ёлку
void drawBalls()
{
	glColor3f(1, 1, 1);

	// Текстурированные шары
	// Шары на нижнем ярусе
	draw_ball(15, 0, 60, 90, ballTexture);
	draw_ball(15, 0, -60, 90, ballTexture);

	// Шары на среднем ярусе
	draw_ball(10, 40, 0, 175, ballTexture2);
	draw_ball(10, -40, 0, 175, ballTexture2);

	// Шары на верхнем ярусе
	draw_ball(7, 0, 25, 240, ballTexture3);
	draw_ball(7, 0, -25, 240, ballTexture3);

	Material m = Material();
	m.set_diffuse(1, 1, 1, 1);
	m.set_specular(1, 1, 1, 1.0);
	m.set_shininess(128);
	set_material(m);

	// Глянцевые шары
	glColor3f(0.0, 0.6, 0.6); // выбираем голубой цвет
    // Шары на нижнем ярусе
	draw_ball(15, 60, 0, 90);
	draw_ball(15, -60, 0, 90);
	
	// Шары на среднем ярусе
	draw_ball(10, 0, 40, 175);
	draw_ball(10, 0, -40, 175);
	
	// Шары на верхнем ярусе
	draw_ball(7, 25, 0, 240);
	draw_ball(7, -25, 0, 240);

	// Шар на верхушке ёлки (Жёлтый)
	glColor3f(1.0, 0.93, 0.0);
	draw_ball(15, 0, 0, 280, 0);

	set_material(Material());
}

// функции для вычисления координат шариков гирлянды
float getX(float radius, float angle)
{
	return radius * cos(angle*180.0/M_PI);
}

float getY(float radius, float angle)
{
	return radius * sin(angle * 180.0 / M_PI);
}

// вычисляет яркость для гирлянды
void compute_shine()
{
	if (shine >= 0.65)
		is_increase = false;
	if (shine <= 0.0)
		is_increase = true;
	if (is_increase)
		shine += 0.005;
	else shine -= 0.005;
}

// Рисуем гирлянду
void drawGarland()
{
	Material default_material = Material();

	Material garland = Material();
	garland.set_emission(shine, shine, shine, 1.0);
	garland.set_shininess(100);

	compute_shine();

	// Гирлянда
	set_material(garland);

	glColor3f(1.0, 0.0, 0.0); // устанавливаем красный цвет
	for (int i = 0; i <= 360; i += 10)
	{
		float x = getX(75, i);
		float z = getY(75, i);
		draw_ball(3, x, z, 62);

		x = getX(60, i);
		z = getY(60, i);
		draw_ball(3, x, z, 145);

		x = getX(38, i);
		z = getY(38, i);
		draw_ball(3, x, z, 220);
	}
	set_material(default_material);
}

void drawCar() {
	int carLength = 80, carHeight = 60, wheelRadius = 7;

	//колеса
	glColor3f(0.0, 0.0, 0.0);

	glPushMatrix();
	glTranslatef(car.positionX + wheelRadius - carLength / 2, wheelRadius + 1, car.positionZ + carHeight / 2);
	glutSolidTorus(3, wheelRadius, 54, 54);
	glPopMatrix();

	glPushMatrix();
	glTranslatef(car.positionX + carLength / 2 - wheelRadius, wheelRadius + 1, car.positionZ + carHeight / 2);
	glutSolidTorus(3, wheelRadius, 54, 54);
	glPopMatrix();

	glPushMatrix();
	glTranslatef(car.positionX + wheelRadius - carLength / 2, wheelRadius + 1, car.positionZ - carHeight / 2);
	glutSolidTorus(3, wheelRadius, 54, 54);
	glPopMatrix();

	glPushMatrix();
	glTranslatef(car.positionX + carLength / 2 - wheelRadius, wheelRadius + 1, car.positionZ - carHeight / 2);
	glutSolidTorus(3, wheelRadius, 54, 54);
	glPopMatrix();

	//активируем текстуру машины
	glBindTexture(GL_TEXTURE_2D, carTexture);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST);
	glEnable(GL_TEXTURE_2D);

	//нижняя часть автомобиля
	int clearance = 12;

	glColor3f(0.40, 0.35, 0.35);

	glBegin(GL_QUAD_STRIP);
	glVertex3f(car.positionX - carLength / 2, 40.0, car.positionZ - carHeight / 2); glTexCoord2f(0, 0);
	glVertex3f(car.positionX + carLength / 2, 40.0, car.positionZ - carHeight / 2); glTexCoord2f(1, 0);

	glVertex3f(car.positionX - carLength / 2, 40.0, car.positionZ + carHeight / 2); glTexCoord2f(0, 1);
	glVertex3f(car.positionX + carLength / 2, 40.0, car.positionZ + carHeight / 2); glTexCoord2f(1, 1);

	glVertex3f(car.positionX - carLength / 2, clearance, car.positionZ + carHeight / 2); glTexCoord2f(0, 0);
	glVertex3f(car.positionX + carLength / 2, clearance, car.positionZ + carHeight / 2); glTexCoord2f(1, 0);

	glVertex3f(car.positionX - carLength / 2, clearance, car.positionZ - carHeight / 2);  glTexCoord2f(1, 1);
	glVertex3f(car.positionX + carLength / 2, clearance, car.positionZ - carHeight / 2);  glTexCoord2f(0, 1);

	glVertex3f(car.positionX - carLength / 2, 40.0, car.positionZ - carHeight / 2);  glTexCoord2f(0, 0);
	glVertex3f(car.positionX + carLength / 2, 40.0, car.positionZ - carHeight / 2);  glTexCoord2f(1, 0);
	glEnd();

	//торец сзади
	glBegin(GL_QUADS);
	glVertex3f(car.positionX - carLength / 2, 40.0, car.positionZ - carHeight / 2);  glTexCoord2f(0, 1);
	glVertex3f(car.positionX - carLength / 2, 40.0, car.positionZ + carHeight / 2);  glTexCoord2f(1, 1);
	glVertex3f(car.positionX - carLength / 2, clearance, car.positionZ + carHeight / 2);  glTexCoord2f(1, 0);
	glVertex3f(car.positionX - carLength / 2, clearance, car.positionZ - carHeight / 2);  glTexCoord2f(0, 0);
	glEnd();

	//торец спереди
	glBegin(GL_QUADS);
	glVertex3f(car.positionX + carLength / 2, 40.0, car.positionZ - carHeight / 2); glTexCoord2f(0, 1);
	glVertex3f(car.positionX + carLength / 2, 40.0, car.positionZ + carHeight / 2); glTexCoord2f(1, 1);
	glVertex3f(car.positionX + carLength / 2, clearance, car.positionZ + carHeight / 2); glTexCoord2f(1, 0);
	glVertex3f(car.positionX + carLength / 2, clearance, car.positionZ - carHeight / 2); glTexCoord2f(0, 0);
	glEnd();


	//кабина
	glBegin(GL_QUAD_STRIP);
	glVertex3f(car.positionX + carLength / 2 - 30, 70.0, car.positionZ - carHeight / 2); glTexCoord2f(0, 0);
	glVertex3f(car.positionX + carLength / 2, 70.0, car.positionZ - carHeight / 2); glTexCoord2f(1, 0);

	glVertex3f(car.positionX + carLength / 2 - 30, 70.0, car.positionZ + carHeight / 2); glTexCoord2f(1, 1);
	glVertex3f(car.positionX + carLength / 2, 70.0, car.positionZ + carHeight / 2); glTexCoord2f(0, 1);

	glVertex3f(car.positionX + carLength / 2 - 30, 40.0, car.positionZ + carHeight / 2); glTexCoord2f(0, 0);
	glVertex3f(car.positionX + carLength / 2, 40.0, car.positionZ + carHeight / 2); glTexCoord2f(1, 0);

	glVertex3f(car.positionX + carLength / 2 - 30, 40.0, car.positionZ - carHeight / 2); glTexCoord2f(0, 1);
	glVertex3f(car.positionX + carLength / 2, 40.0, car.positionZ - carHeight / 2); glTexCoord2f(1, 1);

	glVertex3f(car.positionX + carLength / 2 - 30, 70.0, car.positionZ - carHeight / 2); glTexCoord2f(1, 0);
	glVertex3f(car.positionX + carLength / 2, 70.0, car.positionZ - carHeight / 2); glTexCoord2f(0, 0);
	glEnd();

	//торец кабины
	glBegin(GL_QUADS);
	glVertex3f(car.positionX + carLength / 2 - 30, 70.0, car.positionZ - carHeight / 2); glTexCoord2f(0, 0);
	glVertex3f(car.positionX + carLength / 2 - 30, 70.0, car.positionZ + carHeight / 2); glTexCoord2f(1, 0);
	glVertex3f(car.positionX + carLength / 2 - 30, 40.0, car.positionZ + carHeight / 2); glTexCoord2f(1, 1);
	glVertex3f(car.positionX + carLength / 2 - 30, 40.0, car.positionZ - carHeight / 2); glTexCoord2f(0, 1);
	glEnd();

	glDisable(GL_TEXTURE_2D);


	//активируем текстуру лобового стекла
	glBindTexture(GL_TEXTURE_2D, windscreenTexture);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST);
	glEnable(GL_TEXTURE_2D);

	//лобовое стекло
	glColor3f(0.55, 0.9, 0.9);

	glBegin(GL_QUADS);
	glVertex3f(car.positionX + carLength / 2, 70.0, car.positionZ - carHeight / 2); glTexCoord2f(0, 0);
	glVertex3f(car.positionX + carLength / 2, 70.0, car.positionZ + carHeight / 2); glTexCoord2f(1, 0);
	glVertex3f(car.positionX + carLength / 2, 40.0, car.positionZ + carHeight / 2); glTexCoord2f(1, 1);
	glVertex3f(car.positionX + carLength / 2, 40.0, car.positionZ - carHeight / 2); glTexCoord2f(0, 1);
	glEnd();

	glDisable(GL_TEXTURE_2D);

	GLfloat lightsColor[] = { 0.7, 0.7, 0.2, 1.0 };
	GLfloat lightsPosition[] = { 0, 0, 0, 1 };
	GLfloat lightsDirection[] = { 1, -0.1, 0 };

	GLfloat noLightMaterial[] = { 0, 0, 0, 1 };
	GLfloat lightMaterial[] = { 0.7, 0.7, 0.7, 0 };

	//фара 1
	glPushMatrix();
	glTranslatef(car.positionX + carLength / 2 + 5, 35, car.positionZ - carHeight / 2 + 10);

	glLightfv(GL_LIGHT6, GL_POSITION, lightsPosition);
	glLightfv(GL_LIGHT6, GL_SPOT_DIRECTION, lightsDirection);
	glLightf(GL_LIGHT6, GL_SPOT_CUTOFF, 30);
	glLightf(GL_LIGHT6, GL_SPOT_EXPONENT, 5);
	glLightfv(GL_LIGHT6, GL_DIFFUSE, lightsColor);

	if (car.enabledLight) glMaterialfv(GL_FRONT, GL_EMISSION, lightMaterial);
	else glMaterialfv(GL_FRONT, GL_EMISSION, noLightMaterial);

	glColor3f(0.7, 0.7, 0.2);
	glutSolidSphere(5, 48, 48);
	glPopMatrix();

	//фара 2
	glPushMatrix();
	glTranslatef(car.positionX + carLength / 2 + 5, 35, car.positionZ + carHeight / 2 - 10);

	glLightfv(GL_LIGHT7, GL_POSITION, lightsPosition);
	glLightfv(GL_LIGHT7, GL_SPOT_DIRECTION, lightsDirection);
	glLightf(GL_LIGHT7, GL_SPOT_CUTOFF, 30);
	glLightf(GL_LIGHT7, GL_SPOT_EXPONENT, 5);
	glLightfv(GL_LIGHT7, GL_DIFFUSE, lightsColor);

	if (car.enabledLight) glMaterialfv(GL_FRONT, GL_EMISSION, lightMaterial);
	else glMaterialfv(GL_FRONT, GL_EMISSION, noLightMaterial);

	glColor3f(0.7, 0.7, 0.2);
	glutSolidSphere(5, 48, 48);
	glPopMatrix();

	glMaterialfv(GL_FRONT, GL_EMISSION, noLightMaterial);
	glPopMatrix();
	glColor3f(1, 1, 1);
}

//Задать начальные параметры OpenGL
void init() {
	//	glClearColor(0.3f, 0.5f, 0.5f, 1.0f);
	glClearColor(0.2f, 0.4f, 0.5f, 1.0f);
	glLoadIdentity();
	loadTextures();
	glLightModelf(GL_LIGHT_MODEL_TWO_SIDE, GL_TRUE);
	glEnable(GL_NORMALIZE);
	glEnable(GL_COLOR_MATERIAL);

	car.positionZ = 150;
}

//функция, вызываемая при изменении размера окна
void reshape(int width, int height) {
	glViewport(0, 0, width, height);
	glMatrixMode(GL_PROJECTION);
	glLoadIdentity();
	gluPerspective(65.0f, width * 1.0f / height, 1.0f, 2000.0f);
	glMatrixMode(GL_MODELVIEW);
	glLoadIdentity();
}


//отрисовка
void render()
{
	angle += 0.2;
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
	glLoadIdentity();
	gluLookAt(0.0f, 400.0f, 600.0f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f); //местоположение наблюдателя и вектор наблюдения
	glRotatef(rotateX, 1.0, 0.0, 0.0);
	glRotatef(rotateY, 0.0, 1.0, 0.0);
	glRotatef(rotateZ, 0.0, 0.0, 1.0);

	glEnable(GL_DEPTH_TEST);
	glEnable(GL_LIGHTING);

	glLightf(GL_LIGHT0, GL_CONSTANT_ATTENUATION, 2);
	//glLightf(GL_LIGHT0, GL_QUADRATIC_ATTENUATION, 2);
	glLightfv(GL_LIGHT0, GL_POSITION, new GLfloat[4]{0, 600, 750, 1.0});
	//glEnable(GL_LIGHT0);
	//активируем включенные фонари как источники света
	for (int i = 0; i < lights.size(); i++)
	{
		if (lights[i].enabled)
			glEnable(GL_LIGHT1 + i);
	}

	//активируем фары
	if (car.enabledLight) {
		glEnable(GL_LIGHT6);
		glEnable(GL_LIGHT7);
	}


	drawRoad(); //рисуем дорогу
	drawCar(); //рисуем машину
	drawLights(); //рисуем фонари 
	draw_fir_tree();
	drawBalls();
	drawGarland();
	glColor3f(1, 1, 1);
	//glColor3f(0.396, 0.263, 0.129); // Цвет брусчатки

	//деактивируем фары
	if (car.enabledLight) {
		glDisable(GL_LIGHT6);
		glDisable(GL_LIGHT7);
	}

	//деактивируем включенные фонари
	for (int i = 0; i < lights.size(); i++)
	{
		if (lights[i].enabled)
			glDisable(GL_LIGHT1 + i);
	}

	//деактивируем прожектор на камере
	glDisable(GL_LIGHT0);

	glDisable(GL_LIGHTING);
	glDisable(GL_DEPTH_TEST);
	glFlush();
	glutSwapBuffers();
}

void keyboardCallback(unsigned char key, int x, int y) {
	switch (key) {
	case '1':
		if (lights.size() > 0) lights[0].enabled = !lights[0].enabled;
		break;
	case '2':
		if (lights.size() > 1) lights[1].enabled = !lights[1].enabled;
		break;
	case '3':
		if (lights.size() > 2) lights[2].enabled = !lights[2].enabled;
		break;
	case '4':
		if (lights.size() > 3) lights[3].enabled = !lights[3].enabled;
		break;
	case 'W':
	case 'w':
		car.positionX += 2;
		break;
	case 'S':
	case 's':
		car.positionX -= 2;
		break;
	case '0':
		car.enabledLight = !car.enabledLight;
	}
	glutPostRedisplay();
}

int main(int argc, char* argv[]) {
	//инициализация GLUT
	glutInit(&argc, argv);
	glutInitWindowPosition(50, 50);
	glutInitWindowSize(1280, 720);
	glutCreateWindow("Tree");
	glutInitDisplayMode(GLUT_RGBA | GLUT_DOUBLE | GLUT_DEPTH);

	init();

	glutReshapeFunc(reshape);
	glutIdleFunc(render);
	glutDisplayFunc(render);
	glutKeyboardFunc(keyboardCallback);

	glutMainLoop();
}
