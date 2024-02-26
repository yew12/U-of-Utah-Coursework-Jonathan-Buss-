/**
 * @file animationpreview.h
 * @author Tate, Thatcher, Gage, Sanjay
 * @brief  This file contains the AnimationPreview class which is used to display the animation preview
 * @version 0.1
 * @date 2022-11-15
 *
 * @copyright Copyright (c) 2022, U of U CS 3505
 *
 * @section Reviewed by Tate For code style requirements
 *
 */

#ifndef ANIMATIONPREVIEW_H
#define ANIMATIONPREVIEW_H

#include <QWidget>
#include <QVector>
#include <QPixmap>
#include <QTimer>

/**
 * @brief The AnimationPreview class
 * AnimationPreivew header
 */
class AnimationPreview : public QWidget
{
    Q_OBJECT
public:
    explicit AnimationPreview(QWidget *parent = nullptr);
    QVector<QPixmap> imagesVector; // stores our pixel images
    QPixmap image;                 // singular image
    QTimer *timer;
    void startPreview();
    bool previewing;
    int fpsCount;
    void emitImageSignalMethod(int i);

public slots:
    void updateFPSCount(int i);

signals:
    void sendImage(QPixmap);

private:
    int animationSpeed; // calculated animation speed
    int imageIndex;     // keeps track of what image we are at in our vector
};

#endif // ANIMATIONPREVIEW_H
