/**
 * @file chessboard.cpp - implementation of the chessboard class
 * @author Tate Reynolds, Thatcher Geary, Sanjay Gounder, Gage Buss, Jonas K.
 * @brief  Implementation of the chessboard class, represents ranks, columns, and the board data
 * @version 0.1
 * @date 2022-12-08
 *
 * @copyright Copyright (c) 2022, U of U CS3505
 *
 */

#include "chessboard.h"
#include <iostream>

/**
 * @brief ChessBoard::ChessBoard
 * @param parent
 * @param ranks - i.e. chess lingo for rows
 * @param columns
 */
ChessBoard::ChessBoard(QObject *parent, int ranks, int columns)
    : QObject{parent}
{
    _ranks = ranks;
    _columns = columns;
    initBoard();
    tutorialNum = 0;
    tutorialInSession = false;
    complete = false;
}

/**
 * @brief ChessBoard::ranks - getter for ranks
 * @return ranks
 */
int ChessBoard::ranks() const
{
    return _ranks;
}

/**
 * @brief ChessBoard::columns - getter for columns
 * @return columns
 */
int ChessBoard::columns() const
{
    return _columns;
}

/**
 * @brief ChessBoard::setRanks - setter for ranks
 * @param newRanks - new ranks to be entered
 */
void ChessBoard::setRanks(int newRanks)
{
    if (ranks() == newRanks)
    {
        return;
    }
    _ranks = newRanks;
    initBoard();
    emit ranksChanged(_ranks);
}

/**
 * @brief ChessBoard::setColumns - setter for columns
 * @param newColumns
 */
void ChessBoard::setColumns(int newColumns)
{
    if (columns() == newColumns)
    {
        return;
    }
    _columns = newColumns;
    initBoard();
    emit columnsChanged(_columns);
}

/**
 * @brief ChessBoard::initBoard - initializes the board with ranks and columns (empty board)
 */
void ChessBoard::initBoard()
{
    _boardData.fill(' ', ranks() * columns());
    // set the chessBoard
    chessBoard = QVector<QVector<char>>(8, QVector<char>(8, ' '));
    emit boardReset();
}

/**
 * @brief ChessBoard::data - gets data at a specified rank and col (i.e. piece at A4)
 * @param column
 * @param rank
 * @return the piece char at this space
 */
char ChessBoard::data(int column, int rank) const
{
    return _boardData.at((rank - 1) * columns() + (column - 1));
}

/**
 * @brief ChessBoard::setData - sets the data at a specified rank and col
 * @param column
 * @param rank
 * @param value - char piece to be entered
 */
void ChessBoard::setData(int column, int rank, char value)
{
    if (setDataInternal(column, rank, value))
    {
        emit dataChanged(column, rank);
    }
}

/**
 * @brief ChessBoard::setDataInternal - internal tool which reads and writes data without exposing private members
 * @param column
 * @param rank
 * @param value - piece char
 * @return true if the data is new and should be set, false otherwise
 */
bool ChessBoard::setDataInternal(int column, int rank, char value)
{
    int index = (rank - 1) * columns() + (column - 1);
    if (_boardData.at(index) == value)
    {
        return false;
    }
    _boardData[index] = value;
    chessBoard[8 - rank][column - 1] = value;
    return true;
}

/**
 * @brief ChessBoard::movePiece - moves a piece from a space to a space checks the validity of the move with chessMove method
 * @param fromColumn
 * @param fromRank
 * @param toColumn
 * @param toRank
 */
void ChessBoard::movePiece(int fromColumn, int fromRank, int toColumn, int toRank)
{
    if(complete)
        return;
    if (!chessMove(fromColumn - 1, 8 - fromRank,
                   toColumn - 1, 8 - toRank))
        return;

    if (tutorialInSession && (tutorialNum == 14 || tutorialNum == 13))
        return;

    setData(toColumn, toRank, data(fromColumn, fromRank));
    setData(fromColumn, fromRank, ' ');
    if (data(toColumn, toRank) == 'p' || data(toColumn, toRank) == 'P')
    {
        if (8 - toRank == 7)
            setData(toColumn, toRank, 'q');
        if (8 - toRank == 0)
            setData(toColumn, toRank, 'Q');
    }
    enPassant--;
    turn = !turn;
}

/**
 * @brief ChessBoard::chessMove - main method for the logic of each piece
 * @param fromCol
 * @param fromRow
 * @param toCol
 * @param toRow
 * @return true if the proposed move is valid
 */
bool ChessBoard::chessMove(int fromCol, int fromRow, int toCol, int toRow)
{
    char piece = chessBoard[fromRow][fromCol];
    int color = blackOrWhite(piece);
    char p = chessBoard[toRow][toCol];
    if (tutorialInSession)
    {
        if(tutorialMove(fromCol, fromRow, toCol, toRow))
            complete = true;
        return complete;
    }
    if (color == blackOrWhite(p))
        return false;

    if ((turn && blackOrWhite(piece) == 1) || (!turn && blackOrWhite(piece) == -1))
        return false;

    if (color == 1)
    {
        if (piece == 'P' && fromRow - 1 == toRow && (fromCol - 1 == toCol || fromCol + 1 == toCol) && eCol == toCol && eRow == toRow - 1 && p == ' ')
        {
            if (fromCol - 1 == toCol)
            {
                chessBoard[fromRow][fromCol] = ' ';
                chessBoard[fromRow][fromCol - 1] = ' ';
                chessBoard[fromRow - 1][fromCol - 1] = 'P';
                if (!kingPosCheck(true) && isAttacked(wKingCol, wKingRow, color))
                {
                    chessBoard[fromRow][fromCol] = 'P';
                    chessBoard[fromRow][fromCol - 1] = 'p';
                    chessBoard[fromRow - 1][fromCol - 1] = ' ';
                    return false;
                }
            }
            else
            {
                chessBoard[fromRow][fromCol] = ' ';
                chessBoard[fromRow][fromCol + 1] = ' ';
                chessBoard[fromRow - 1][fromCol + 1] = 'P';
                if (!kingPosCheck(true) && isAttacked(wKingCol, wKingRow, color))
                {
                    chessBoard[fromRow][fromCol] = 'P';
                    chessBoard[fromRow][fromCol + 1] = 'p';
                    chessBoard[fromRow - 1][fromCol + 1] = ' ';
                    return false;
                }
            }
        }
        else if (piece == 'K')
        {
            chessBoard[fromRow][fromCol] = ' ';
            if (isAttacked(toCol, toRow, color))
            {
                chessBoard[fromRow][fromCol] = 'K';
                return false;
            }
            chessBoard[fromRow][fromCol] = 'K';
        }
        else
        {
            chessBoard[toRow][toCol] = piece;
            chessBoard[fromRow][fromCol] = ' ';
            if (isAttacked(wKingCol, wKingRow, color))
            {
                chessBoard[toRow][toCol] = p;
                chessBoard[fromRow][fromCol] = piece;
                return false;
            }

            chessBoard[toRow][toCol] = p;
            chessBoard[fromRow][fromCol] = piece;
        }
    }
    if (color == -1)
    {

        if (piece == 'p' && fromRow + 1 == toRow && (fromCol - 1 == toCol || fromCol + 1 == toCol) && eCol == toCol && eRow == toRow + 1 && p == ' ')
        {
            if (fromCol - 1 == toCol)
            {
                chessBoard[fromRow][fromCol] = ' ';
                chessBoard[fromRow][fromCol - 1] = ' ';
                chessBoard[fromRow + 1][fromCol - 1] = 'p';
                if (!kingPosCheck(false) && isAttacked(bKingCol, bKingRow, color))
                {
                    chessBoard[fromRow][fromCol] = 'p';
                    chessBoard[fromRow][fromCol - 1] = 'p';
                    chessBoard[fromRow + 1][fromCol - 1] = ' ';
                    return false;
                }
            }
            else
            {
                chessBoard[fromRow][fromCol] = ' ';
                chessBoard[fromRow][fromCol + 1] = ' ';
                chessBoard[fromRow + 1][fromCol + 1] = 'p';
                if (!kingPosCheck(false) && isAttacked(bKingCol, bKingRow, color))
                {
                    chessBoard[fromRow][fromCol] = 'p';
                    chessBoard[fromRow][fromCol + 1] = 'p';
                    chessBoard[fromRow + 1][fromCol + 1] = ' ';
                    return false;
                }
            }
        }
        else if (piece == 'k')
        {
            chessBoard[fromRow][fromCol] = ' ';
            if (isAttacked(toCol, toRow, color))
            {
                chessBoard[fromRow][fromCol] = 'k';
                return false;
            }
            chessBoard[fromRow][fromCol] = 'k';
        }

        else
        {
            chessBoard[toRow][toCol] = piece;
            chessBoard[fromRow][fromCol] = ' ';
            if (isAttacked(bKingCol, bKingRow, color))
            {
                chessBoard[toRow][toCol] = p;
                chessBoard[fromRow][fromCol] = piece;
                return false;
            }

            chessBoard[toRow][toCol] = p;
            chessBoard[fromRow][fromCol] = piece;
        }
    }
    if (piece == 'p' || piece == 'P')
        return pawnMove(fromCol, fromRow, toCol, toRow);

    if (piece == 'n' || piece == 'N')
        return knightMove(fromCol, fromRow, toCol, toRow);

    if (piece == 'b' || piece == 'B')
        return bishopMove(fromCol, fromRow, toCol, toRow);

    if (piece == 'r' || piece == 'R')
        return rookMove(fromCol, fromRow, toCol, toRow);

    if (piece == 'q' || piece == 'Q')
        return queenMove(fromCol, fromRow, toCol, toRow);

    if (piece == 'k' || piece == 'K')
        return kingMove(fromCol, fromRow, toCol, toRow);

    return false;
}

/**
 * @brief ChessBoard::checkNums checks if the two numbers are valid rows and cols
 * @param col
 * @param row
 * @return returns true if both are valid
 */
bool ChessBoard::checkNums(int col, int row)
{
    return col < 8 && col > -1 && row > -1 && row < 8;
}

/**
 * @brief ChessBoard::pawnMove checks the possible moves for pawn
 * @param fromCol orginal column
 * @param fromRow original row
 * @param toCol   potential new column
 * @param toRow   potential new row
 * @return true if the move was valid false otherwise.
 */
bool ChessBoard::pawnMove(int fromCol, int fromRow, int toCol, int toRow)
{
    int color = blackOrWhite(chessBoard[fromRow][fromCol]);
    // black
    if (color == -1)
    {
        // special case enpeasant read rules
        if (enPassant == 1 && chessBoard[toRow][toCol] == ' ')
        {
            if (fromRow == eRow && (fromCol + 1 == eCol || fromCol - 1 == eCol))
            {
                if (toCol == eCol && toRow == fromRow + 1)
                {
                    setData(eCol + 1, 4, ' ');
                    return true;
                }
            }
        }
        // moving up
        if (blackOrWhite(chessBoard[fromRow + 1][fromCol]) == 0 && toCol == fromCol && toRow == fromRow + 1)
            return true;
        // special case moving two squares
        if (fromRow == 1 && blackOrWhite(chessBoard[fromRow + 1][fromCol]) == 0 && blackOrWhite(chessBoard[fromRow + 2][fromCol]) == 0 && toCol == fromCol && toRow == fromRow + 2)
        {
            enPassant = 2;
            eRow = fromRow + 2;
            eCol = fromCol;
            return true;
        }

        if (fromCol + 1 < 8 && (blackOrWhite(chessBoard[fromRow + 1][fromCol + 1]) == 1 && toCol == fromCol + 1 && toRow == fromRow + 1))
            return true;
        if (fromCol - 1 > -1 && (blackOrWhite(chessBoard[fromRow + 1][fromCol - 1]) == 1 && toCol == fromCol - 1 && toRow == fromRow + 1))
            return true;
    }

    else
    {
        // special case enpeasant read rules
        if (enPassant == 1 && chessBoard[toRow][toCol] == ' ')
        {
            if (fromRow == eRow && (fromCol + 1 == eCol || fromCol - 1 == eCol))
            {
                if (toCol == eCol && toRow == fromRow - 1)
                {
                    setData(eCol + 1, 5, ' ');
                    return true;
                }
            }
        }
        // moving up
        if (blackOrWhite(chessBoard[fromRow - 1][fromCol]) == 0 && toCol == fromCol && toRow == fromRow - 1)
            return true;
        // special case moving two squares
        if (fromRow == 6 && blackOrWhite(chessBoard[fromRow - 1][fromCol]) == 0 && blackOrWhite(chessBoard[fromRow - 2][fromCol]) == 0 && toCol == fromCol && toRow == fromRow - 2)
        {
            enPassant = 2;
            eRow = fromRow - 2;
            eCol = fromCol;
            return true;
        }
        if (fromCol + 1 < 8 && (blackOrWhite(chessBoard[fromRow - 1][fromCol + 1]) == -1 && toCol == fromCol + 1 && toRow == fromRow - 1))
            return true;
        if (fromCol - 1 > -1 && (blackOrWhite(chessBoard[fromRow - 1][fromCol - 1]) == -1 && toCol == fromCol - 1 && toRow == fromRow - 1))
            return true;
    }
    return false;
}

/**
 * @brief ChessBoard::knightMove checks all the possible legal move for knight
 * @param fromCol
 * @param fromRow
 * @param toCol
 * @param toRow
 * @return
 */
bool ChessBoard::knightMove(int fromCol, int fromRow, int toCol, int toRow)
{
    QVector<int> rowVector;
    QVector<int> colVector;

    rowVector.push_back(fromRow + 2);
    colVector.push_back(fromCol - 1);
    rowVector.push_back(fromRow + 2);
    colVector.push_back(fromCol + 1);

    rowVector.push_back(fromRow - 2);
    colVector.push_back(fromCol - 1);
    rowVector.push_back(fromRow - 2);
    colVector.push_back(fromCol + 1);

    rowVector.push_back(fromRow + 1);
    colVector.push_back(fromCol - 2);
    rowVector.push_back(fromRow + 1);
    colVector.push_back(fromCol + 2);

    rowVector.push_back(fromRow - 1);
    colVector.push_back(fromCol - 2);
    rowVector.push_back(fromRow - 1);
    colVector.push_back(fromCol + 2);

    int color = blackOrWhite(chessBoard[fromRow][fromCol]);

    for (int i = 0; i < rowVector.size(); i++)
    {
        int curRow = rowVector[i];
        int curCol = colVector[i];

        if (checkNums(curCol, curRow))
        {
            if (color != blackOrWhite(chessBoard[toRow][toCol]) && toCol == curCol && toRow == curRow)
                return true;
        }
    }
    return false;
}

/**
 * @brief ChessBoard::bishopMove checks all the bishop legal moves
 * @param fromCol
 * @param fromRow
 * @param toCol
 * @param toRow
 * @return
 */
bool ChessBoard::bishopMove(int fromCol, int fromRow, int toCol, int toRow)
{
    int color = blackOrWhite(chessBoard[fromRow][fromCol]);
    QVector<int> rowVector;
    QVector<int> colVector;

    int tempRow = fromRow + 1;
    int tempCol = fromCol + 1;
    while (checkNums(tempCol, tempRow) && blackOrWhite(chessBoard[tempRow][tempCol]) != color)
    {
        rowVector.push_back(tempRow);
        colVector.push_back(tempCol);
        if (blackOrWhite(chessBoard[tempRow][tempCol]) != 0)
            break;
        tempRow++;
        tempCol++;
    }

    tempRow = fromRow - 1;
    tempCol = fromCol + 1;
    while (checkNums(tempCol, tempRow) && blackOrWhite(chessBoard[tempRow][tempCol]) != color)
    {
        rowVector.push_back(tempRow);
        colVector.push_back(tempCol);
        if (blackOrWhite(chessBoard[tempRow][tempCol]) != 0)
            break;
        tempRow--;
        tempCol++;
    }

    tempRow = fromRow + 1;
    tempCol = fromCol - 1;
    while (checkNums(tempCol, tempRow) && blackOrWhite(chessBoard[tempRow][tempCol]) != color)
    {
        rowVector.push_back(tempRow);
        colVector.push_back(tempCol);
        if (blackOrWhite(chessBoard[tempRow][tempCol]) != 0)
            break;
        tempRow++;
        tempCol--;
    }

    tempRow = fromRow - 1;
    tempCol = fromCol - 1;
    while (checkNums(tempCol, tempRow) && blackOrWhite(chessBoard[tempRow][tempCol]) != color)
    {
        rowVector.push_back(tempRow);
        colVector.push_back(tempCol);
        if (blackOrWhite(chessBoard[tempRow][tempCol]) != 0)
            break;
        tempRow--;
        tempCol--;
    }

    for (int i = 0; i < rowVector.size(); i++)
    {
        if (rowVector[i] == toRow && colVector[i] == toCol)
            return true;
    }

    return false;
}

/**
 * @brief ChessBoard::rookMove checsks all the legal rook move
 * @param fromCol
 * @param fromRow
 * @param toCol
 * @param toRow
 * @return
 */
bool ChessBoard::rookMove(int fromCol, int fromRow, int toCol, int toRow)
{
    int color = blackOrWhite(chessBoard[fromRow][fromCol]);
    QVector<int> rowVector;
    QVector<int> colVector;

    int tempRow = fromRow + 1;
    int tempCol = fromCol;
    while (checkNums(tempCol, tempRow) && blackOrWhite(chessBoard[tempRow][tempCol]) != color)
    {
        rowVector.push_back(tempRow);
        colVector.push_back(tempCol);
        if (blackOrWhite(chessBoard[tempRow][tempCol]) != 0)
            break;
        tempRow++;
    }

    tempRow = fromRow - 1;
    while (checkNums(tempCol, tempRow) && blackOrWhite(chessBoard[tempRow][tempCol]) != color)
    {
        rowVector.push_back(tempRow);
        colVector.push_back(tempCol);
        if (blackOrWhite(chessBoard[tempRow][tempCol]) != 0)
            break;
        tempRow--;
    }

    tempRow = fromRow;
    tempCol = fromCol - 1;
    while (checkNums(tempCol, tempRow) && blackOrWhite(chessBoard[tempRow][tempCol]) != color)
    {
        rowVector.push_back(tempRow);
        colVector.push_back(tempCol);
        if (blackOrWhite(chessBoard[tempRow][tempCol]) != 0)
            break;
        tempCol--;
    }

    tempCol = fromCol + 1;
    while (checkNums(tempCol, tempRow) && blackOrWhite(chessBoard[tempRow][tempCol]) != color)
    {
        rowVector.push_back(tempRow);
        colVector.push_back(tempCol);
        if (blackOrWhite(chessBoard[tempRow][tempCol]) != 0)
            break;
        tempCol++;
    }

    for (int i = 0; i < rowVector.size(); i++)
    {
        if (rowVector[i] == toRow && colVector[i] == toCol)
        {
            checkRookPosition(fromCol, fromRow);
            return true;
        }
    }

    return false;
}

/**
 * @brief ChessBoard::checkRookPosition checks if rook has moved
 * @param fromCol
 * @param fromRow
 */
void ChessBoard::checkRookPosition(int fromCol, int fromRow)
{
    if (fromCol == 0 && fromRow == 0)
        bLRookMove = true;
    if (fromCol == 7 && fromRow == 0)
        bRRookMove = true;
    if (fromCol == 0 && fromRow == 7)
        wLRookMove = true;
    if (fromCol == 7 && fromRow == 7)
        wRRookMove = true;
}
/**
 * @brief ChessBoard::queenMove checks all legal queen moves
 * @param fromCol
 * @param fromRow
 * @param toCol
 * @param toRow
 * @return
 */
bool ChessBoard::queenMove(int fromCol, int fromRow, int toCol, int toRow)
{
    return bishopMove(fromCol, fromRow, toCol, toRow) || rookMove(fromCol, fromRow, toCol, toRow);
}

/**
 * @brief ChessBoard::kingMove checks all the king moves
 * @param fromCol
 * @param fromRow
 * @param toCol
 * @param toRow
 * @return
 */
bool ChessBoard::kingMove(int fromCol, int fromRow, int toCol, int toRow)
{
    int color = blackOrWhite(chessBoard[fromRow][fromCol]);
    if (!castle(color, fromCol, fromRow, toCol, toRow))
    {
        if (color == 1)
        {
            wKingRow = toRow;
            wKingCol = toCol;
        }
        if (color == -1)
        {
            bKingRow = toRow;
            bKingCol = toCol;
        }
        return false;
    }

    for (int i = fromRow - 1; i <= fromRow + 1; i++)
    {
        for (int j = fromCol - 1; j <= fromCol + 1; j++)
        {
            if (i == fromRow && j == fromCol)
                continue;
            if (checkNums(j, i) && blackOrWhite(chessBoard[i][j]) != color && i == toRow && j == toCol)
            {
                if (color == 1)
                {
                    wKingRow = toRow;
                    wKingCol = toCol;
                    wKingMove = true;
                }

                else
                {
                    bKingRow = toRow;
                    bKingCol = toCol;
                    bKingMove = true;
                }

                return true;
            }
        }
    }
    return false;
}

/**
 * @brief ChessBoard::castle checks if castling is possible
 * @param color color of the piece
 * @param fromCol
 * @param fromRow
 * @param toCol
 * @param toRow
 * @return
 */
bool ChessBoard::castle(int color, int fromCol, int fromRow, int toCol, int toRow)
{
    if (color == 1 && !wKingMove && !wRRookMove && chessBoard[fromRow][fromCol + 1] == ' ' && chessBoard[fromRow][fromCol + 2] == ' ' && !isAttacked(fromCol, fromRow, 1) && !isAttacked(fromCol + 1, fromRow, 1) && !isAttacked(fromCol + 2, fromRow, 1) && toRow == fromRow && toCol == fromCol + 2)
    {
        enPassant--;
        turn = !turn;

        setData(toCol + 1, 8 - toRow, 'K');
        setData(fromCol + 1, 8 - fromRow, ' ');

        setData(fromCol + 2, 8 - toRow, 'R');
        setData(toCol + 2, 8 - fromRow, ' ');
        wKingMove = true;
        return false;
    }

    if (color == 1 && !wKingMove && !wLRookMove && chessBoard[fromRow][fromCol - 1] == ' ' && chessBoard[fromRow][fromCol - 2] == ' ' && chessBoard[fromRow][fromCol - 3] == ' ' && !isAttacked(fromCol, fromRow, 1) && !isAttacked(fromCol - 1, fromRow, 1) && !isAttacked(fromCol - 2, fromRow, 1) && !isAttacked(fromCol - 3, fromRow, 1) && toRow == fromRow && toCol == fromCol - 2)
    {
        enPassant--;
        turn = !turn;

        setData(toCol + 1, 8 - toRow, 'K');
        setData(fromCol + 1, 8 - fromRow, ' ');

        setData(toCol + 2, 8 - toRow, 'R');
        setData(toCol - 1, 8 - fromRow, ' ');
        wKingMove = true;
        return false;
    }

    if (color == -1 && !bKingMove && !bRRookMove && chessBoard[fromRow][fromCol + 1] == ' ' && chessBoard[fromRow][fromCol + 2] == ' ' && !isAttacked(fromCol, fromRow, -1) && !isAttacked(fromCol + 1, fromRow, -1) && !isAttacked(fromCol + 2, fromRow, -1) && toRow == fromRow && toCol == fromCol + 2)
    {
        enPassant--;
        turn = !turn;

        setData(toCol + 1, 8 - toRow, 'k');
        setData(fromCol + 1, 8 - fromRow, ' ');

        setData(fromCol + 2, 8 - toRow, 'r');
        setData(toCol + 2, 8 - fromRow, ' ');
        bKingMove = true;
        return false;
    }

    if (color == -1 && !bKingMove && !bLRookMove && chessBoard[fromRow][fromCol - 1] == ' ' && chessBoard[fromRow][fromCol - 2] == ' ' && chessBoard[fromRow][fromCol - 3] == ' ' && !isAttacked(fromCol, fromRow, -1) && !isAttacked(fromCol - 1, fromRow, -1) && !isAttacked(fromCol - 2, fromRow, -1) && !isAttacked(fromCol - 3, fromRow, -1) && toRow == fromRow && toCol == fromCol - 2)
    {
        enPassant--;
        turn = !turn;

        setData(toCol + 1, 8 - toRow, 'k');
        setData(fromCol + 1, 8 - fromRow, ' ');

        setData(toCol + 2, 8 - toRow, 'r');
        setData(toCol - 1, 8 - fromRow, ' ');
        bKingMove = true;
        return false;
    }
    return true;
}

/**
 * @brief ChessBoard::isAttacked checks if the square is attacked by the oposiste color
 * @param col
 * @param row
 * @param color
 * @return
 */
bool ChessBoard::isAttacked(int col, int row, int color)
{
    //king attack
    for (int i = row - 1; i <= row + 1; i++)
    {
        for (int j = col - 1; j <= col + 1; j++)
        {
            if (i == row && j == col)
                continue;
            if (checkNums(j, i) && blackOrWhite(chessBoard[i][j]) != color && (chessBoard[i][j] == 'k' || chessBoard[i][j] == 'K'))
            {
                return true;
            }
        }
    }


    // find pawn attacking
    if (color == 1)
    {
        if (checkNums(col + 1, row - 1) && chessBoard[row - 1][col + 1] == 'p')
            return true;
        if (checkNums(col - 1, row - 1) && chessBoard[row - 1][col - 1] == 'p')
            return true;
    }

    if (color == -1)
    {
        if (checkNums(col + 1, row + 1) && chessBoard[row + 1][col + 1] == 'P')
            return true;
        if (checkNums(col - 1, row + 1) && chessBoard[row + 1][col - 1] == 'P')
            return true;
    }

    // find knights attacking
    QVector<int> rowVector;
    QVector<int> colVector;

    rowVector.push_back(row + 2);
    colVector.push_back(col - 1);
    rowVector.push_back(row + 2);
    colVector.push_back(col + 1);

    rowVector.push_back(row - 2);
    colVector.push_back(col - 1);
    rowVector.push_back(row - 2);
    colVector.push_back(col + 1);

    rowVector.push_back(row + 1);
    colVector.push_back(col - 2);
    rowVector.push_back(row + 1);
    colVector.push_back(col + 2);

    rowVector.push_back(row - 1);
    colVector.push_back(col - 2);
    rowVector.push_back(row - 1);
    colVector.push_back(col + 2);

    for (int i = 0; i < rowVector.size(); i++)
    {
        int curRow = rowVector[i];
        int curCol = colVector[i];

        if (checkNums(curCol, curRow))
        {
            if ((chessBoard[curRow][curCol] == 'n' || chessBoard[curRow][curCol] == 'N') && color != blackOrWhite(chessBoard[curRow][curCol]))
            {
                return true;
            }
        }
    }

    int tempRow = row + 1;
    int tempCol = col + 1;
    while (checkNums(tempCol, tempRow) && blackOrWhite(chessBoard[tempRow][tempCol]) == 0)
    {
        tempRow++;
        tempCol++;
    }
    if (checkNums(tempCol, tempRow) && blackOrWhite(chessBoard[tempRow][tempCol]) != color && checkIfBishopRookQueen(tempCol, tempRow, false))
    {
        return true;
    }

    tempRow = row - 1;
    tempCol = col + 1;
    while (checkNums(tempCol, tempRow) && blackOrWhite(chessBoard[tempRow][tempCol]) == 0)
    {
        tempRow--;
        tempCol++;
    }
    if (checkNums(tempCol, tempRow) && blackOrWhite(chessBoard[tempRow][tempCol]) != color && checkIfBishopRookQueen(tempCol, tempRow, false))
    {
        return true;
    }

    tempRow = row + 1;
    tempCol = col - 1;
    while (checkNums(tempCol, tempRow) && blackOrWhite(chessBoard[tempRow][tempCol]) == 0)
    {
        tempRow++;
        tempCol--;
    }

    if (checkNums(tempCol, tempRow) && blackOrWhite(chessBoard[tempRow][tempCol]) != color && checkIfBishopRookQueen(tempCol, tempRow, false))
    {
        return true;
    }

    tempRow = row - 1;
    tempCol = col - 1;
    while (checkNums(tempCol, tempRow) && blackOrWhite(chessBoard[tempRow][tempCol]) == 0)
    {
        tempRow--;
        tempCol--;
    }

    if (checkNums(tempCol, tempRow) && blackOrWhite(chessBoard[tempRow][tempCol]) != color && checkIfBishopRookQueen(tempCol, tempRow, false))
    {
        return true;
    }

    tempRow = row + 1;
    tempCol = col;
    while (checkNums(tempCol, tempRow) && blackOrWhite(chessBoard[tempRow][tempCol]) == 0)
    {
        tempRow++;
    }
    if (checkNums(tempCol, tempRow) && blackOrWhite(chessBoard[tempRow][tempCol]) != color && checkIfBishopRookQueen(tempCol, tempRow, true))
    {
        return true;
    }

    tempRow = row - 1;
    while (checkNums(tempCol, tempRow) && blackOrWhite(chessBoard[tempRow][tempCol]) == 0)
    {
        tempRow--;
    }
    if (checkNums(tempCol, tempRow) && blackOrWhite(chessBoard[tempRow][tempCol]) != color && checkIfBishopRookQueen(tempCol, tempRow, true))
    {
        return true;
    }

    tempRow = row;
    tempCol = col - 1;
    while (checkNums(tempCol, tempRow) && blackOrWhite(chessBoard[tempRow][tempCol]) == 0)
    {
        tempCol--;
    }

    if (checkNums(tempCol, tempRow) && blackOrWhite(chessBoard[tempRow][tempCol]) != color && checkIfBishopRookQueen(tempCol, tempRow, true))
    {
        return true;
    }

    tempCol = col + 1;
    while (checkNums(tempCol, tempRow) && blackOrWhite(chessBoard[tempRow][tempCol]) == 0)
    {
        tempCol++;
    }

    if (checkNums(tempCol, tempRow) && blackOrWhite(chessBoard[tempRow][tempCol]) != color && checkIfBishopRookQueen(tempCol, tempRow, true))
    {
        return true;
    }

    return false;
}

/**
 * @brief ChessBoard::checkIfBishopRookQueen checks if a piece is rook/bishop or queen
 * @param col
 * @param row
 * @param rook determine if we are hecking rook or queen or bishop or queen
 * @return
 */
bool ChessBoard::checkIfBishopRookQueen(int col, int row, bool rook)
{
    char piece = chessBoard[row][col];
    if (rook)
    {
        if (piece == 'r' || piece == 'R' || piece == 'Q' || piece == 'q')
            return true;
        else
            return false;
    }
    else
    {
        if (piece == 'b' || piece == 'B' || piece == 'Q' || piece == 'q')
            return true;
        else
            return false;
    }
}

/**
 * @brief ChessBoard::blackOrWhite determines if a piece is black or white or none
 * @param piece
 * @return return 0 if empty 1 if white 0 if black or otherwise
 */
int ChessBoard::blackOrWhite(char piece)
{
    if (piece == ' ')
        return 0;
    if (isupper(piece))
        return 1;
    return -1;
}

/**
 * @brief ChessBoard::setFen - sets the string to a FEN string
 * @param fen - the string representation of the board (standardized FEN - https://www.chess.com/terms/fen-chess)
 * @cite https://codereview.stackexchange.com/questions/251795/parsing-a-chess-fen
 */
void ChessBoard::setFen(const QString &fen)
{
    if (fen != ("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"))
    {
        tutorialInSession = true;
    }

    if (fen == ("8/8/8/8/8/8/8/8 w KQkq - 0 1"))
    {
        complete = true;
    }

    int index = 0;
    int skip = 0;
    const int columnCount = columns();
    QChar ch;
    for (int rank = ranks(); rank > 0; --rank)
    {
        for (int column = 1; column <= columnCount; ++column)
        {
            if (skip > 0)
            {
                ch = ' ';
                skip--;
            }
            else
            {
                ch = fen.at(index++);
                if (ch.isDigit())
                {
                    skip = ch.toLatin1() - '0';
                    ch = ' ';
                    skip--;
                }
            }
            setDataInternal(column, rank, ch.toLatin1());
        }
        QChar next = fen.at(index++);
        if (next != '/' && next != ' ')
        {
            initBoard();
            return;
        }
    }

    int row = 0;
    int col = 0;
    bool colChange = false;
    for (int i = 0; i < fen.length(); i++)
    {
        QChar character = fen.at(i);
        if (character == ' ')
            break;
        if (character == '/')
        {
            row++;
            col = 0;
            continue;
        }
        else if (character.isDigit())
        {
            // converts from character to number
            col += character.toLatin1() - 48;
            colChange = true;
        }
        else
        {
            chessBoard[row][col] = character.toLatin1();
        }
        if (!colChange)
            col++;
        colChange = false;
    }
    newBoard();
    // for enpeasant
    emit boardReset();
}

/**
 * @brief ChessBoard::newBoard resets the logic for chessBoard
 */
void ChessBoard::newBoard()
{
    enPassant = 0;
    eRow = 0;
    eCol = 0;
    turn = false;

    wKingMove = false;
    bKingMove = false;
    wRRookMove = false;
    wLRookMove = false;
    bRRookMove = false;
    bLRookMove = false;

    bKingRow = -1;
    bKingCol = -1;
    wKingRow = -1;
    wKingCol = -1;

    for (int i = 0; i < chessBoard.size(); i++)
    {
        for (int j = 0; j < chessBoard.size(); j++)
        {
            if (chessBoard[i][j] == 'k')
            {
                bKingRow = i;
                bKingCol = j;
            }

            if (chessBoard[i][j] == 'K')
            {
                wKingRow = i;
                wKingCol = j;
            }
        }
    }
}

/**
 * @brief ChessBoard::kingPosCheck checks the king pos ie checks if kings are on the board.
 * @param white
 * @return
 */
bool ChessBoard::kingPosCheck(bool white)
{
    if (white)
        return wKingRow == -1 || wKingCol == -1;
    else
        return bKingRow == -1 || bKingCol == -1;
}

/**
 * @brief ChessBoard::tutorialMove checks all the correct tutorial moves.
 * @param fromCol
 * @param fromRow
 * @param toCol
 * @param toRow
 * @return
 */
bool ChessBoard::tutorialMove(int fromCol, int fromRow, int toCol, int toRow)
{
    switch (tutorialNum)
    {
    case 0:
        if (toCol == fromCol && toRow == fromRow - 2)
        {
            tutorialNum++;
            return true;
        }
        else
            return false;
        break;
    case 1:
        if (toCol == fromCol && toRow == fromRow - 1)
        {
            tutorialNum++;
            return true;
        }
        else
            return false;
        break;
    case 2:
        if (toCol == fromCol && toRow == fromRow - 1)
        {
            tutorialNum++;
            return true;
        }
        else
            return false;
        break;
    case 3:
        if (toCol == fromCol + 1 && toRow == fromRow - 1)
        {
            tutorialNum++;
            setData(5, 5, ' ');
            return true;
        }
        else
            return false;
        break;
    case 4:
        if (toCol == fromCol || toRow == fromRow)
        {
            tutorialNum++;
            return true;
        }
        else
            return false;
        break;
    case 5:
        if (toCol == fromCol - 3 && toRow == fromRow)
        {
            tutorialNum++;
            return true;
        }
        else
            return false;
        break;
    case 6:
        if ((toCol == fromCol - 1 && toRow == fromRow - 2) || (toCol == fromCol - 2 && toRow == fromRow - 1) || (toCol == fromCol + 1 && toRow == fromRow - 2))
        {
            tutorialNum++;
            return true;
        }
        else
            return false;
        break;
    case 7:
        if ((toCol == fromCol - 2 && toRow == fromRow - 1))
        {
            tutorialNum++;
            return true;
        }
        else
            return false;
        break;
    case 8:
        if (toCol == 0 && toRow == 0)
        {
            tutorialNum++;
            return true;
        }
        else
            return false;
        break;
    case 9: // Bishop taking a piece
        if (toCol == 6 && toRow == 0)
        {
            tutorialNum++;
            return true;
        }
        else
            return false;
        break;
    case 10: // queen move diag
        if (toCol == 7 && toRow == 3)
        {
            tutorialNum++;
            return true;
        }
        else
            return false;
        break;

    case 11:
        if (fromCol == toCol || toRow == fromRow)
        {
            tutorialNum++;
            return true;
        }
        else
            return false;
        break;
        // queen take diag

    case 12:
        if (toCol == fromCol + 2 && toRow == fromRow)
        {
            setData(5, 1, ' ');
            setData(8, 1, ' ');
            setData(7, 1, 'K');
            setData(6, 1, 'R');
            tutorialNum++;
            return true;
        }
        else
            return false;
        break;

    case 13:
        if (toCol == fromCol - 2 && toRow == fromRow)
        {
            setData(5, 1, ' ');
            setData(1, 1, ' ');
            setData(3, 1, 'K');
            setData(4, 1, 'R');
            tutorialNum++;
            return true;
        }
        else
            return false;
        break;
    }
}
