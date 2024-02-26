/**
 * @file animationpreview.cpp
 * @author Tate, Thatcher, Gage, Sanjay
 * @brief  Implementation of the AnimationPreview class
 * @version 0.1
 * @date 2022-11-15
 *
 * @copyright Copyright (c) 2022, U of U CS 3505
 *
 * @section Reviewed by Tate For code style requirements
 */

#include "animationpreview.h"
#include <iostream>

AnimationPreview::AnimationPreview(QWidget *parent)
    : QWidget{parent}
{
    fpsCount = 30; // starting fps value
    imageIndex = 0;
    previewing = false;
}

/**
 * @brief - starts the preview of images inside our vector
 * Emits a signal every n seconds to be displayed in animation previewer label
 */
void AnimationPreview::startPreview()
{
    // insuring we are not going to preview no frames.
   if(imagesVector.size() == 0)
       return;
    if (previewing)
    {
        timer->stop();
        previewing = false;
    }
    else
    {
        // circles around the vector of images
        timer = new QTimer(this);
        connect(timer, &QTimer::timeout, this, [=]()
                {
            emitImageSignalMethod(imageIndex);
            if (imageIndex == imagesVector.size() - 1)
            {
                imageIndex = 0;
            }
            else
            {
                imageIndex++;
            } });
        timer->start(1000 / fpsCount);
        previewing = true;
    }
}

/**
 * @brief - Emits a signal every n seconds to be displayed in animation previewer label
 */
void AnimationPreview::emitImageSignalMethod(int i)
{
    emit sendImage(imagesVector.at(i));
}

/**
 * @brief -SLOT that recieves the fps count from our fpsSlider in mainwindow.cpp
 */
void AnimationPreview::updateFPSCount(int i)
{
    fpsCount = i;
}
