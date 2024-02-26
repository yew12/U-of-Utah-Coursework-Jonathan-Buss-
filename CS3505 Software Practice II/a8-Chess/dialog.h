/**
 * @file dialog.h
 * @author Tate Reynolds, Thatcher Geary, Sanjay Gounder, Gage Buss, Jonas K.
 * @brief This file contains the tutorial dialog
 * @version 0.1
 * @date 2022-12-08
 *
 * @copyright Copyright (c) 2022, U of U CS3505
 *
 */

#ifndef DIALOG_H
#define DIALOG_H

#include <QDialog>
#include <QAbstractButton>
#include <QVector>
namespace Ui
{
    class Dialog;
}

/**
 * @brief The Dialog class
 */

class Dialog : public QDialog
{
    Q_OBJECT

public:
    explicit Dialog(QWidget *parent = nullptr);
    ~Dialog();
    bool completedTutorial;
signals:
    void tutorialStage(int frameNumber); // send frame number to mainwindow for tutorial logic checks
    void askForCompletion(); //sends signal to mainwindow to see get the boolean from chessboard on if a valid tutorial move has been made
public slots:
    void completionCheckSlot(bool); //gets the boolean from mainwindow (which gets the boolean from chessboard) indicating if a valid tutorial move has been made

private slots:
    void on_nextButton_clicked();

private:
    Ui::Dialog *ui;
    int frameNumber; // 0 = start of pawn explanation, 8 = start of king explanation. See switch-case for all the corresponding numbers in on_nextButton_clicked()

    void pawnExplanation();
    void movePawnInstruction2Spaces();
    void movePawnInstruction1Space();
    void movePawnAcrossBoard();

    void enPassant();
    void enPassant2();
    void enPassant3();
    void enPassantInstruction();

    void rookExplanation();
    void rookInstructionMove();
    void rookInstructionTake();

    void knightExplanation();
    void knightInstructionMove();
    void knightInstructionTake();

    void bishopExplanation();
    void bishopInstructionMove();
    void bishopInstructionTake();

    void queenExplanation();
    void queenInstructionBishopMove();
    void queenInstructionRookMove();

    void kingExplanation();
    void kingExplanation2();

    void castlingExplanation();
    void shortCastlingInstruction();
    void longCastlingInstruction();
};

#endif // DIALOG_H
