#ifndef MODEL_H
#define MODEL_H

#include <QObject>
#include <QTimer>
#include <QVector>

class Model: public QObject
{
    Q_OBJECT
public:
   explicit Model(QObject *parent = nullptr);
   QVector<int> patternVector; // 0 = red || 1 = blue
   QTimer *timer;
   int progressPercent;

public slots:
    void startGame();
    void redButton();
    void blueButton();
    void continueButton();
    void emitBlueButtonSignal();
    void emitRedButtonSignal();
    void emitDefaultColorSignal();
    void flashRandomColor();
    void emitYourTurnSignal();
    void incrementProgressBar();
signals:
    // RANDOM SIGNALS
    void flashRedSignalRandomColor();
    void flashBlueSignalRandomColor();
    // SEQUENCE SIGNALS
    void flashRedSignalSequenceColor(QString);
    void flashBlueSignalSequenceColor(QString);
    void defaultColorSignal(QString);
    void wrongInputSignal(); //signal for when user inputs wrong color
    void moveToNextRoundSignal();

    // GUI UPDATES
    void showYourTurnSignal();
    void hideContinueButtonSignal();
    void updateProgressBarSignal(int);
    void resetProgressBarSignal(int);

    // CUSTOM IMPLEMENTATION
    void swapToOppositeSignal();
    void swapToOriginalSignal();

private:
   int patternIndex; // keeps track of index we are at in vector
   int gameSpeed; // our game speed variable
   void runPreviousSequence();
   int switchButtonCount;


};

#endif // MODEL_H
