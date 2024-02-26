/**
 * @file editor.h
 * @author Tate, Thatcher, Gage, Sanjay
 * @brief  This file contains the editor header.
 * @version 0.1
 * @date 2022-11-15
 *
 * @copyright Copyright (c) 2022, U of U CS 3505
 *
 * @section Reviewed by Thatcher For code style requirements
 *
 */
#ifndef EDITOR_H
#define EDITOR_H

#include <QWidget>
#include <QVector>
#include <QBrush>
#include <QColor>
#include <QPainter>

class Editor : public QWidget
{
    Q_OBJECT
public:
    explicit Editor(QWidget *parent = nullptr);
    bool mirror;
    bool fillFlag;
    int  size;
    int pixelLimit;// toggles whether or not user wants to mirror their pixel placement
    QBrush brush;                                              // color brush
    QVector<QVector<QColor>> pixels;
    QColor color;
    int pixelSize;                                             // size of the pixels on the screen
    void paint(int x, int y, int pixelSize, QPainter painter); // paints given color at x and y positions
    void setBrushColor(QColor color);                          // sets our global brush variable of given color
    void setMirror();                                          // toggles mirror variable
    void setScreenSize(int pixelSize);                              // sets the pixel size
    void pixelsCreator(int size);
    void revert();
    void clearColor();
    void setColor(int x, int y); // method is called from model to set color
    void fill(int x, int y, QColor old);
    void rePaint();
    QPixmap toPixmap();

public slots:

signals:
    void setPixelColor(int x, int y, QColor color);
    void setPixelLocation(QRect);

private:
    void colorInPixels(int x, int y, QColor color); // fills in each pixel with given color
};

#endif // EDITOR_H
