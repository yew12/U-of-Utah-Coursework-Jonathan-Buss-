#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include "model.h"


QT_BEGIN_NAMESPACE
namespace Ui { class MainWindow; }
QT_END_NAMESPACE

class MainWindow : public QMainWindow
{
    Q_OBJECT

public:
    QTimer *timer;
    QTimer *continueTimer;
    MainWindow(Model& model, QWidget *parent = nullptr);
    ~MainWindow();
    void disableGameButtons();
    void enableGameButtons();
    void hideStartButton();

public slots:
    // RNADOM COLOR SIGNAL
    void flashRedRandomColor(); // when called it should flash red
    void flashBlueRandomColor(); //  when called it should flash blue
    void flashDefaultRandomColor();    
    void hideContinueButton();
    void showYourTurnLabel();

    void wrongInput();
    void startNextRound(); // updates UI after each round to allow user to continue game
    void swapToOriginal(); // sets Red/Blue buttons to original positions
    void swapToOpposite(); // set Red/Blue buttons to opposite positions

private:
    Ui::MainWindow *ui;
    int milliseconds; // will continually get smaller




};
#endif // MAINWINDOW_H
