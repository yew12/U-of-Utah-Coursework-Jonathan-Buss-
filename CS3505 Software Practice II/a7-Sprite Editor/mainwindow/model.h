/**
 * @file model.h
 * @author Tate, Thatcher, Gage, Sanjay
 * @brief This file contains the Model class which handles all of the data for the program
 * @version 0.1
 * @date 2022-11-15
 *
 * @copyright Copyright (c) 2022, U of U CS 3505
 *
 * @section Reviewed by Gage For code style requirements
 */
#ifndef MODEL_H
#define MODEL_H

#include <QObject>
#include "animationpreview.h"
#include "editor.h"
#include <QImage>
#include <QPixmap>
#include <QLabel>
#include <QApplication>
#include <QFile>
#include <QFileDialog>
#include <QMessageBox>

/**
 * @brief The Model class header for the model.
 */
class Model : public QWidget
{
    Q_OBJECT
public:
    explicit Model(QWidget *parent = nullptr);
    AnimationPreview animationPreview; // animation preview widget object
    Editor editor;                     // editor grid widget object
    QVector<QImage> sprites;           // holds each frame(sprite) of animation
    QVector<QVector<int>> grid;        // 2D vector representing the grid
    // Empty sprite which will be converted to QPixmap for display
    QImage sprite = QImage(400, 300, QImage::Format_RGB32);

public slots:
    void startAnimationPreviewer();
    void adjustFPSCount();
    void paintPixel(int x, int y);
    void fillButton();
    void invertButton();
    void clearButton();
    void newFrameButton();
    void exportAsPNGButton();
    void saveButton();
    void loadButton();
    void updateColor(QColor c);
    void mirrorCheckBox(int isChecked); // 2 = checked | 0 = unchecked
    // Slots for when user chooses initial grid size
    void gridSizeTwoByTwo(int gridSizeTwo);
    void gridSizeFourByFour(int gridSizeFour);
    void gridSizeEightByEight(int gridSizeEight);
    void gridSizeSixteenBySixteen(int gridSizeSixteen);
    void gridSizeThirtyTwoByThirtyTwo(int gridSizeThirtyTwo);

signals:
    void updateFPSSlider(int); // updates our horizontal fps slider
    void updateLabel(QPixmap);
    void updateSprite(QPixmap);
    void showUI(); // displays UI once a grid size is chosen
};

#endif // MODEL_H
