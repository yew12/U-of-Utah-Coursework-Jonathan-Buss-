/**
 * Jonathan Gage Buss
 * CS 3505
 * Assignment 2
 **/

#include <math.h>
#include "Spiral.h"

double positionY; // global variable that keeps track of our current Y position in the spiral
double positionX; // global variable that keeps track of our current X position in the spiral

/**
 * Spiral constructor that initializes where our first letter will be placed
 **/
Spiral::Spiral(double x, double y, double startingAngle, double startRadius)
    : centerX(x), centerY(y), angleClockwise(startingAngle), radius(startRadius)
{
    double rad1 = (angleClockwise - 90) / 180 * M_PI; // determines the angle of the letter on the page in radians

    positionX = centerX + cos(rad1) * radius; // initializes x position around spiral for first letter
    positionY = centerY + sin(rad1) * radius; // initializes y position around spiral for first letter
}

/**
 * Prefix operator inspired from stackoverlflow.
 *  - https://stackoverflow.com/questions/3846296/how-to-overload-the-operator-in-two-different-ways-for-postfix-a-and-prefix
 *
 * When called from spiralPDF.cpp, we update our new x and y positions as well as
 * the angle of each character
 **/
Spiral &Spiral::operator++()
{

    radius += 1.17;                          // increment radius by a factor of our choice
    angleClockwise = angleClockwise - 20;    // subtract angle by a factor of our choice
    angleClockwise = angleClockwise * .9891; // then multiply by a fraction of our choice to continually adjust our angle

    /**
     * PI constant:
     * - https://stackoverflow.com/questions/1727881/how-to-use-the-pi-constant-in-c
     **/
    double rad2 = (angleClockwise - 90) / 180 * M_PI; // determines the angle of the letter on the page in radians

    positionX = centerX + cos(rad2) * radius; // updates x position around spiral
    positionY = centerY + sin(rad2) * radius; // updates y position around spiral

    return *this;
}

/**
 * You want to make the ++ operator work like the standard operators
 * The simple way to do this is to implement postfix in terms of prefix.
 *
 * https://stackoverflow.com/questions/3846296/how-to-overload-the-operator-in-two-different-ways-for-postfix-a-and-prefix
 *
 **/
Spiral Spiral::operator++(int) // postfix ++
{
    Spiral result(*this); // make a copy for result
    ++(*this);            // Now use the prefix version to do the work
    return result;        // return the copy (the old) value.
}

/**
 * gives the X position on the page of the current spiral point
 **/
double Spiral::getTextX()
{
    return positionX; // returns our updated x position set in our operator++ method
}

/**
 * gives the Y position on the page of the current spiral point
 **/
double Spiral::getTextY()
{
    return positionY; // returns our updated y position set in our operator++ method
}

/**
 * Returns the angle a letter should be printed at for the current spiral point.
 * should report an angle consistent with the angle of the spiral -
 * that is, 0 degrees is 12 o'clock and a positive change in angle is a clockwise change.
 *
 * Variable is updated in our prefix operator++ method
 **/
double Spiral::getLetterAngle()
{
    return angleClockwise; // returns our angle of our letter in spiral
}