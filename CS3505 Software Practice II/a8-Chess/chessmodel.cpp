/**
 * @file chessmodel.cpp - backend for the chess board
 * @author Tate Reynolds, Thatcher Geary, Sanjay Gounder, Gage Buss, Jonas K.
 * @brief This file contains the backend for the chess board
 * @version 0.1
 * @date 2022-12-08
 *
 * @copyright Copyright (c) 2022, U of U CS3505
 *
 */

#include "ChessModel.h"

/**
 * @brief ChessModel::ChessModel initialies the algorithm for moving pieces
 * @param parent
 */
ChessModel::ChessModel(QObject *parent)
    : QObject{parent}
{
    _board = nullptr;
}

/**
 * @brief ChessModel::board - getter for board state in algorithm
 * @return the current board state
 */
ChessBoard *ChessModel::board() const
{
    return _board;
}

/**
 * @brief ChessModel::setBoard - setter for the board membere
 * @param board - new board the algorithm will use
 */
void ChessModel::setBoard(ChessBoard *board)
{
    if (board == _board)
    {
        return;
    }
    delete _board;
    _board = board;
    emit boardChanged(_board);
}

/**
 * @brief ChessModel::setupBoard sets up the board with a new chess board
 */
void ChessModel::setupBoard()
{
    setBoard(new ChessBoard(this, 8, 8));
}

void ChessModel::emptyBoard()
{
    board()->setFen("8/8/8/8/8/8/8/8 w KQkq - 0 1");
}

/**
 * @brief ChessModel::newGame - initializes the game with a FEN style string
 */
void ChessModel::newGame()
{
    setupBoard();
    board()->setFen(
        "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
}

//------------------------TUTORIALS------------------------//
/* The Below Methods Set the Chess board up for various outlines using FEN
    Notation. The FEN Notation is a string that describes the state of the
    chess board. The first part of the string describes the board state, the
    second part describes the side to move, the third part describes castling
    rights, the fourth part describes en passant rights, the fifth part describes
    the half move clock, and the sixth part describes the full move number. For
    more information on FEN Notation, see:
    https://en.wikipedia.org/wiki/Forsyth%E2%80%93Edwards_Notation
*/

void ChessModel::pawnTutorial()
{
    board()->setFen("8/8/8/8/8/8/4P3/8 w KQkq - 0 1");
}

void ChessModel::enPassantTutorial()
{
    board()->setFen("8/8/8/3Pp3/8/8/8/8 w KQkq - 0 1");
}

void ChessModel::pawnToQueenTutorial()
{
    board()->setFen("r4k1r/p1pP1ppp/2n5/Qp6/2b1nB2/6P1/PPP1PPP1/RN2KBNR w KQkq - 0 1");
}

void ChessModel::pawnTakePieceTutorial()
{
    board()->setFen("rnbqkbnr/pppp1ppp/8/4p3/3P4/8/PPP1PPPP/RNBQKBNR w KQkq - 0 1");
}

void ChessModel::rookTutorial()
{
    board()->setFen("8/8/8/8/8/8/8/7R w KQkq - 0 1");
}

void ChessModel::rookTakePieceTutorial()
{
    board()->setFen("8/8/8/4n2R/8/8/8/8 w KQkq - 0 1");
}

void ChessModel::knightTutorial()
{
    board()->setFen("8/8/8/8/8/8/8/6N1 w KQkq - 0 1");
}

void ChessModel::knightTakePieceTutorial()
{
    board()->setFen("8/8/8/3b4/5N2/8/8/8 w KQkq - 0 1");
}

void ChessModel::bishopTutorial()
{
    board()->setFen("8/8/8/8/8/8/8/7B w KQkq - 0 1");
}

void ChessModel::bishopTakePieceTutorial()
{
    board()->setFen("6r1/8/8/8/2B5/8/8/8 w KQkq - 0 1");
}

void ChessModel::queenTutorial()
{
    board()->setFen("8/8/8/8/8/8/8/3Q4 w KQkq - 0 1");
}

void ChessModel::queenTakePieceDiagonalTutorial()
{
    board()->setFen("8/8/8/1Q6/8/3n4/8/8 w KQkq - 0 1");
}

void ChessModel::queenTakePieceLateralTutorial()
{
    board()->setFen("8/8/8/1Q2n3/8/8/8/8 w KQkq - 0 1");
}

void ChessModel::kingShortCastle()
{
    board()->setFen("8/8/8/8/8/8/8/4K2R w KQkq - 0 1");
}

void ChessModel::kingLongCastle()
{
    board()->setFen("8/8/8/8/8/8/8/R3K3 w KQkq - 0 1");
}
