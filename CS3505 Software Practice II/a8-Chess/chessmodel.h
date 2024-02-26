/**
 * @file chessmodel.h - backend for the chess board
 * @author Tate Reynolds, Thatcher Geary, Sanjay Gounder, Gage Buss, Jonas K.
 * @brief  This file contains the backend for the chess board
 * @version 0.1
 * @date 2022-12-08
 *
 * @copyright Copyright (c) 2022 U of U CS3505
 *
 */

#ifndef CHESSMODEL_H
#define CHESSMODEL_H

#include <QObject>
#include <chessboard.h>

/**
 * @brief The ChessModel class
 */
class ChessModel : public QObject
{
    Q_OBJECT
private:
public:
    ChessBoard *board() const;
    ChessModel(QObject *parent);
    ChessBoard *_board;
public slots:
    virtual void newGame();
    // pawn tutorials
    virtual void pawnTutorial();
    virtual void enPassantTutorial();
    virtual void pawnToQueenTutorial();
    virtual void pawnTakePieceTutorial();

    // rook tutorials
    virtual void rookTutorial();
    virtual void rookTakePieceTutorial();

    // knight tutorials
    virtual void knightTutorial();
    virtual void knightTakePieceTutorial();

    // bishop tutorials
    virtual void bishopTutorial();
    virtual void bishopTakePieceTutorial();

    // queen tutorials
    virtual void queenTutorial();
    virtual void queenTakePieceDiagonalTutorial();
    virtual void queenTakePieceLateralTutorial();

    // king tutorials
    virtual void kingShortCastle();
    virtual void kingLongCastle();

    virtual void setupBoard();
    virtual void emptyBoard();
signals:
    void boardChanged(ChessBoard *);

protected:
    void setBoard(ChessBoard *board);
};

#endif
