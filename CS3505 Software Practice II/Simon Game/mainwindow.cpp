#include "mainwindow.h"
#include "ui_mainwindow.h"
#include "model.h"
#include <cstdlib>
#include <iostream>

// interface - our "View"
MainWindow::MainWindow(Model& model, QWidget *parent)
    : QMainWindow(parent)
    , ui(new Ui::MainWindow)
{
    ui->setupUi(this);
    ui->continueButton->setVisible(false); // hide continue button
    ui->progressBar->setValue(0); // sets progress bar to 0
    disableGameButtons();
    milliseconds = 1000; // 1 second start

    // conenction for START button
    connect(ui->startButton, // connected to ui
            &QPushButton::clicked, // clicked signal
            &model, // pointer to model object
            &Model::startGame // our slot
            );

    // conenction for RED button
    connect(ui->redButton, // connected to ui
            &QPushButton::clicked, // clicked signal
            &model, // pointer to model object
            &Model::redButton // our slot
            );

    // conenction for BLUE button
    connect(ui->blueButton, // connected to ui
            &QPushButton::clicked, // clicked signal
            &model, // pointer to model object
            &Model::blueButton // our slot
            );

    // conenction for CONTINUE button
    connect(ui->continueButton, // connected to ui
            &QPushButton::clicked, // clicked signal
            &model, // pointer to model object
            &Model::continueButton // our slot
            );

    // connection that flashes RANDOM RED
    connect(&model,
            &Model::flashRedSignalRandomColor,
            this,
            &MainWindow::flashRedRandomColor
            );
    // connection that flashes RANDOM BLUE
    connect(&model,
            &Model::flashBlueSignalRandomColor,
            this,
            &MainWindow::flashBlueRandomColor
            );

    // connection that flashes SEQUENCE RED
    connect(&model,
            &Model::flashRedSignalSequenceColor,
            ui->redButton,
            &QPushButton::setStyleSheet
            );
    // connection that flashes SEQUENCE BLUE
    connect(&model,
            &Model::flashBlueSignalSequenceColor,
            ui->blueButton,
            &QPushButton::setStyleSheet
            );
    // SETS BUTTONS BACK TO DEFAULT
    connect(&model,
            &Model::defaultColorSignal,
            ui->blueButton,
            &QPushButton::setStyleSheet
            );
    // SETS BUTTONS BACK TO DEFAULT
    connect(&model,
            &Model::defaultColorSignal,
            ui->redButton,
            &QPushButton::setStyleSheet
            );

    // ends game if user inputs incorrect color
    connect(&model,
            &Model::wrongInputSignal,
            this,
            &MainWindow::wrongInput
            );


    connect(&model,
            &Model::moveToNextRoundSignal,
            this,
            &MainWindow::startNextRound
            );

    // disable continue button
    connect(&model,
            &Model::hideContinueButtonSignal,
            this,
            &MainWindow::hideContinueButton
            );

    // show continue button
    connect(&model,
            &Model::showYourTurnSignal,
            this,
            &MainWindow::showYourTurnLabel
            );

    // conenction for PROGRESS bar - RED BUTTON
    connect(ui->redButton, // connected to ui
            &QPushButton::clicked, // clicked signal
            &model, // pointer to model object
            &Model::incrementProgressBar // our slot
            );
    // conenction for PROGRESS bar - BLUE BUTTON
    connect(ui->blueButton, // connected to ui
            &QPushButton::clicked, // clicked signal
            &model, // pointer to model object
            &Model::incrementProgressBar // our slot
            );
    // update progress bar ui
    connect(&model,
            &Model::updateProgressBarSignal,
            ui->progressBar,
            &QProgressBar::setValue
            );
    // RESET progress bar ui
    connect(&model,
            &Model::resetProgressBarSignal,
            ui->progressBar,
            &QProgressBar::setValue
            );

    // swap ORIGINAL RED/BLUE position
    connect(&model,
            &Model::swapToOriginalSignal,
            this,
            &MainWindow::swapToOriginal
            );
    // swap OPPOSITE RED/BLUE position
    connect(&model,
            &Model::swapToOppositeSignal,
            this,
            &MainWindow::swapToOpposite
            );

}

MainWindow::~MainWindow()
{
    delete ui;
}

// SLOTS:

// RANDOM COLOR
void MainWindow::flashRedRandomColor()
{
//    milliseconds *= 0.97; // flashes faster each time
    ui->redButton->setStyleSheet(
                QString("QPushButton {background-color: rgb(200,50,50);}")
                );
    QTimer::singleShot(milliseconds+1000, this, SLOT(flashDefaultRandomColor()));
}

void MainWindow::flashBlueRandomColor()
{
//    milliseconds *= 0.97; // flashes faster each time
    ui->blueButton->setStyleSheet(
                QString("QPushButton {background-color: rgb(0,70,255);}")
                );
    QTimer::singleShot(milliseconds+1000, this, SLOT(flashDefaultRandomColor()));
}

// FLASH DEFAULT COLOR SLOT WHEN WE ARE ADDING A RANDOM COLOR TO SEQUENCE
void MainWindow::flashDefaultRandomColor()
{
    ui->blueButton->setStyleSheet(
               QString("QPushButton {background-color: rgb(180,180,180);}")
               );
    ui->redButton->setStyleSheet(
               QString("QPushButton {background-color: rgb(180,180,180);}")
               );

    enableGameButtons();
    hideStartButton();
    ui->turnLabel->setText("Your turn");
}

// SLOT FOR ENDING GAME WHEN YOU INCORRECTLY ENTER THE SEQUENCE
void MainWindow::wrongInput()
{
    disableGameButtons();
    ui->turnLabel->setText("Wrong input, GAME OVER!");
}

// SLOT FOR STARTING NEXT ROUND
void MainWindow::startNextRound()
{
    disableGameButtons();
    ui->continueButton->setVisible(true); // show continue button
}

// SLOT FOR OUR CUSTOM FEATURE SWAPPING BUTTONS AFTER EACH TURN
void MainWindow::swapToOriginal()
{
    ui->blueButton->setGeometry(460, 200, 251, 71); // blue button original position (starting)
    ui->redButton->setGeometry(110, 200, 241, 71); // red button original position (starting)
}

// SLOT FOR OUR CUSTOM FEATURE SWAPPING BUTTONS AFTER EACH TURN
void MainWindow::swapToOpposite()
{
    ui->redButton->setGeometry(460, 200, 251, 71); // position of original blue button
    ui->blueButton->setGeometry(110, 200, 241, 71); // position of original red button
}

// SLOT FOR HIDING THE CONTINUE BUTTON AFTER PRESSING IT
void MainWindow::hideContinueButton() {
    ui->continueButton->setVisible(false);
    ui->turnLabel->setText("Simon's turn");
}

// SLOT INDICATING IT IS YOUR TURN
void MainWindow::showYourTurnLabel()
{
     ui->turnLabel->setText("Your turn");
}

// HELPER METHODS:

void MainWindow::disableGameButtons()
{
    ui->redButton->setDisabled(true);
    ui->blueButton->setDisabled(true);
}

void MainWindow::enableGameButtons()
{
    ui->redButton->setDisabled(false);
    ui->blueButton->setDisabled(false);
}

void MainWindow::hideStartButton() {
    ui->startButton->setVisible(false);
}
