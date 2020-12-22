#include <GL/glew.h>
#include <gl/GL.h>   // GL.h header file    
#include <gl/GLU.h> // GLU.h header file     
#include <gl/freeglut.h>
#include <gl/glaux.h>
#include <glm/trigonometric.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <iostream>
#include "GL/SOIL.h"
#include <assimp/Importer.hpp>
#include <assimp/scene.h>
#include <assimp/postprocess.h>
#include <vector>
#include "GLShader.h"

using namespace std;

GLShader glShader;

GLint Unif_matrix;

glm::mat4 Matrix_projection;

GLuint VBO, VAO, EBO;

struct Vertex
{
	glm::vec3 Position; // �������
	glm::vec3 Normal; // �������
	glm::vec2 TexCoords; // ���������� ����������
	glm::vec3 Color;
};

struct Texture
{
	unsigned int id;
	string type;
	string path;
};

class Mesh
{
public:
	// ������ ����
	vector<Vertex> vertices;
	vector<unsigned int> indices;
	//unsigned int VAO;

	// �����������
	Mesh(vector<Vertex> vertices, vector<unsigned int> indices)
	{
		this->vertices = vertices;
		this->indices = indices;

		// ������, ����� � ��� ���� ��� ����������� ������, ������������� ��������� ������ � ��������� ���������
		setupMesh();
	}

private:
	// ������ ��� ���������� 
	//unsigned int VBO, EBO;

	// �������������� ��� �������� �������/�������
	void setupMesh()
	{
		// ������� �������� �������/�������
		glGenVertexArrays(1, &VAO);
		glGenBuffers(1, &VBO);
		glGenBuffers(1, &EBO);

		glBindVertexArray(VAO);

		// ��������� ������ � ��������� �����
		glBindBuffer(GL_ARRAY_BUFFER, VBO);

		// ����� ������������� � ���������� ��, ��� ������������ � ������ �� ���������� ���������� �������� ����������������.
		// ����� ������� ����� � ���, ��� �� ����� ������ �������� ��������� �� ���������, � ��� ��������� ������������� � ������ ������ � ���������� ���� glm::vec3 (��� glm::vec2), ������� ����� ����� ������������ � ������ ������ float, �� � � ����� � � �������� ������
		glBufferData(GL_ARRAY_BUFFER, vertices.size() * sizeof(Vertex), &vertices[0], GL_STATIC_DRAW);

		glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, EBO);
		glBufferData(GL_ELEMENT_ARRAY_BUFFER, indices.size() * sizeof(unsigned int), &indices[0], GL_STATIC_DRAW);

		// ������������� ��������� ��������� ���������

		// ���������� ������
		glEnableVertexAttribArray(0);
		glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, sizeof(Vertex), (void*)0);

		// ������� ������
		glEnableVertexAttribArray(1);
		glVertexAttribPointer(1, 3, GL_FLOAT, GL_FALSE, sizeof(Vertex), (void*)offsetof(Vertex, Normal));

		// ���������� ���������� ������
		glEnableVertexAttribArray(2);
		glVertexAttribPointer(2, 2, GL_FLOAT, GL_FALSE, sizeof(Vertex), (void*)offsetof(Vertex, TexCoords));

		// ���� ���������� ������
		glEnableVertexAttribArray(3);
		glVertexAttribPointer(3, 3, GL_FLOAT, GL_FALSE, sizeof(Vertex), (void*)offsetof(Vertex, Color));

		glBindVertexArray(0);
	}
};
class Model
{
public:
	Model() {}

	Model(char* path)
	{
		loadModel(path);
	}

	// ������ ������
	vector<Mesh> meshes;
	string directory;

	// �������� ������
	void loadModel(string path)
	{
		Assimp::Importer import;
		const aiScene* scene = import.ReadFile(path, aiProcess_Triangulate | aiProcess_FlipUVs);

		if (!scene || scene->mFlags & AI_SCENE_FLAGS_INCOMPLETE || !scene->mRootNode)
		{
			cout << "ERROR::ASSIMP::" << import.GetErrorString() << endl;
			return;
		}
		directory = path.substr(0, path.find_last_of('/'));

		processNode(scene->mRootNode, scene);
	}

	// ��������� �����
	void processNode(aiNode* node, const aiScene* scene)
	{
		// ������������ ��� ���� (���� ��� ����) � ���������� ����
		for (unsigned int i = 0; i < node->mNumMeshes; i++)
		{
			aiMesh* mesh = scene->mMeshes[node->mMeshes[i]];
			meshes.push_back(processMesh(mesh, scene));
		}
		// � ����������� �� �� ����� ��� ���� �������� �����
		for (unsigned int i = 0; i < node->mNumChildren; i++)
		{
			processNode(node->mChildren[i], scene);
		}
	}

	Mesh processMesh(aiMesh* mesh, const aiScene* scene)
	{
		// ������ ��� ����������
		vector<Vertex> vertices;
		vector<unsigned int> indices;
		vector<Texture> textures;

		// ���� �� ���� �������� ����
		for (unsigned int i = 0; i < mesh->mNumVertices; i++)
		{
			Vertex vertex;
			glm::vec3 vector; // ��������� ������������� ������, �.�. Assimp ���������� ���� ����������� ��������� �����, ������� �� ������������� �������� � ��� glm::vec3, ������� ������� �� �������� ������ � ���� ������������� ������ ���� glm::vec3

			// ����������
			vector.x = mesh->mVertices[i].x;
			vector.y = mesh->mVertices[i].y;
			vector.z = mesh->mVertices[i].z;
			vertex.Position = vector;

			// �������
			vector.x = mesh->mNormals[i].x;
			vector.y = mesh->mNormals[i].y;
			vector.z = mesh->mNormals[i].z;
			vertex.Normal = vector;

			// ���������� ����������
			if (mesh->mTextureCoords[0]) // ���� ��� �������� ���������� ����������
			{
				glm::vec2 vec;
				// ������� ����� ��������� �� 8 ��������� ���������� ���������. �� ������������, ��� �� �� ����� ������������ ������,
				// � ������� ������� ����� ��������� ��������� ���������� ���������, ������� �� ������ ����� ������ ����� (0)
				vec.x = mesh->mTextureCoords[0][i].x;
				vec.y = mesh->mTextureCoords[0][i].y;
				vertex.TexCoords = vec;
			}
			else
				vertex.TexCoords = glm::vec2(0.0f, 0.0f);
			vertex.Color = glm::vec3(0.6f, 0.0f + (rand()%3)/2.0f, 0.0f);
			vertices.push_back(vertex);
		}

		// ������ ���������� �� ������ ����� ���� (����� - ��� ����������� ����) � ��������� ��������������� ������� ������
		for (unsigned int i = 0; i < mesh->mNumFaces; i++)
		{
			aiFace face = mesh->mFaces[i];
			// �������� ��� ������� ������ � ��������� �� � ������� indices
			for (unsigned int j = 0; j < face.mNumIndices; j++)
				indices.push_back(face.mIndices[j]);
		}

		// ���������� mesh-������, ��������� �� ������ ���������� ������
		return Mesh(vertices, indices);
	}
};

Model OURmodel;

//! ������� 
struct vertex
{
	GLfloat x;
	GLfloat y;
	GLfloat z;
};


//! �������� ������ OpenGL, ���� ���� �� ����� � ������� ��� ������ 
void checkOpenGLerror()
{
	GLenum errCode;
	if ((errCode = glGetError()) != GL_NO_ERROR)
		std::cout << "OpenGl error! - " << gluErrorString(errCode);
}

//! ������������� �������� 
void initShader()
{
	glShader.loadFiles("shaders/vertex2.txt", "shaders/fragment2.txt");
	checkOpenGLerror();
}

// Load and create a texture 
GLuint texture;
void text()
{
	// �������� 2
	glGenTextures(1, &texture);
	glBindTexture(GL_TEXTURE_2D, texture);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);

	int width, height;
	unsigned char* image = SOIL_load_image("img/list.jpg", &width, &height, 0, SOIL_LOAD_RGB);
	glTexImage2D(GL_TEXTURE_2D, 0, GL_RGB, width, height, 0, GL_RGB, GL_UNSIGNED_BYTE, image);
	glGenerateMipmap(GL_TEXTURE_2D);
	SOIL_free_image_data(image);
	glBindTexture(GL_TEXTURE_2D, 0);
}


//! ������������� VBO 
void initVBO()
{

}

//! ������������ ������
void freeVBO()
{
	glBindBuffer(GL_ARRAY_BUFFER, 0);
	glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);
	glDeleteBuffers(1, &EBO);
	glDeleteBuffers(1, &VBO);
	glDeleteBuffers(1, &VAO);

}
double angle_x = 0;

void resizeWindow(int width, int height)
{
	glViewport(0, 0, width, height);
}


//! ��������� 
void render()
{
	angle_x += 0.0007;

	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

	glm::mat4 Projection = glm::perspective(glm::radians(45.0f), 4.0f / 3.0f, 0.1f, 1000.0f);
	glm::mat4 View = glm::lookAt(glm::vec3(40, 40, 40), glm::vec3(0, 0, 0), glm::vec3(0, 1, 0));

	glm::mat4 rotate_y = { glm::cos(angle_x), 0.0f, glm::sin(angle_x), 0.0f,
					   0.0f, 1, 0, 0.0f,
					   -glm::sin(angle_x),0, glm::cos(angle_x), 0.0f,
					   0.0f, 0.0f, 0.0f, 1.0f };

	Matrix_projection = Projection * View * rotate_y;

	//! ������������� ��������� ��������� ������� 
		//glUseProgram(Program);
	glShader.use();
	//glUniformMatrix4fv(Unif_matrix, 1, GL_FALSE, &Matrix_projection[0][0]);
	glShader.setUniform(glShader.getUniformLocation("matrix"), Matrix_projection);
	/*
	// Position attribute
	glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 8 * sizeof(GLfloat), (GLvoid*)0);
	glEnableVertexAttribArray(0);
	// Color attribute
	glVertexAttribPointer(1, 3, GL_FLOAT, GL_FALSE, 8 * sizeof(GLfloat), (GLvoid*)(3 * sizeof(GLfloat)));
	glEnableVertexAttribArray(1);
	// TexCoord attribute
	glVertexAttribPointer(2, 2, GL_FLOAT, GL_FALSE, 8 * sizeof(GLfloat), (GLvoid*)(6 * sizeof(GLfloat)));
	glEnableVertexAttribArray(2);

	glBindVertexArray(0); // Unbind VAO*/

	// Bind Textures using texture units
	glActiveTexture(GL_TEXTURE0);
	glBindTexture(GL_TEXTURE_2D, texture);
	glShader.setUniform(glShader.getUniformLocation("ourTexture"), 0);
	//glUniform1i(glGetUniformLocation(Program, "ourTexture"), 0);

	GLsizei count = 0;
	for (auto mesh : OURmodel.meshes)
		count += mesh.indices.size();

	// Draw container
	glBindVertexArray(VAO);
	glDrawElements(GL_TRIANGLES, count, GL_UNSIGNED_INT, 0);
	glBindVertexArray(0);

	glFlush();

	checkOpenGLerror();

	glutSwapBuffers();

	//! ��������� ��������� ���������  
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

	//! ����������� ����� �������������� �������� 
	GLenum glew_status = glewInit();
	if (GLEW_OK != glew_status)
	{
		//! GLEW �� ���������������������  	 	
		std::cout << "Error: " << glewGetErrorString(glew_status) << "\n";
		return 1;
	}

	//! ��������� ����������� OpenGL 2.0  	
	if (!GLEW_VERSION_2_0)
	{
		//! OpenGl 2.0 ��������� �� ��������  	 	
		std::cout << "No support for OpenGL 2.0 found\n";
		return 1;
	}

	//! �������������  
	glClearColor(0.5, 0.5, 0.5, 0);

	text();
	OURmodel = Model("medieval house.obj");
	//initVBO();
	initShader();
	glutReshapeFunc(resizeWindow);
	//glutIdleFunc(render);
	glutDisplayFunc(render);
	glutMainLoop();

	//! ������������ ��������  
	freeVBO();
}