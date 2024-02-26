/**
 * @file chessboard.h - header file for the chessboard class
 * @author Tate Reynolds, Thatcher Geary, Sanjay Gounder, Gage Buss, Jonas K.
 * @brief  This file contains the header file for the chessboard class
 * @version 0.1
 * @date 2022-12-08
 *
 * @copyright Copyright (c) 2022, U of U CS3505
 *
 */

#ifndef CHESSBOARD_H
#define CHESSBOARD_H

#include <QObject>
#include <QPainter>

/**
 * @brief The ChessBoard class
 */
class ChessBoard : public QObject
{
    Q_OBJECT
    Q_PROPERTY(int ranks READ ranks NOTIFY ranksChanged)
    Q_PROPERTY(int columns READ columns NOTIFY columnsChanged)

private:
    int _ranks;
    int _columns;
    QVector<char> _boardData;
    // data structure for all the pieces
    QVector<QVector<char>> chessBoard;

    // all the booleans neccassary for chess

    int enPassant;
    int eRow;
    int eCol;
    bool checkNums(int col, int row);
    bool turn;

    bool wKingMove;
    bool bKingMove;
    bool wRRookMove;
    bool wLRookMove;
    bool bRRookMove;
    bool bLRookMove;

    int wKingRow;
    int wKingCol;
    int bKingRow;
    int bKingCol;

    bool kingPosCheck(bool white);

    bool chessMove(int fromColumn, int fromRank, int toColumn, int toRank);
    int blackOrWhite(char piece);
    bool pawnMove(int fromColumn, int fromRank, int toColumn, int toRank);
    bool knightMove(int fromColumn, int fromRank, int toColumn, int toRank);
    bool bishopMove(int fromColumn, int fromRank, int toColumn, int toRank);
    bool rookMove(int fromColumn, int fromRank, int toColumn, int toRank);
    bool queenMove(int fromColumn, int fromRank, int toColumn, int toRank);
    bool kingMove(int fromColumn, int fromRank, int toColumn, int toRank);
    void checkRookPosition(int fromCol, int fromRow);
    bool isAttacked(int col, int row, int color);
    bool checkIfBishopRookQueen(int col, int row, bool rook);
    bool castle(int color, int fromCol, int fromRow, int toCol, int toRow);
    void newBoard();
    bool tutorialInSession;
    int tutorialNum;
    bool tutorialMove(int fromColumn, int fromRank, int toColumn, int toRank);

public:
    explicit ChessBoard(QObject *parent = nullptr, int ranks = 8, int columns = 8);
    int ranks() const;
    int columns() const;
    ChessBoard *board() const;
    char data(int col, int rank) const;
    void setData(int col, int rank, char piece);
    void movePiece(int fromCol, int fromRank, int toCol, int toRank);
    void setFen(const QString &fen);
    bool complete;

signals:
    void ranksChanged(int);
    void columnsChanged(int);
    void dataChanged(int c, int r);
    void boardReset();


protected:
    void setRanks(int newRanks);
    void setColumns(int newColumns);
    void initBoard();
    bool setDataInternal(int col, int rank, char piece);
};

#endif // CHESSBOARD_H
