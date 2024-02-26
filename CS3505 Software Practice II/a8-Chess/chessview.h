/**
 * @file chessview.h
 * @author Tate Reynolds, Thatcher Geary, Sanjay Gounder, Gage Buss, Jonas K.
 * @brief This file contains the frontend for the chess board
 * @version 0.1
 * @date 2022-12-08
 *
 * @copyright Copyright (c) 2022, U of U CS3505
 *
 */

#ifndef CHESSVIEW_H
#define CHESSVIEW_H

#include <QWidget>
#include <QPointer>
#include <chessboard.h>

/**
 * @brief The ChessView class
 */
class ChessView : public QWidget
{
    Q_OBJECT
    Q_PROPERTY(QSize fieldSize
                   READ fieldSize WRITE setFieldSize
                       NOTIFY fieldSizeChanged)

public:
    class Highlight
    {
    public:
        Highlight() {}
        virtual ~Highlight() {}
        virtual int type() const { return 0; }
    };

    class FieldHighlight : public Highlight
    {
    public:
        enum
        {
            Type = 1
        };
        FieldHighlight(int column, int rank, QColor color)
            : m_field(column, rank), m_color(color) {}
        inline int column() const { return m_field.x(); }
        inline int rank() const { return m_field.y(); }
        inline QColor color() const { return m_color; }
        int type() const { return Type; }

    private:
        QPoint m_field;
        QColor m_color;
    };
    QList<Highlight *> m_highlights;
    explicit ChessView(QWidget *parent = nullptr);
    void setBoard(ChessBoard *board);
    const QSize &fieldSize() const;
    void setFieldSize(const QSize &newFieldSize);
    QSize sizeHint() const;
    void paintEvent(QPaintEvent *);
    void drawRank(QPainter *painter, int rank);
    void drawColumn(QPainter *painter, int column);
    void drawField(QPainter *painter, int column, int rank);
    ChessBoard *board() const;
    void setPiece(char type, const QIcon &icon);
    QIcon piece(char type) const;
    void drawPiece(QPainter *painter, int column, int rank);
    QPoint fieldAt(const QPoint &pt) const;
    void mouseReleaseEvent(QMouseEvent *event);
    void addHighlight(Highlight *hl);
    void removeHighlight(Highlight *hl);
    inline Highlight *highlight(int index) const
    {
        return m_highlights.at(index);
    }
    inline int highlightCount() const
    {
        return m_highlights.size();
    }
    void drawHighlights(QPainter *painter);

signals:
    void clicked(const QPoint &);
    void fieldSizeChanged();

private:
    QPointer<ChessBoard> _board;
    QRect fieldRect(int column, int rank) const;
    QSize _fieldSize;
    QMap<char, QIcon> m_pieces;
};

#endif // CHESSVIEW_H
