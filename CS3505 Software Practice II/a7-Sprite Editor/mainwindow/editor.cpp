/**
 * @file editor.cpp
 * @author Tate, Thatcher, Gage, Sanjay
 * @brief  This file contains the editor class which is used to store data and use methods
 * @version 0.1
 * @date 2022-11-15
 *
 * @copyright Copyright (c) 2022, U of U CS 3505
 *
 * @section Reviewed by Thatcher For code style requirements
 *
 */
#include "editor.h"
#include <iostream>
/**
 * @brief Editor::Editor the constructor for the Editor We dont initalize anything in the constructor
 * since we dont use the editor class until the size is selected.
 * @param parent Qwidget
 */
Editor::Editor(QWidget *parent)
    : QWidget{parent}
{

}

/**
 * @brief qt: paints on the editor screen.
 * the editor is a 2d array of pixels and the size of the pixels can be changed by the user.
 * The x and y coordinates of the mouse are used to determine which pixel is being painted on,
 * and must be translated based on the dimensions of the grid and the size of the pixels.
 *
 * @param x the x coordinate of the mouse
 * @param y the y coordinate of the mouse
 * @param painter the painter object used to paint on the screen
 */
void Editor::paint(int x, int y, int pixelSize, QPainter painter)
{
    // paint the pixel at the mouse position
    painter.drawRect(x, y, pixelSize, pixelSize);
    painter.fillRect(x, y, pixelSize, pixelSize, brush);
}

/**
 * @brief Editor::toPixmap
 * @return
 */
QPixmap Editor::toPixmap()
{
    QPixmap pixmap(size * pixelSize, size * pixelSize);
    QPainter painter(&pixmap);
    for (int i = 0; i < size; i++)
    {
        for (int j = 0; j < size; j++)
        {
            painter.fillRect(i * pixelSize, j * pixelSize, pixelSize, pixelSize, pixels[i][j]);
        }
    }
    return pixmap;
}

/**
 * @brief sets the color of the painter brush being used by the user
 *
 * @param color the color to set the brush to
 */
void Editor::setBrushColor(QColor color)
{
    brush.setColor(color);
}

/**
 * @brief toggles the mirror variable
 *
 */
void Editor::setMirror()
{
    mirror = !mirror;
}

/**
 * @brief sets the size of the pixels on the screen
 *
 * @param size the size of the pixels
 */
void Editor::setScreenSize(int pixelSize)
{
    mirror = false;
    brush = QBrush(Qt::black);
    this->pixelSize = pixelSize;
    pixelLimit = pixelSize -1;
    fillFlag = false;
    size = 32;
    color = qRgba(0, 0, 0, 0);
    pixels = QVector<QVector<QColor>>(size, QVector<QColor>(size, qRgba(255, 255, 255, 255)));
}

/**
 * @brief Editor::colorInPixels Colors in the pixels by coloring in all the colors that
 * are needed relative to the pixel size. Since we are working with 32 pixels if the size
 * was 4 we must paint a 8 X 8 pixel box. This method converts the coordinate to
 * the actual box and pixels needed to paint.
 * @param x the index of the x coordinate
 * @param y thew index of the y coordinate
 * @param color color to be colored in
 */
void Editor::colorInPixels(int x, int y, QColor color)
{
    int scale = size / pixelSize;
    int pixelX = x * scale;
    int pixelY = y * scale;
    if (pixelX < 0 || pixelX >= pixels.size() || pixelY < 0 || pixelY >= pixels.size())
        return;
    for (int i = pixelX; i < (scale + pixelX); i++)
    {
        for (int j = pixelY; j < (scale + pixelY); j++)
        {
            pixels[i][j] = color; // this gives an index our of bound error.
            emit setPixelColor(i, j, color);
        }
    }
}

/**
 * @brief Editor::setColor The main function to be called when changing the color in the pixel editor
 * @param x the index of the x coordinate
 * @param y the index of the y coordinate
 */
void Editor::setColor(int x, int y)
{
    // if the fillFlagt is true we do a fill function instead of painting
    if(fillFlag){
        int scale = size / pixelLimit;
        x *= scale;
        y *= scale;
        if (x < 0 || x >= pixels.size() || y < 0 || y >= pixels.size())
            return;
        if(pixels[x][y].red() == color.red() && pixels[x][y].green() == color.green() && pixels[x][y].blue() == color.blue() && pixels[x][y].alpha() == color.alpha())
        {
             std::cout << "same color" << std::endl;
             return;
        }

        fill(x , y, pixels[x][y]);
        fillFlag = false;
        rePaint();
        return;
    }

    // actually colors the pixels with this function
    colorInPixels(x, y, color);
    if(mirror){
        if(x * 2 > pixelLimit)
            colorInPixels(x - (2 * x - pixelLimit), y, color);
        else
            colorInPixels(x - (2 * x - pixelLimit), y, color);
    }
}

/**
 * @brief Editor::revert reverts the colors for all the pixels
 */
void Editor::revert()
{
    for (int i = 0; i < pixels.size(); i++)
    {
        for (int j = 0; j < pixels.size(); j++)
        {
            QColor reverted = qRgba(255 - pixels[i][j].red(), 255 - pixels[i][j].green(), 255 - pixels[i][j].blue(), 255 - pixels[i][j].alpha());
            pixels[i][j] = reverted;
            emit setPixelColor(i, j, reverted);
        }
    }
}

/**
 * @brief Editor::clearColor clears the all the pixels
 */
void Editor::clearColor()
{
    for(int i = 0; i < pixels.size(); i++)
    {
        for(int j = 0; j < pixels.size(); j++)
        {
            pixels[i][j] = qRgba(255,255,255,255);
            emit setPixelColor(i, j, pixels[i][j]);
        }
    }
}

/**
 * @brief Editor::fill Fill function that colors in the new color with the old color.
 * It colors in all the old colors it can reach
 * This is just a DFS reaching all the old colors it can
 * @param x the index of the x coordinate
 * @param y the index of the y coordinate
 * @param old the old color that the user clicked it will only change this color to the new color
 */
void Editor::fill(int x, int y, QColor old)
{
    if(x < 0 || x >= size || y < 0 || y >= size)
        return;
    if(!(pixels[x][y].red() == old.red() && pixels[x][y].green() == old.green() && pixels[x][y].blue() == old.blue() && pixels[x][y].alpha() == color.alpha()))
        return;

    pixels[x][y] = color;
    fill(x + 1, y , old);
    fill(x - 1, y , old);
    fill(x, y + 1, old);
    fill(x, y -  1, old);
}

/**
 * @brief Editor::rePaint repaints all the colors in the pixel editor.
 */
void Editor::rePaint()
{
    for(int i = 0; i < pixels.size(); i++)
    {
        for(int j = 0; j < pixels.size(); j++)
        {
            emit setPixelColor(i, j, pixels[i][j]);
        }
    }
}

