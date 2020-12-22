#include <gl/SOIL.h>
#include <gl/glew.h> 
#include <gl/GL.h>
#include <gl/GLU.h>
#include <gl/freeglut.h>
#include <glm/trigonometric.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <iostream>
#include <fstream>
#include <vector>
#include <tuple>
#include <regex>

using namespace std;

class Point3D
{
public:
    float x;
    float y;
    float z;
	Point3D(float x, float y, float z)
    {
        this->x = x;
        this->y = y;
        this->z = z;
    }
    Point3D()
    {
        x = 0;
        y = 0;
        z = 0;
    }
};

class Point2D
{
public:
    float x;
    float y;
	Point2D(float x, float y)
    {
        this->x = x;
        this->y = y;
    }
    Point2D()
    {
        x = 0;
        y = 0;
    }
};

class Mesh
{
public:
    vector<Point3D> pointList;
    vector<Point3D> normalList;
    vector<Point2D> texturePoint;
    vector<vector<tuple<int, int, int>>> polygons;
    vector<int> indicesList;
	Mesh()
    {
        this->normalList = vector<Point3D>();
        this->pointList = vector<Point3D>();
        this->texturePoint = vector<Point2D>();
        this->polygons = vector<vector<tuple<int, int, int>>>();
        this->indicesList = vector<int>();
    }
	Mesh(vector<Point3D> points)
    {
        this->pointList = vector<Point3D>();

        for (int i = 0; i < points.size(); i++)
        {
            this->pointList.push_back(points[i]);
        }
    }
};

class GLShader
{
private:
    GLuint vertex_shader;
    GLuint fragment_shader;

    GLuint compileSource(const char* source, GLuint shader_type)
    {
        GLuint shader = glCreateShader(shader_type);
        glShaderSource(shader, 1, &source, NULL);
        glCompileShader(shader);
        return shader;
    }

    void linkProgram()
    {
        ShaderProgram = glCreateProgram();
        glAttachShader(ShaderProgram, vertex_shader);
        glAttachShader(ShaderProgram, fragment_shader);
        glLinkProgram(ShaderProgram);
    }

public:
    GLuint ShaderProgram;

    GLShader() :ShaderProgram(0) {}
    ~GLShader()
    {
        glUseProgram(0);
        glDeleteShader(vertex_shader);
        glDeleteShader(fragment_shader);
        glDeleteProgram(ShaderProgram);
    }

    void load(const char* vertext_src, const char* fragment_src)
    {
        vertex_shader = compileSource(vertext_src, GL_VERTEX_SHADER);
        fragment_shader = compileSource(fragment_src, GL_FRAGMENT_SHADER);
        linkProgram();
    }

    void load_vertex_shader(const char* vertext_src, const char* fragment_src)
    {
        vertex_shader = compileSource(vertext_src, GL_VERTEX_SHADER);
        fragment_shader = compileSource(fragment_src, GL_FRAGMENT_SHADER);
        linkProgram();
    }

    GLuint getIDProgram()
    {
        return ShaderProgram;
    }

    GLint getAttribLocation(const char* name)const
    {
        GLint t = glGetAttribLocation(ShaderProgram, name);
        if (t == -1)
        {
            std::cout << "could not bind attrib " << name << std::endl;
            return -1;
        }
        return t;
    }
    GLuint getUniformLocation(const char* name)const
    {
        GLint t = glGetUniformLocation(ShaderProgram, name);
        if (t == -1)
        {
            std::cout << "could not bind uniform " << name << std::endl;
            return -1;
        }
        return t;
    }
    void use() { glUseProgram(ShaderProgram); }
};
