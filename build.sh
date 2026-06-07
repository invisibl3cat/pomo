#! /bin/sh

RAYLIB_DIR=${1}

if [ -z "$RAYLIB_DIR" ]; then
	echo "No path to Raylib dir"
	exit 1
fi

g++ -std=c++20 -Wall -Wextra -pedantic pomo.cpp -I"${RAYLIB_DIR}/include" -L"${RAYLIB_DIR}/lib" -lraylib -o pomo
