/**
 * @file dialog.cpp - tutorial dialog
 * @author your name (you@domain.com)
 * @brief  This file contains the tutorial dialog
 * @version 0.1
 * @date 2022-12-08
 *
 * @copyright Copyright (c) 2022, U of U CS3505
 *
 */

#include "dialog.h"
#include "ui_dialog.h"
#include <iostream>

Dialog::Dialog(QWidget *parent) : QDialog(parent),
                                  ui(new Ui::Dialog)
{
    ui->setupUi(this);

    // Making text only read only for the QTextEdit
    ui->textEdit->setReadOnly(true);
    // Hiding text background and frame for the QTextEdit
    ui->textEdit->viewport()->setAutoFillBackground(false);
    ui->textEdit->setFrameStyle(QFrame::NoFrame);
    // Making label font bold
    ui->description->setStyleSheet("font-weight: bold; color: black");

    frameNumber = -1;
    completedTutorial = true;
}

Dialog::~Dialog()
{
    delete ui;
}

/**
 * @brief When the tutorial is complete, this function will be called to set the completedTutorial bool to true
 * @param isComplete
 */
void Dialog::completionCheckSlot(bool isComplete)
{
    completedTutorial = isComplete;
}

/**
 * @brief when the next button is clicked, this function will be called. It will increment the frameNumber and call the corresponding function to show the next frame
 *
 */
void Dialog::on_nextButton_clicked()
{

    emit askForCompletion();
    if (frameNumber <= 1 || (frameNumber <= 7 && frameNumber > 4))
        completedTutorial = true;

    if (!completedTutorial)
        return;

    // frameNumber is a private global int tracked only inside dialog.cpp. It will increment up and be used in the switch-case to show a certain frame.

    // Switch-case statement for jumping to the various 'frames' of the tutorial
    // 0 (frameNumber)  = pawn, goes up from there to 29 (frameNumber) = long castling explanation. 30 (frameNumber) will result in closing the tutorial window
    if (frameNumber == 25)
        this->hide();

    switch (frameNumber)
    {
    case 0:
        pawnExplanation();
        break;
    case 1:
        movePawnInstruction2Spaces();
        break;
    case 2:
        movePawnInstruction1Space();
        break;
    case 3:
        movePawnAcrossBoard();
        break;
    case 4:
        enPassant();
        break;
    case 5:
        enPassant2();
        break;
    case 6:
        enPassant3();
        break;
    case 7:
        enPassantInstruction();
        break;
    case 8:
        rookExplanation();
        break;
    case 9:
        rookInstructionMove();
        break;
    case 10:
        rookInstructionTake();
        break;
    case 11:
        knightExplanation();
        break;
    case 12:
        knightInstructionMove();
        break;
    case 13:
        knightInstructionTake();
        break;
    case 14:
        bishopExplanation();
        break;
    case 15:
        bishopInstructionMove();
        break;
    case 16:
        bishopInstructionTake();
        break;
    case 17:
        queenExplanation();
        break;
    case 18:
        queenInstructionBishopMove();
        break;
    case 19:
        queenInstructionRookMove();
        break;
    case 20:
        kingExplanation();
        break;
    case 21:
        kingExplanation2();
        break;
    case 22:
        castlingExplanation();
        break;
    case 23:
        shortCastlingInstruction();
        break;
    case 24:
        longCastlingInstruction();
        break;
    default:
        break;
    }

    // once all the cases have been done, frameNumber will be 30, we have no need for this tutorial and it will go away now.

    frameNumber++;
    emit tutorialStage(frameNumber);

    // go to next frame
}

/**
 * @brief This function will show the pawn explanation frame
 *
 */
void Dialog::pawnExplanation()
{
    ui->description->setText("Pawn");
    ui->textEdit->setText("On a pawn's first move, it can move up 1 or 2 spaces. After its first move, it can only move up 1 space."
                          " To take a piece, a pawn must move diagonally 1 up to the right or left. If you advance a pawn all the way to the other side of the board,"
                          " you are rewarded by getting to replace that pawn with a queen.");
}

/**
 * @brief This function will show the pawn move up 2 spaces frame
 *
 */
void Dialog::movePawnInstruction2Spaces()
{
    ui->textEdit->setText("Try moving a pawn up 2 on start.");
    //    ui->nextButton->setEnabled(false); //cannot click 'next' until the pawn is moved up 2 spaces
}

/**
 * @brief This function will show the pawn move up 1 space frame
 *
 */
void Dialog::movePawnInstruction1Space()
{
    ui->textEdit->setText("Try moving a pawn up 1 on start.");
    //    ui->nextButton->setEnabled(false); //cannot click 'next' until the pawn is moved up 1 spaces
}

/**
 * @brief This function will show the pawn move up the board frame
 *
 */
void Dialog::movePawnAcrossBoard()
{
    ui->textEdit->setText("The pawn is nearly at the end of the board! Move it up the board to change it to a queen.");
}

/**
 * @brief This function will show the en passant explanation frame
 *
 */
void Dialog::enPassant()
{
    ui->description->setText("Pawn: En\nPassant");
    // En Passant explanation: https://www.chess.com/terms/en-passant
    ui->textEdit->setText("The en passant rule is a special pawn capturing move in chess. Pawns can usually capture only pieces that are directly and diagonally in "
                          "front of them on an adjacent file. It moves to the captured piece's square and replaces it. With en passant, though, things are a little different. "
                          "Click 'next' to read more on the en passant.");
}

/**
 * @brief This function will show the en passant explanation frame
 *
 */
void Dialog::enPassant2()
{
    // En Passant explanation: https://www.chess.com/terms/en-passant
    ui->textEdit->setText("To perform this capture, you must take your opponent's pawn as if it had moved just one square. "
                          "You move your pawn diagonally to an adjacent square, one rank farther from where it had been, on the same file where the enemy's pawn is, "
                          "and remove the opponent's pawn from the board.");
}

/**
 * @brief   This function will show the en passant explanation frame
 *
 */
void Dialog::enPassant3()
{
    // En Passant explanation: https://www.chess.com/terms/en-passant
    ui->textEdit->setText("There are a few requirements for the move to be legal:\n"
                          "1. The capturing pawn must have advanced exactly three ranks to perform this move.\n"
                          "2. The captured pawn must have moved two squares in one move, landing right next to the capturing pawn.\n"
                          "3. The en passant capture must be performed on the turn immediately after the pawn being captured moves. If the player does not capture en passant on that turn, "
                          "they no longer can do it later.\n"
                          "This type of capture cannot happen if the capturing pawn has already advanced four or more squares. "
                          "Another instance where this capture is not allowed is when the enemy pawn lands right next to your pawn but only after making two moves.");
}

/**
 * @brief This function will show the en passant instruction frame
 *
 */
void Dialog::enPassantInstruction()
{
    ui->textEdit->setText("Try performing the en passant. Try taking the piece on the right by moving the white pawn to the square behind the right pawn. ");
}

/**
 * @brief This function will show the knight explanation frame
 *
 */
void Dialog::rookExplanation()
{
    ui->description->setText("Rook");
    ui->textEdit->setText("Rooks may move any direction as long as it is within the column or row they are in. They can go as far up, down, left, or"
                          "right unless there is a piece blocking the full way. To take a piece, simply move the rook over the piece desired to take so long as they are within"
                          "the same row/column the rook is in.");
}

/**
 * @brief This function will show the rook instruction frame
 *
 */
void Dialog::rookInstructionMove()
{
    ui->textEdit->setText("Try moving the rook one of the directions.");
}

/**
 * @brief This function will show the rook instruction frame
 *
 */
void Dialog::rookInstructionTake()
{
    ui->textEdit->setText("Try taking a piece with the rook.");
}

/**
 * @brief This function will show the knight explanation frame
 *
 */
void Dialog::knightExplanation()
{
    ui->description->setText("Knight");
    ui->textEdit->setText("Knights have a unique ability to jump other pieces. They may move in 'L-shaped' movements. It may move either two"
                          "squares vertically and one square horizontally, or two squares horizontally and one square vertically. It is in these movements, knights can jump"
                          "pieces in front or behind it. To take a piece, simply move the knight in the legal L-shaped movement, and if there is a piece at the end result"
                          "of the move, the knight will move on top of it and take it. Try moving a knight various ways, as well as taking a piece.");
}

/**
 * @brief This function will show the knight instruction frame
 *
 */
void Dialog::knightInstructionMove()
{
    ui->textEdit->setText("Try moving the knight in an L-shape.");
}

/**
 * @brief This function will show the knight instruction frame
 *
 */
void Dialog::knightInstructionTake()
{
    ui->textEdit->setText("Try taking a piece with the knight.");
}

/**
 * @brief This function will show the bishop explanation frame
 *
 */
void Dialog::bishopExplanation()
{
    ui->description->setText("Bishop");
    ui->textEdit->setText("Bishops may move diagonally on the color they are on. They may go as far up or down the board diagonally unless"
                          "there is a piece blocking the full way. To take a piece, simply move the bishop over the piece desired so long as they are within the same direction "
                          "diagonally. Try moving a bishop various directions, as well as taking a piece.");
}

/**
 * @brief   This function will show the bishop instruction frame
 *
 */
void Dialog::bishopInstructionMove()
{
    ui->textEdit->setText("Try moving the bishop diagonally to the other edge of the board.");
}

/**
 * @brief  This function will show the bishop instruction frame
 *
 */
void Dialog::bishopInstructionTake()
{
    ui->textEdit->setText("Try taking a piece with the bishop.");
}

/**
 * @brief This function will show the queen explanation frame
 *
 */
void Dialog::queenExplanation()
{
    ui->description->setText("Queen");
    ui->textEdit->setText("Queens have the ability to make the moves of every other piece we have talked about, with the exception"
                          "of a pawn. To take a piece, simply do one of the many valid movements a queen is allowed and move over a piece. Try moving a queen various"
                          "ways, as well as taking a piece.");
}

/**
 * @brief This function will show the queen instruction frame
 *
 */
void Dialog::queenInstructionBishopMove()
{
    ui->textEdit->setText("Move the queen like a bishop, diagonally to the right edge.");
}

/**
 * @brief This function will show the queen instruction frame
 *
 */
void Dialog::queenInstructionRookMove()
{
    ui->textEdit->setText("Move the queen like a rook, laterally.");
}

/**
 * @brief This function will show the king explanation frame
 *
 */
void Dialog::kingExplanation()
{
    ui->description->setText("King");
    // King Explanation: https://en.wikipedia.org/wiki/King_(chess)#:~:text=The%20king%20(%E2%99%94%2C%20%E2%99%9A),capture%20on%20the%20next%20move.
    ui->textEdit->setText("To win/lose a game depends on the king. It is the most imporant piece in chess. Kings may move to any"
                          "adjoining square. It may also perform a move known as castling.");
    ui->nextButton->setText("Continue");
}

/**
 * @brief   This function will show the king explanation frame
 *
 */
void Dialog::kingExplanation2()
{
    // King Explanation: https://en.wikipedia.org/wiki/King_(chess)#:~:text=The%20king%20(%E2%99%94%2C%20%E2%99%9A),capture%20on%20the%20next%20move.
    ui->textEdit->setText("If a player's king is threatened with capture, "
                          "it is said to be in check, and the player must remove the threat of capture on the next move. If this cannot be done, the king is said to "
                          "be in checkmate, resulting in a loss for that player.");
}

/**
 * @brief This function will show the king instruction frame
 *
 */
void Dialog::castlingExplanation()
{
    ui->description->setText("King: Castling");
    // King Explanation: https://en.wikipedia.org/wiki/King_(chess)#:~:text=The%20king%20(%E2%99%94%2C%20%E2%99%9A),capture%20on%20the%20next%20move.
    ui->textEdit->setText("Castling is when you can swap the places of your king and rook. "
                          "It is very useful in games because you can protect your king and make your rook active in only one move!"
                          "Here are the conditions:\n"
                          "1. Once you move your king you may not castle.(even if you move it back to the original place).\n"
                          "2. Once you move your rook you cannot castle with that side of the rook.(even if you move it back to the original place).\n"
                          "3. The squares between your king and rook that you are castling with cannot have any pieces in between them.\n"
                          "4. Your king is not in check.\n"
                          "5. The squares between your king and rook that you are castling with cannot be attacked ie the next move the enemy cannot move/attack their piece at that square.\n"
                          "6. On the right side of the board(from white pov) you can put your king on g1 if white, g8 if black and rook to f1 for white and f8 for black.\n"
                          "7. On the left side of the board (from white pov) you can put your king on c1 if white, c8 if black and rook to d1 for white and d8 for black.");
    ui->nextButton->setText("Done");
}

/**
 * @brief   This function will show the king instruction frame
 *
 */
void Dialog::shortCastlingInstruction()
{
    ui->textEdit->setText("Perform the short castling movement (Move the king to g1).");
}

/**
 * @brief  This function will show the king instruction frame
 *
 */

void Dialog::longCastlingInstruction()
{
    ui->textEdit->setText("Perform the long castling movement (Move the king to c1).");
}
