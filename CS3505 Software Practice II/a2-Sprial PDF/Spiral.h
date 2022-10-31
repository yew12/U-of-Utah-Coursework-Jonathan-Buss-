#include <iostream>
#ifndef SPIRAL_H
#define SPIRAL_H
using namespace std;
#include <math.h> // PI constant - https://stackoverflow.com/questions/1727881/how-to-use-the-pi-constant-in-c

/**
 * Jonathan Gage Buss
 * CS 3505
 * Assignment 2
 **/

/**
 * Header class that essentially gives us an "outline" of what the
 * Spiral.cpp file contains.
 **/
class Spiral
{
    double centerX;
    double centerY;
    double angleClockwise;
    double radius;

public:
    Spiral(double x, double y, double startingAngle, double startRadius);
    double getTextX();
    double getTextY();
    double getLetterAngle();
    Spiral &operator++();
    Spiral operator++(int);
};

#endif