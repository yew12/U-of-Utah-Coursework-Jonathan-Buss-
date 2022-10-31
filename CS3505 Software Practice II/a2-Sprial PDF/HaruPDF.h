
/**
 * Jonathan Gage Buss
 * CS 3505
 * Assignment 2
 **/

#ifndef HARUPDF_H
#define HARUPDF_H
#include "hpdf.h"
#include <iostream>
using namespace std;

/**
 * Header class that essentially gives us an "outline" of what the
 * HaruPDF.cpp file contains.
 **/
class HaruPDF
{
    HPDF_Doc pdf;   // creates our pdf object from libharu
    HPDF_Page page; // creates pdf singular page object
    HPDF_Font font; // creates pdf font object

public:
    HaruPDF()
    {
        // intilialize pdf object
        pdf = HPDF_New(NULL, NULL);
        /* add a new page object. */
        page = HPDF_AddPage(pdf);
        HPDF_Page_SetSize(page, HPDF_PAGE_SIZE_A5, HPDF_PAGE_PORTRAIT);
        //    print_grid  (pdf, page);
        font = HPDF_GetFont(pdf, "Helvetica", NULL);
        HPDF_Page_SetTextLeading(page, 20);
        HPDF_Page_SetGrayStroke(page, 0);

        HPDF_Page_BeginText(page);

        // Their example sets font twice. Probably some kind of mistake. Fix it or do what they do.
        font = HPDF_GetFont(pdf, "Courier-Bold", NULL);
        HPDF_Page_SetFontAndSize(page, font, 30);

    } // default HaruPDF constructor

    void printChar(char character, double angle, double positionX, double positionY); // prints character to our pdf file
    void savePDF(const char *fname);                                                  // saves our pdf file with fname
};

#endif