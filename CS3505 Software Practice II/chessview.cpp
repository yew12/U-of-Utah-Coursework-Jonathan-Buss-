/**
 * @file chessview.cpp - frontend for the chess board
 * @author Tate Reynolds, Thatcher Geary, Sanjay Gounder, Gage Buss, Jonas K.
 * @brief This file contains the frontend for the chess board
 * @version 0.1
 * @date 2022-12-08
 *
 * @copyright Copyright (c) 2022, U of U CS3505
 *
 */

#include "chessview.h"
#include <QMouseEvent>
#include <QColor>

ChessView::ChessView(QWidget *parent)
    : QWidget{parent}
{
    // for tutorial
}

/**
 * @brief ChessView::setBoard - setter for the board member
 *
 * @param board
 */
void ChessView::setBoard(ChessBoard *board)
{
    if (_board == board)
    {
        return;
    }
    if (_board)
    {
        // disconnect all signal-slot connections between _board and this
        _board->disconnect(this);
    }
    _board = board;
    // connect signals
    if (board)
    {
        connect(board, SIGNAL(dataChanged(int, int)),
                this, SLOT(update()));
        connect(board, SIGNAL(boardReset()),
                this, SLOT(update()));
    }
    updateGeometry();
}

/**
 * @brief ChessView::board - getter for the board member
 *
 * @return ChessBoard*
 */

ChessBoard *ChessView::board() const
{
    return _board;
}

/**
 * @brief ChessView::fieldSize - getter for the fieldSize member
 *
 * @return const QSize&
 */
const QSize &ChessView::fieldSize() const
{
    return _fieldSize;
}

/**
 * @brief ChessView::setFieldSize - setter for the fieldSize member
 *
 * @param newFieldSize
 */
void ChessView::setFieldSize(const QSize &newFieldSize)
{
    if (_fieldSize == newFieldSize)
        return;
    _fieldSize = newFieldSize;
    emit fieldSizeChanged();
}

/**
 * @brief ChessView::sizeHint - getter for the sizeHint member
 *
 * @return QSize
 */
QSize ChessView::sizeHint() const
{
    if (!_board)
    {
        return QSize(100, 100);
    }
    QSize boardSize = QSize(fieldSize().width() * _board->columns() + 1,
                            _fieldSize.height() * _board->ranks() + 1);
    // 'M' is the widest letter
    int rankSize = fontMetrics().maxWidth() + 4;
    int columnSize = fontMetrics().height() + 4;
    return boardSize + QSize(rankSize, columnSize);
}

/**
 * @brief ChessView::fieldRect - getter for the fieldRect member
 *
 * @param column
 * @param rank
 * @return QRect - the rectangle of the field
 */
QRect ChessView::fieldRect(int column, int rank) const
{
    if (!_board)
    {
        return QRect();
    }
    const QSize fs = fieldSize();
    QPoint topLeft((column - 1) * fs.width(),
                   (_board->ranks() - rank) * fs.height());
    QRect fRect = QRect(topLeft, fs);
    // offset rect by rank symbols
    int offset = fontMetrics().maxWidth();
    return fRect.translated(offset + 4, 0);
}

/**
 * @brief paintEvent - paints the board
 *
 */
void ChessView::paintEvent(QPaintEvent *)
{
    if (!_board)
    {
        return;
    }
    QPainter painter(this);
    for (int r = _board->ranks(); r > 0; --r)
    {
        painter.save();
        drawRank(&painter, r);
        painter.restore();
    }
    for (int c = 1; c <= _board->columns(); ++c)
    {
        painter.save();
        drawColumn(&painter, c);
        painter.restore();
    }
    for (int r = 1; r <= _board->ranks(); ++r)
    {
        for (int c = 1; c <= _board->columns(); ++c)
        {
            painter.save();
            drawField(&painter, c, r);
            painter.restore();
        }
    }
    drawHighlights(&painter);
    for (int r = _board->ranks(); r > 0; --r)
    {
        for (int c = 1; c <= _board->columns(); ++c)
        {
            drawPiece(&painter, c, r);
        }
    }
}

/**
 * @brief ChessView::drawRank - draws the rank (row)
 *
 * @param painter
 * @param rank
 */
void ChessView::drawRank(QPainter *painter, int rank)
{
    QRect r = fieldRect(1, rank);
    QRect rankRect = QRect(0, r.top(), r.left(), r.height())
                         .adjusted(2, 0, -2, 0);
    QString rankText = QString::number(rank);
    painter->drawText(rankRect,
                      Qt::AlignVCenter | Qt::AlignRight, rankText);
}

/**
 * @brief ChessView::drawColumn - draws the column (column)
 *
 * @param painter
 * @param column
 */
void ChessView::drawColumn(QPainter *painter, int column)
{
    QRect r = fieldRect(column, 1);
    QRect columnRect =
        QRect(r.left(), r.bottom(), r.width(), height() - r.bottom())
            .adjusted(0, 2, 0, -2);
    painter->drawText(columnRect,
                      Qt::AlignHCenter | Qt::AlignTop, QChar('a' + column - 1));
}

/**
 * @brief ChessView::drawField - draws the field
 *
 * @param painter
 * @param column
 * @param rank
 */
void ChessView::drawField(QPainter *painter, int column, int rank)
{
    QColor dark(163, 133, 80);
    QColor light(250, 231, 197);
    QRect rect = fieldRect(column, rank);
    QColor fillColor = (column + rank) % 2 ? light : dark;
    painter->setPen(palette().color(QPalette::Dark));
    painter->setBrush(fillColor);
    painter->drawRect(rect);
}

/**
 * @brief ChessView::setPiece - sets the piece
 *
 * @param type
 * @param icon
 */
void ChessView::setPiece(char type, const QIcon &icon)
{
    m_pieces.insert(type, icon);
    update();
}

/**
 * @brief ChessView::piece - getter for the piece
 *
 * @param type
 * @return QIcon
 */
QIcon ChessView::piece(char type) const
{
    return m_pieces.value(type, QIcon());
}

/**
 * @brief  ChessView::drawPiece - draws the piece
 *
 * @param painter
 * @param column
 * @param rank
 */
void ChessView::drawPiece(QPainter *painter, int column, int rank)
{
    QRect rect = fieldRect(column, rank);
    char value = _board->data(column, rank);
    if (value != ' ')
    {
        QIcon icon = piece(value);
        if (!icon.isNull())
        {
            icon.paint(painter, rect, Qt::AlignCenter);
        }
    }
}

/**
 * @brief fieldAt - returns the field at the given point
 *
 * @param pt
 * @return QPoint
 */
QPoint ChessView::fieldAt(const QPoint &pt) const
{
    if (!_board)
    {
        return QPoint();
    }
    const QSize fs = fieldSize();
    int offset = fontMetrics().maxWidth() + 4;
    // 'M' is the widest letter
    if (pt.x() < offset)
    {
        return QPoint();
    }
    int c = (pt.x() - offset) / fs.width();
    int r = pt.y() / fs.height();
    if (c < 0 || c >= _board->columns() ||
        r < 0 || r >= _board->ranks())
    {
        return QPoint();
    }
    return QPoint(c + 1, _board->ranks() - r);
    // max rank - r
}

/**
 * @brief ChessView::mouseReleaseEvent - mouse release event
 *
 * @param event
 */
void ChessView::mouseReleaseEvent(QMouseEvent *event)
{
    QPoint pt = fieldAt(event->pos());
    if (pt.isNull())
    {
        return;
    }
    emit clicked(pt);
}

/**
 * @brief ChessView::addHighlight - adds the highlight
 *
 * @param hl
 */
void ChessView::addHighlight(ChessView::Highlight *hl)
{
    m_highlights.append(hl);
    update();
}

/**
 * @brief ChessView::removeHighlight - removes the highlight
 *
 * @param hl
 */
void ChessView::removeHighlight(ChessView::Highlight *hl)
{
    m_highlights.removeOne(hl);
    update();
}

/**
 * @brief ChessView::drawHighlights - draws the highlights
 *
 * @param painter
 */
void ChessView::drawHighlights(QPainter *painter)
{
    for (int idx = 0; idx < highlightCount(); ++idx)
    {
        Highlight *hl = highlight(idx);
        if (hl->type() == FieldHighlight::Type)
        {
            FieldHighlight *fhl = static_cast<FieldHighlight *>(hl);
            QRect rect = fieldRect(fhl->column(), fhl->rank());
            painter->fillRect(rect, fhl->color());
        }
    }
}
