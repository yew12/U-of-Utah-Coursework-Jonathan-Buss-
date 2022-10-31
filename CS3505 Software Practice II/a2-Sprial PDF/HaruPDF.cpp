/**
 * Jonathan Gage Buss
 * CS 3505
 * Assignment 2
 **/

#include "HaruPDF.h"
#include "hpdf.h"
#include <stdlib.h>
#include <stdio.h>
#include <string.h>
#include <iostream>
#include <math.h>

/**
 * Prints character onto pdf file that we have created using the HPDF library objects
 **/
void HaruPDF::printChar(char character, double angle, double positionX, double positionY)
{
    angle = -0.33; // currently at a set angle to keep a consistent spiral. (should be modified)

    // This ugly function defines where any following text will be placed
    // on the page. The cos/sin stuff is actually defining a 2D rotation
    // matrix.
    HPDF_Page_SetTextMatrix(page,
                            cos(angle), sin(angle), -sin(angle), cos(angle),
                            positionX, positionY);

    // C-style strings are null-terminated. The last character must a 0.
    char buf[2];
    buf[0] = character; // The character to display
    buf[1] = 0;
    HPDF_Page_ShowText(page, buf); // show text on pdf
}

/**
 * Saves our pdf once completed using the hpdf library methods
 **/
void HaruPDF::savePDF(const char *fname)
{
    HPDF_Page_EndText(page);
    /* save the document to a file */
    HPDF_SaveToFile(pdf, fname);
    /* clean up */
    HPDF_Free(pdf);
}
