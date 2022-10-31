/**
 * Jonathan Gage Buss
 * CS 3505
 * Assignment 2
 **/

#include <iostream>
using namespace std;
#include <stdlib.h>
#include <stdio.h>
#include <string.h>
#include <cstring>
#include "Spiral.h"
#include "HaruPDF.h"

/**
 * Main programs that creates our HaruPdf and Spiral pdf object.
 * This allows the code to talk to both our Spiral and HaruPDF classes without allowing
 * them to directly communicate with each other.
 **/
int main(int argc, char **argv)
{
    double centerX = 210;        // set values from PDFExample.cpp
    double centerY = 300;        // set values from PDFExample.cpp
    double angleClockwise = 180; // set values from PDFExample.cpp
    double startRadius = 50;     // will increase as spiral gets bigger

    HaruPDF pdf; // create pdf object

    char fname[256];

    strcpy(fname, argv[0]); // argv[0] is the name of the executable program and the executable is now the fname
    strcat(fname, ".pdf");  // concatenates our new fname with the .pdf extension

    Spiral spiral(centerX, centerY, angleClockwise, startRadius); // create spiral object with starting values

    /**
     * Create a duplicate of the command line string, then loop through this string
     * We use argv at index 1 because this is the start of our command line text
     **/
    string commandLineText = argv[1];

    // Place characters one at a time on the page.
    for (unsigned int i = 0; i < commandLineText.length(); i++)
    {
        // print character by character
        pdf.printChar(commandLineText[i], spiral.getLetterAngle(), spiral.getTextX(), spiral.getTextY());
        // update location of text character using the overloaded prefix ++ operator method
        ++spiral;
    }

    // saves our pdf
    pdf.savePDF(fname);

    return 0;
}