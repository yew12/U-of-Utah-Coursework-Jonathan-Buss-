/**
 * @file mainwindow.h
 * @author Tate, Thatcher, Gage, Sanjay
 * @brief  This file contains the mainwindow header
 * @version 0.1
 * @date 2022-11-15
 *
 * @copyright Copyright (c) 2022, U of U CS 3505
 *
 * @section Reviewed by Sanjay For code style requirements
 *
 */
#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include "model.h"
#include <QMouseEvent>
#include <QColorDialog>

QT_BEGIN_NAMESPACE
namespace Ui
{
    class MainWindow;
}
QT_END_NAMESPACE
// view/controller
/**
 * @brief The MainWindow class headerf for the mainWindow.
 */
class MainWindow : public QMainWindow
{
    Q_OBJECT

public:
    MainWindow(Model &model, QWidget *parent = nullptr);
    ~MainWindow();
    // Events
    void mousePressEvent(QMouseEvent *event);
    const int RANGE = 320;
    const int X_OFFSET = 20;
    const int Y_OFFSET = 32;
    const int SPRITE_SIZE = 32; // 32x32 grid
    const int MULTIPLE_DYNAMIC = 10;
    QImage image;
    QPainter painter;
    QPixmap p;           // load pixmap

    int pixelSize;

private:
    Ui::MainWindow *ui;
    // Position for mouse when clicking screen
    QPoint mousePos;
    bool mousePressed;

    QColor currentPenColor;

signals:
    void previewButtonClicked();
    void loadButtonClicked();
    void saveButtonClicked();
    void exportPNGButtonClicked();
    void invertColorButtonClicked();
    void newFrameButtonClicked();
    void mirrorButtonClicked();
    void clearButtonClicked();
    void fpsChanged(int);
    void gridSizeChanged(int gridSize);
    // Will emit signal to model for performing paint operations
    void mousePressedSignal(int x, int y);
    void penColorChanged(QColor penColor);
    void mirrorCheckedSignal(int x, int y);
    // start of signals where user chooses initial grid size
    void twoByTwoSignal(int gridSizeTwo);
    void fourByFourSignal(int gridSizeFour);
    void eightByEightSignal(int gridSizeEight);
    void sixteenBySixteenSignal(int gridSizeSixteen);
    void thirtyTwoByThirtyTwoSignal(int gridSizeThirtyTwo);

public slots:
    void colorsInPixel(int x, int y, QColor color);
    void updateSpriteLabels(QPixmap spriteImage);
    void updateAnimation(QPixmap animationImage);

private slots:
    void on_previewButton_clicked();
    void on_loadButton_clicked();
    void on_saveButton_clicked();
    void on_exportPNGButton_clicked();
    void on_invertColorButton_clicked();
    void on_newFrameButton_clicked();
    void on_clearButton_clicked();
    void on_fpsSlider_valueChanged(int value);
    void on_gridSizeSlider_valueChanged(int value);
    void on_colorButton_clicked();
    void displaySprite(QPixmap spriteImage);
    void on_twoGridSizeButton_clicked();
    void on_fourGridSizeButton_clicked();
    void on_eightGridSizeButton_clicked();
    void on_sixteenGridSizeButton_clicked();
    void on_thirtyTwoGridSizeButton_clicked();
    void showUI();
};
#endif // MAINWINDOW_H
