/**
 * @file mainwindow.cpp
 * @author Tate, Thatcher, Gage, Sanjay
 * @brief  This file contains the mainwindow class which is used to display the animation preview
 * @version 0.1
 * @date 2022-11-15
 *
 * @copyright Copyright (c) 2022, U of U CS 3505
 *
 * @section Reviewed by Sanjay For code style requirements
 *
 */
#include "mainwindow.h"
#include "ui_mainwindow.h"
#include "model.h"
#include <iostream>
#include <QLabel>

MainWindow::MainWindow(Model &model, QWidget *parent)
    : QMainWindow(parent), ui(new Ui::MainWindow)
{

    ui->setupUi(this);
    setWindowTitle("Spritely");
    // set initial grid size label to 8 and fps label to 30

    ui->fpsLabel->setText("FPS: " + QString::number(30));
    mousePressed = false;

    // HIDE ALL OF THE BUTTONS ON INITIAL OPEN
    ui->verticalLayoutWidget_5->hide();
    ui->spriteLabel->hide(); // where we color our spirte
    ui->verticalLayoutWidget_2->hide(); // animationLayoutWidget
    ui->verticalLayoutWidget_3->hide(); // colorLayout
    ui->verticalLayoutWidget_4->hide(); // grid label layout
    ui->imagesLabel->hide();

    // CONNECTIONS FOR USER CHOOSING INTIAL GRID SIZE
    // 2x2
    connect(this,
            &MainWindow::twoByTwoSignal,
            &model,
            &Model::gridSizeTwoByTwo);
    // 4x4
    connect(this,
            &MainWindow::fourByFourSignal,
            &model,
            &Model::gridSizeFourByFour);
    // 8x8
    connect(this,
            &MainWindow::eightByEightSignal,
            &model,
            &Model::gridSizeEightByEight);
    // 16x16
    connect(this,
            &MainWindow::sixteenBySixteenSignal,
            &model,
            &Model::gridSizeSixteenBySixteen);
    // 32x32
    connect(this,
            &MainWindow::thirtyTwoByThirtyTwoSignal,
            &model,
            &Model::gridSizeThirtyTwoByThirtyTwo);
    // display UI once size is chosen
    connect(&model,
            &Model::showUI,
            this,
            &MainWindow::showUI);


    // connection for PAINTING SPRITE
    connect(this,                            // connected to ui
            &MainWindow::mousePressedSignal, // clicked signal
            &model,                          // pointer to model object
            &Model::paintPixel               // our slot
    );

    // connection for DISPLAYING SPRITE
    connect(&model,                    // connected to ui
            &Model::updateLabel,       // clicked signal
            this,                      // pointer to model object
            &MainWindow::displaySprite // our slot
    );

    // STARTS ANIMATION PREVIEWER
    connect(this,
            &MainWindow::previewButtonClicked,
            &model,
            &Model::startAnimationPreviewer);

    // displays our image in the imagesLabel
    connect(&model.animationPreview,
            &AnimationPreview::sendImage,
            this,
            &MainWindow::updateAnimation);

    // sends the updated FPS count to our AnimationPreview class
    connect(ui->fpsSlider,
            &QSlider::sliderMoved,
            &model.animationPreview,
            &AnimationPreview::updateFPSCount);

    // once mouse is pressed, signal is sent to model to paint pixel at location
    connect(this,
            &MainWindow::mousePressedSignal,
            &model,
            &Model::paintPixel);
    // connects signal sent from editor to slot in mainwindow.cpp to update GUI
    connect(&model.editor,
            &Editor::setPixelColor,
            this,
            &MainWindow::colorsInPixel);

    // connection for fill button when clicked
    connect(ui->fillButton,
            &QPushButton::clicked,
            &model,
            &Model::fillButton // slot in model.cpp that handles data when fillButton is clicked
    );
    // connection for invert button when clicked
    connect(ui->invertColorButton,
            &QPushButton::clicked,
            &model,
            &Model::invertButton // slot in model.cpp that handles data when invertButton is clicked
    );
    // connection for invert button when clicked
    connect(ui->clearButton,
            &QPushButton::clicked,
            &model,
            &Model::clearButton // slot in model.cpp that handles data when invertButton is clicked
    );
    // connection for new frame button when clicked
    connect(ui->newFrameButton,
            &QPushButton::clicked,
            &model,
            &Model::newFrameButton // slot in model.cpp that handles data when newFrameButton is clicked
    );
    // connection for export as PNG button when clicked
    connect(ui->exportPNGButton,
            &QPushButton::clicked,
            &model,
            &Model::exportAsPNGButton // slot in model.cpp that handles data when exportAsPNGButton is clicked
    );
    // connection for save button when clicked
    connect(ui->saveButton,
            &QPushButton::clicked,
            &model,
            &Model::saveButton // slot in model.cpp that handles data when saveButton is clicked
    );
    // connection for load button when clicked
    connect(ui->loadButton,
            &QPushButton::clicked,
            &model,
            &Model::loadButton // slot in model.cpp that handles data when loadButton is clicked
    );

    // connection between view and model to set editor.color QColor
    connect(this,
            &MainWindow::penColorChanged,
            &model,
            &Model::updateColor);
    // connection for mirror checkBox
    connect(ui->mirrorBox,
            &QCheckBox::stateChanged, // 2 is sent if checked, 0 if not
            &model,
            &Model::mirrorCheckBox);

    // connect Model::updateSprite to updateSpriteLabel to update the sprite label
    connect(&model,
            &Model::updateSprite,
            this,
            &MainWindow::updateSpriteLabels);

    currentPenColor = QColor(0, 0, 0); // black
    ui->colorLabel->setStyleSheet("background-color: " + currentPenColor.name());

    // Setting Pixmap to size of sprite
    p = QPixmap(SPRITE_SIZE * MULTIPLE_DYNAMIC, SPRITE_SIZE * MULTIPLE_DYNAMIC);
    // Ensuring it is a multiple of 32
    image = QImage(SPRITE_SIZE * MULTIPLE_DYNAMIC, SPRITE_SIZE * MULTIPLE_DYNAMIC, QImage::Format_RGB32);
    image.fill(qRgb(255, 255, 255));
    // Setting pixmap to be the image and ensuring size of label is the size of pixmap
    ui->spriteLabel->setPixmap(p.fromImage(image).scaled(SPRITE_SIZE * MULTIPLE_DYNAMIC, SPRITE_SIZE * MULTIPLE_DYNAMIC, Qt::KeepAspectRatio));
    ui->spriteLabel->setFixedSize(p.size());
    // set imagesLabel to be the a scaled version
    ui->imagesLabel->setPixmap(p.fromImage(image).scaled(pixelSize * MULTIPLE_DYNAMIC, pixelSize * MULTIPLE_DYNAMIC, Qt::KeepAspectRatio));
}

MainWindow::~MainWindow()
{
    delete ui;
}

/**
 * @brief Updates the sprite label to the imported sprite
 *
 * @param image imported sprite
 */
void MainWindow::updateSpriteLabels(QPixmap image)
{
    // the label needs to be scaled so the image is fully visible based on the model.pixelSize
    ui->spriteLabel->setPixmap(image.scaled(SPRITE_SIZE * MULTIPLE_DYNAMIC, SPRITE_SIZE * MULTIPLE_DYNAMIC));
    // update the animation preview label with the new QPixmap
    ui->imagesLabel->setPixmap(image.scaled(pixelSize * MULTIPLE_DYNAMIC, pixelSize * MULTIPLE_DYNAMIC));
}

void MainWindow::updateAnimation(QPixmap image)
{
    ui->imagesLabel->setPixmap(image.scaled(16 * MULTIPLE_DYNAMIC, 16 * MULTIPLE_DYNAMIC));
}

void MainWindow::mousePressEvent(QMouseEvent *event)
{
    //  MAP Coordinates for relative canvas rather than global application
    mousePos = event->pos();
    int x = mousePos.x();
    int y = mousePos.y();
    // start of change
    // off set the x and y
    int gridSize = RANGE / pixelSize;
    // Getting rid of remainder ensuring it is a multiple (i.e.-finding the cell)
    int xPixelPoint = (x - X_OFFSET) / gridSize;
    int yPixelPoint = (y - Y_OFFSET) / gridSize;
    ui->spriteLabel->setPixmap(p.fromImage(image));
    emit mousePressedSignal(xPixelPoint, yPixelPoint);
}

void MainWindow::colorsInPixel(int x, int y, QColor color)
{
    // Draw rectangle at point
    QPainter painter(&image);
    painter.setBrush(Qt::SolidPattern);
    QPen pen;
    QRect rect(x * MULTIPLE_DYNAMIC, y * MULTIPLE_DYNAMIC, MULTIPLE_DYNAMIC - 1, MULTIPLE_DYNAMIC - 1);
    pen.setColor(color);
    pen.setWidth(1);
    painter.setPen(pen);
    // pixelPoint*multipleDynamic now will snap to specific point
    painter.drawRect(rect);
    painter.fillRect(rect, color);
    painter.end();
    QPoint convertedCoordinates(x, y);
}

// ************************************************************ //
//                            SLOTS                             //
// ************************************************************ //

void MainWindow::on_previewButton_clicked()
{
    emit previewButtonClicked();
}

void MainWindow::on_loadButton_clicked()
{
    emit loadButtonClicked();
}

void MainWindow::on_saveButton_clicked()
{
    emit saveButtonClicked();
}

void MainWindow::on_exportPNGButton_clicked()
{
    emit exportPNGButtonClicked();
}

void MainWindow::on_invertColorButton_clicked()
{
    emit invertColorButtonClicked();
}

void MainWindow::on_newFrameButton_clicked()
{
    emit newFrameButtonClicked();
}

void MainWindow::on_clearButton_clicked()
{
    emit clearButtonClicked();
}

// when the fpsSlider is moved, the fpsLabel is updated to show the current fps in format "FPS: 10"
// emit the fpsChanged signal to update the fps in the model
void MainWindow::on_fpsSlider_valueChanged(int value)
{
    ui->fpsLabel->setText("FPS: " + QString::number(value));
}

// when the gridSizeSlider is moved, the gridSizeLabel is updated to show the current grid size in format "Grid Size: 10"
//  emit the gridSizeChanged signal to update the grid size in the model
void MainWindow::on_gridSizeSlider_valueChanged(int value)
{
    ui->gridSizeLabel->setText("Grid Size: " + QString::number(value) + "x" + QString::number(value));
    emit gridSizeChanged(value);
}

void MainWindow::on_colorButton_clicked()
{
    QColor newColor = QColorDialog::getColor();
    currentPenColor = newColor;
    qDebug() << currentPenColor;
    ui->colorLabel->setStyleSheet("background-color: " + currentPenColor.name());
    emit penColorChanged(currentPenColor);
}

void MainWindow::displaySprite(QPixmap spriteImage)
{
    ui->spriteLabel->setPixmap(spriteImage);
}

// user chose grid size 2x2
void MainWindow::on_twoGridSizeButton_clicked()
{
    pixelSize = 2;
        ui->gridSizeLabel->setText("Grid Size: " + QString::number(pixelSize) + "x" + QString::number(pixelSize));
    emit twoByTwoSignal(2); // sends the number two to the model
}

// user chose grid size 4x4
void MainWindow::on_fourGridSizeButton_clicked()
{
    pixelSize = 4;
        ui->gridSizeLabel->setText("Grid Size: " + QString::number(pixelSize) + "x" + QString::number(pixelSize));
    emit fourByFourSignal(4); // sends the number two to the model
}

// user chose grid size 8x8
void MainWindow::on_eightGridSizeButton_clicked()
{
    pixelSize = 8;
        ui->gridSizeLabel->setText("Grid Size: " + QString::number(pixelSize) + "x" + QString::number(pixelSize));
    emit eightByEightSignal(8);
}

// user chose grid size 16x16
void MainWindow::on_sixteenGridSizeButton_clicked()
{
    pixelSize = 16;
        ui->gridSizeLabel->setText("Grid Size: " + QString::number(pixelSize) + "x" + QString::number(pixelSize));
    emit sixteenBySixteenSignal(16);
}

// user chose grid size 32x32
void MainWindow::on_thirtyTwoGridSizeButton_clicked()
{
    pixelSize = 32;
        ui->gridSizeLabel->setText("Grid Size: " + QString::number(pixelSize) + "x" + QString::number(pixelSize));
    emit thirtyTwoByThirtyTwoSignal(32);
}

// shows rest of the UI and hides the grid layout
void MainWindow::showUI()
{
    // hides the grid size layout
    ui->verticalLayoutWidget->hide();

    // Show ALL OF THE BUTTONS now that a grid size was chosen
    ui->verticalLayoutWidget_5->show();
    ui->spriteLabel->show(); // where we color our spirte
    ui->verticalLayoutWidget_2->show(); // animationLayoutWidget
    ui->verticalLayoutWidget_3->show(); // colorLayout
    ui->verticalLayoutWidget_4->show(); // grid label layout
    ui->imagesLabel->show();
}

