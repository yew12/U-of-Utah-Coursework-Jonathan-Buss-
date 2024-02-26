/**
 * @file mainwindow.h
 * @author Tate Reynolds, Thatcher Geary, Sanjay Gounder, Gage Buss, Jonas K.
 * @brief This file contains the main window for the chess board
 * @version 0.1
 * @date 2022-12-08
 *
 * @copyright Copyright (c) 2022, U of U CS3505
 *
 */

#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include "chessmodel.h"
#include "chessview.h"
#include "chessboard.h"
#include "dialog.h"
#include "physicsworld.h"

#include <QMainWindow>
#include <QMediaPlayer>

QT_BEGIN_NAMESPACE
namespace Ui
{
    class MainWindow;
}
QT_END_NAMESPACE

/**
 * @brief The MainWindow class
 */
class MainWindow : public QMainWindow
{
    Q_OBJECT

public:
    MainWindow(QWidget *parent = nullptr);
    ~MainWindow();

signals:
    void tutorialMoveCompleted();
    void completionCheck(bool);

public slots:
    void viewClicked(const QPoint &field);
    void startButtonClicked();
    void tutorialButtonClicked();
    void getFrameNumber(int frameNumber);

    void askIfCompletedSlot();

private slots:
    void on_muteButton_toggled(bool checked);

private:
    Ui::MainWindow *ui;
    PhysicsWorld *physicsWorld;
    Dialog popUp;
    ChessView *_view;
    ChessModel *_algorithm;
    ChessBoard *_chessBoard = nullptr;
    QPoint _clickPoint;
    ChessView::FieldHighlight *_selectedField;

    bool tutorial = false;  // if we are on tutorial, this is true
    int tutorialNumber = 0; // 0 = pawn, 1 = rook, 2 = knight, 3 = bishop, 4 = queen, 5 = king
    QMediaPlayer *music;

    // tutorial helper methods
    void resetBoardTutorial();
};
#endif // MAINWINDOW_H
