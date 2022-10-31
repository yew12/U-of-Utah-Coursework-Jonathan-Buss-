#include "model.h"
#include <iostream>

// Model class -The job of the model is to hold the data and game logic
Model::Model(QObject *parent)
    : QObject{parent}
{
    patternIndex = 0;      // index for where our pattern is at while traversing our patternVector
    progressPercent = 0;   // tracking percent of progress bar which is full
    switchButtonCount = 0; // odd # original red/blue button location | even # switched location
}

void Model::startGame()
{
    flashRandomColor();
}

void Model::redButton() {

    if(patternVector[patternIndex] == 0) {
        // if we reach end of sequence, move to next round
        if(patternIndex == patternVector.size() - 1)
            emit moveToNextRoundSignal();

        patternIndex++; // correct input go to next index in patternVector
    } else {
        emit wrongInputSignal();
    }
}

void Model::blueButton() {

    if(patternVector[patternIndex] == 1) {
        // if we reach end of sequence, move to next round
        if(patternIndex == patternVector.size() - 1)
            emit moveToNextRoundSignal();

        patternIndex++; // correct input
    } else {
        emit wrongInputSignal();
    }
}

void Model::continueButton()
{
    runPreviousSequence();
    //reset pattern index
    patternIndex = 0;
    emit hideContinueButtonSignal();
    // reset progress bar
    emit resetProgressBarSignal(0);
    // starts at odd, will switch button after first click and continously switch from here on
    switchButtonCount++;
    // odd - so we swap FROM original location (opposite)
    if(switchButtonCount % 2 == 1) {
        emit swapToOppositeSignal();
    } else {
        // even - swap back TO original location (original)
        emit swapToOriginalSignal();
    }
}

void Model::flashRandomColor()
{
    int randomColor = arc4random() % 2; // chooses random colors between 0 and 1

    if(randomColor == 0) // flash RED
    {
        emit flashRedSignalRandomColor();
        patternVector.push_back(0);
    } else {
        emit flashBlueSignalRandomColor();
        patternVector.push_back(1);
    }

}

void Model::runPreviousSequence()
{
    gameSpeed = 1000;
    // run a for loop to flash previous sequence of colors
    for(int i = 0; i < patternVector.size(); i++) {

        gameSpeed = (gameSpeed + 1500) - 500; //add to gamespeed (plus 1500) for future singleShots while speeding it up (subtracting 500)
        if(patternVector[i] == 0) {
            QTimer::singleShot(gameSpeed, this, SLOT(emitRedButtonSignal()));
            QTimer::singleShot(gameSpeed + 600, this, SLOT(emitDefaultColorSignal())); //queue'd up for 600 milliseconds after line 86
        } else {
            QTimer::singleShot(gameSpeed, this, SLOT(emitBlueButtonSignal()));
            QTimer::singleShot(gameSpeed + 600, this, SLOT(emitDefaultColorSignal())); //queue'd up for 600 milliseconds after line 86
        }
    }

    // then flash a new random color;
    QTimer::singleShot(gameSpeed + 1600, this, SLOT(flashRandomColor()));
    QTimer::singleShot(gameSpeed + 2600, this, SLOT(emitYourTurnSignal()));
}

void Model::incrementProgressBar() {
    progressPercent = (patternIndex * 100) / patternVector.size();
    emit updateProgressBarSignal(progressPercent);
}

void Model::emitYourTurnSignal()
{
    emit showYourTurnSignal();
}

void Model::emitRedButtonSignal()
{
    emit flashRedSignalSequenceColor(
                QString("QPushButton {background-color: rgb(200,50,50);}"));
}


void Model::emitBlueButtonSignal()
{
    emit flashBlueSignalSequenceColor(
                QString("QPushButton {background-color: rgb(0,70,255);}"));
}


void Model::emitDefaultColorSignal()
{
    emit defaultColorSignal(
                QString("QPushButton {background-color: rgb(180,180,180);}"));
}
