/**
 * @file model.cpp
 * @author Tate, Thatcher, Gage, Sanjay
 * @brief  Implementation of the Model class which handles all of the data for the program
 * @version 0.1
 * @date 2022-11-15
 *
 * @copyright Copyright (c) 2022, U of U CS 3505
 *
 * @section Reviewed by Gage For code style requirements
 */

#include "model.h"
#include "nlohmann/json.hpp"
#include <iostream>
#include <fstream>
#include <QDebug>

using json = nlohmann::json;

Model::Model(QWidget *parent)
    : QWidget{parent}
{
    AnimationPreview animationPreview;
    sprite.fill(qRgba(0, 0, 0, 0));
}

/**
 * @brief - changes color of the editor at x.y
 */
void Model::paintPixel(int x, int y)
{

    editor.setColor(x, y); // calls method in editor.cpp to paint pixel in given location
}

/**
 * @brief fills the sprite with the color of the editor
 *
 */
void Model::fillButton()
{
    editor.fillFlag = true;
}

/**
 * @brief inverts all colors
 *
 */
void Model::invertButton()
{
    editor.revert();
}

/**
 * @brief clears the editor
 *
 */
void Model::clearButton()
{
    editor.clearColor();
}

/**
 * @brief add the current QPixmap to the vector of QPixmaps in animationPreview.imagesVector
 *
 */
void Model::newFrameButton()
{
    QPixmap currentFrame = editor.toPixmap();
    animationPreview.imagesVector.push_back(currentFrame);
}

/**
 * @brief Sprite can be exported as a png file
 *
 */
void Model::exportAsPNGButton()
{
    QImage image = QImage(editor.pixels.size(), editor.pixels.size(), QImage::Format_RGB32);
    for (int i = 0; i < editor.pixels.size(); i++)
    {
        for (int j = 0; j < editor.pixels.size(); j++)
        {
            image.setPixelColor(i, j, editor.pixels[i][j]);
        }
    }
    // image.save("test.png");
    QString fileName = QFileDialog::getSaveFileName(this, tr("Save File"), "sprite.png", tr("PNG (*.png)"));
    if (fileName.isEmpty())
        return;
    else
    {
        image.save(fileName);
    }
}

/**
 * @brief exports the current animation to a JSON file (.ssp format)
 *
 */
void Model::saveButton()
{
    // convert the QPixmap vector to a QImage vector
    json jsonFile;
    jsonFile["height"] = editor.pixels.size();
    jsonFile["width"] = editor.pixels.size();
    // if (animationPreview.imagesVector.size() < 1), then we need to add the current frame to the vector without using toPixMap()
    if (animationPreview.imagesVector.size() < 1)
    {
        // add the current editor.pixels to the animation preview
        QImage image = QImage(editor.pixels.size(), editor.pixels.size(), QImage::Format_RGB32);
        for (int i = 0; i < editor.pixels.size(); i++)
        {
            for (int j = 0; j < editor.pixels.size(); j++)
            {
                image.setPixelColor(i, j, editor.pixels[i][j]);
            }
        }
        animationPreview.imagesVector.push_back(QPixmap::fromImage(image));
    }
    jsonFile["numberOfFrames"] = animationPreview.imagesVector.size(); // TODO: sprites.size();
    // create a jsonFile["frames"] object to hold all the frames
    json frames;

    for (int i = 0; i < jsonFile["numberOfFrames"]; i++)
    {
        frames["frame" + std::to_string(i)] = json::array();
    }

    for (int i = 0; i < jsonFile["numberOfFrames"]; i++)
    {
        // for each frame, loop through animationPreview.imagesVector[i], convert it to QImage, and add each pixel to the 2d array
        QImage img = animationPreview.imagesVector[i].toImage();
        for (int j = 0; j < jsonFile["height"]; j++)
        {
            // each frame has an array of arrays of pixels
            frames["frame" + std::to_string(i)].push_back(json::array());
            for (int k = 0; k < jsonFile["width"]; k++)
            {
                // each pixel has an array of 4 values
                frames["frame" + std::to_string(i)][j].push_back(json::array());
                QColor color = img.pixelColor(k, j);
                frames["frame" + std::to_string(i)][j][k].push_back(color.red());
                frames["frame" + std::to_string(i)][j][k].push_back(color.green());
                frames["frame" + std::to_string(i)][j][k].push_back(color.blue());
                frames["frame" + std::to_string(i)][j][k].push_back(color.alpha());
            }
        }
    }

    jsonFile["frames"] = frames;
    //  save the file to a .ssp file (Sprite Sheet Project) using QFileDialog and Qfile to save the file
    QString fileName = QFileDialog::getSaveFileName(nullptr, "Save File", "newSprite.ssp", "Sprite Sheet Project (*.ssp)");
    QFile saveFile(fileName);
    if (!saveFile.open(QIODevice::WriteOnly))
    {
        qWarning("Couldn't open save file.");
    }
    saveFile.write(jsonFile.dump(4).c_str());
}

/**
 * @brief loads a .ssp file and displays the sprite sheet
 *
 */
void Model::loadButton()
{
    try
    {

        QString fileName = QFileDialog::getOpenFileName(nullptr, "Open File", "", "Sprite Sheet Project (*.ssp)");
        QFile loadFile(fileName);
        if (!loadFile.open(QIODevice::ReadOnly))
        {
            qWarning("Couldn't open save file.");
        }
        QByteArray saveData = loadFile.readAll();
        json jsonFile = json::parse(saveData.toStdString());

        // read the json file and update the editor.pixels vector and pixelSize and frames
        editor.pixels.clear();
        animationPreview.imagesVector.clear();
        editor.pixelSize = jsonFile["height"];
        animationPreview.imagesVector.resize(jsonFile["numberOfFrames"]);

        // set pixels to frame 0
        for (int i = 0; i < jsonFile["height"]; i++)
        {
            editor.pixels.push_back(QVector<QColor>());
            for (int j = 0; j < jsonFile["width"]; j++)
            {
                editor.pixels[i].push_back(QColor(jsonFile["frames"]["frame0"][j][i][0], jsonFile["frames"]["frame0"][j][i][1], jsonFile["frames"]["frame0"][j][i][2], jsonFile["frames"]["frame0"][j][i][3]));
            }
        }

        // add all frames to animationPreview.imagesVector (which takes in QPixmaps)
        for (int i = 0; i < jsonFile["numberOfFrames"]; i++)
        {
            QPixmap frame(jsonFile["width"], jsonFile["height"]);
            frame.fill(Qt::transparent);
            QPainter painter(&frame);
            for (int j = 0; j < jsonFile["height"]; j++)
            {
                for (int k = 0; k < jsonFile["width"]; k++)
                {
                    painter.setPen(QColor(jsonFile["frames"]["frame" + std::to_string(i)][j][k][0], jsonFile["frames"]["frame" + std::to_string(i)][j][k][1], jsonFile["frames"]["frame" + std::to_string(i)][j][k][2], jsonFile["frames"]["frame" + std::to_string(i)][j][k][3]));
                    painter.drawPoint(k, j);
                }
            }
            animationPreview.imagesVector[i] = frame;
        }

        emit updateSprite(animationPreview.imagesVector[0]);
        editor.revert();
        editor.revert();
    }
    // catch any error
    catch (const std::exception &e)
    {
        // show a message box to the user, but let them use the app
        QMessageBox msgBox;
        msgBox.setText("Error parsing JSON file.");
        msgBox.exec();
    }
}

/**
 * @brief Mirrors the colored pixels
 *
 * @param isChecked
 */
void Model::mirrorCheckBox(int isChecked)
{
    if (isChecked == 2)
    {

        editor.mirror = true;
    }
    else
    {

        editor.mirror = false;
    }
}

/**
 * @brief sets grid size to 2x2
 *
 * @param gridSizeTwo  2
 */
void Model::gridSizeTwoByTwo(int gridSizeTwo)
{
    std::cout << "User chose grid size: " << gridSizeTwo << std::endl;
    editor.setScreenSize(gridSizeTwo);
    emit showUI(); // displays ui
}

/**
 * @brief sets grid size to 4x4
 *
 * @param gridSizeFour  4
 */
void Model::gridSizeFourByFour(int gridSizeFour)
{
    std::cout << "User chose grid size: " << gridSizeFour << std::endl;
    editor.setScreenSize(gridSizeFour);
    emit showUI(); // displays ui
}

/**
 * @brief sets grid size to 8x8
 *
 * @param gridSizeEight  8
 */
void Model::gridSizeEightByEight(int gridSizeEight)
{
    std::cout << "User chose grid size: " << gridSizeEight << std::endl;
    editor.setScreenSize(gridSizeEight);
    emit showUI(); // displays ui
}

/**
 * @brief sets grid size to 16x16
 *
 * @param gridSizeSixteen  16
 */
void Model::gridSizeSixteenBySixteen(int gridSizeSixteen)
{
    std::cout << "User chose grid size: " << gridSizeSixteen << std::endl;
    editor.setScreenSize(gridSizeSixteen);
    emit showUI(); // displays ui
}

/**
 * @brief sets grid size to 32x32
 *
 * @param gridSizeThirtyTwo  32
 */
void Model::gridSizeThirtyTwoByThirtyTwo(int gridSizeThirtyTwo)
{
    std::cout << "User chose grid size: " << gridSizeThirtyTwo << std::endl;
    editor.setScreenSize(gridSizeThirtyTwo);
    emit showUI(); // displays ui
}

/**
 * @brief updates the color of the editor to c
 *
 * @param c  QColor
 */
void Model::updateColor(QColor c)
{
    editor.color = c; // sets color in editor class
}

/**
 * @brief starts the animation preview
 *
 */
void Model::startAnimationPreviewer()
{
    animationPreview.startPreview(); // calls our helper method that starts display of sprite images
}

/**
 * @brief change fps
 *
 */
void Model::adjustFPSCount()
{
    animationPreview.fpsCount = 0;
}
