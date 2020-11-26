cmake_minimum_required(VERSION 3.16)
project(CompGraphicsLab10_Savelev)

set(CMAKE_CXX_STANDARD 98)

add_executable(CompGraphicsLab10_Savelev main.cpp figures.cpp figures.h)

include_directories(/usr/local/include/)
target_link_libraries(CompGraphicsLab10_Savelev /usr/local/lib/libglut.dylib)

#target_link_libraries(CompGraphicsLab10_Savelev "-framework GLUT") #Apple's GLUT

target_link_libraries(CompGraphicsLab10_Savelev "-framework OpenGL")
