/**
 * @file physicsworld.h
 * @author Tate Reynolds, Thatcher Geary, Sanjay Gounder, Gage Buss, Jonas K.
 * @brief This file contains the backend for the physics world
 * @version 0.1
 * @date 2022-12-08
 *
 * @copyright Copyright (c) 2022, U of U CS3505
 *
 */

#ifndef PHYSICSWORLD_H
#define PHYSICSWORLD_H

#include <QWidget>
#include <QTimer>
#include <QPainter>
#include <QMouseEvent>
#include <Box2D/Box2D.h>

enum
{
    PawnObject,
    RookObject,
    KnightObject,
    BishopObject,
    QueenObject,
    KingObject,
    BoardObject,
    BallObject
} Objects;

struct Object
{
    int type;
    b2Body *body;
    b2Fixture *fixture;
};

/**
 * @brief The PhysicsWorld class
 */
class PhysicsWorld : public QWidget
{
    Q_OBJECT
public:
    explicit PhysicsWorld(QWidget *parent = nullptr);
    void paintEvent(QPaintEvent *event);
    void mousePressEvent(QMouseEvent *event);
    //  void mouseMoveEvent(QMouseEvent *event);
    //  void mouseReleaseEvent(QMouseEvent *event);
    void timerEvent(QTimerEvent *event);
    Object createBall(const b2Vec2 position, float radius);
    QTimer *timer;
    void drawPiece(QPainter &painter, b2Vec2 position, float radius);

signals:

public slots:
    void addBall();
    void addBall(const QPoint &point);

private:
    b2World *_world;
    int _timerId;
    QTransform _transform;
    QVector<Object> _objects;
};

#endif // PHYSICSWORLD_H
