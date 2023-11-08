cmake_minimum_required(VERSION 3.26)
project(Trivial)

set(CMAKE_CXX_STANDARD 17)

add_executable(Trivial src/main.cpp src/lib/lib.cpp)
target_include_directories(Trivial PRIVATE inc)