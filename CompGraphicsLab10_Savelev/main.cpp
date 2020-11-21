#include <iostream>
#define GL_SILENCE_DEPRECATION
#include <GL/freeglut.h>
//#include <GLUT/glut.h> //Apple's GLUT

#include "figures.h"

typedef void(*Function) ();

int n = 6;
const int primitivesCount = 9;

const Function primitives[primitivesCount] =
        {
                teapot,
                cube,
                sphere,
                torus,
                icosahedron,
                octehedron,

                rect,
                triangle2,
                triangle
        };


double rotate_x =0;
double rotate_y =0;
double rotate_z =0;

float r = 0, g = 0, b = 0;

void setRandomColor()
{
    r = (rand() % 255) / 255.0;
    g = (rand() % 255) / 255.0;
    b = (rand() % 255) / 255.0;
}

void render()
{
    glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
    glClearColor(0.23, 0.23, 0.23, 1.0);

    glRotatef(rotate_x, 1, 0, 0);
    glRotatef(rotate_y, 0, 1, 0);
    glRotatef(rotate_z, 0, 0, 1);

    glColor3f(r, g, b);
    primitives[n]();

    glLoadIdentity();
    glutSwapBuffers();
}

void mouseHandler(int button, int state, int x, int y)
{
    if (button == GLUT_LEFT_BUTTON && state == GLUT_DOWN)
    {
        setRandomColor();
        n = rand() % primitivesCount;
    }
}

void keyHandler(int key, int x, int y)
{
    switch (key)
    {
        case GLUT_KEY_UP: rotate_x += 5; break;
        case GLUT_KEY_DOWN: rotate_x -= 5; break;
        case GLUT_KEY_LEFT: rotate_y += 5; break;
        case GLUT_KEY_RIGHT: rotate_y -= 5; break;
        case GLUT_KEY_PAGE_UP: rotate_z += 5; break;
        case GLUT_KEY_PAGE_DOWN: rotate_z -= 5; break;
        case GLUT_KEY_END: n++; break;
        case GLUT_KEY_HOME: n--; break;
        default:
            break;
    }
    glutPostRedisplay();
}

//Глобальные статические переменные -
//хранят текущий размер экрана
static int w = 0 , h = 0 ;
//Функция вызываемая перед вхождением в главный цикл
void Init () {
    glClearColor( 0.0f,0.0f,0.0f,1.0f) ;
}
//Функция вызываемая каждый кадр -
// для его отрисовки, вычислений и т. д.
void Update () {
    glClear(GL_COLOR_BUFFER_BIT) ;
    render();
    glutSwapBuffers() ;
}
//Функця вызываемая при изменении размеров окна
void Reshape(int width , int height) {
w = width ; h = height ;
}


int main(int argc, char* argv [])
{
    //Инициализировать сам glut
    glutInit(&argc, argv);
    //Установить начальное положение окна
    glutInitWindowPosition(100,100);
    //Установить начальные размеры окна
    glutInitWindowSize(800,600);
    //Установить параметры окна - двойная буфферизация
    // и поддержка цвета RGBA
    glutInitDisplayMode(GLUT_DEPTH | GLUT_DOUBLE | GLUT_RGBA);

    /*glutInitContextVersion ( 4, 1);
    glutInitContextFlags   ( GLUT_FORWARD_COMPATIBLE | GLUT_DEBUG );
    glutInitContextProfile ( GLUT_CORE_PROFILE );*/

    //Создать окно с заголовком OpenGL
    glutCreateWindow("OpenGL");

    glEnable(GL_DEPTH_TEST);

    //Укажем glut функцию, которая будет вызываться каждый кадр
    //glutIdleFunc(render);
    //Укажем glut функцию, которая будет рисовать каждый кадр
    glutDisplayFunc( render );
    //Укажем glut функцию, которая будет вызываться при
    // изменении размера окна приложения
    //glutReshapeFunc(Reshape);
    Init() ;

    printf("GL_VERSION = %s\n",glGetString(GL_VERSION) );

    glutMouseFunc(mouseHandler);
    glutSpecialFunc(keyHandler);
    //Войти в главный цикл приложения
    glutMainLoop();
    return 0;
}
