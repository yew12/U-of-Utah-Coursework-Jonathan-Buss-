/**
 * @file physicsworld.cpp - physics world for the queen objects
 * @author Tate Reynolds, Thatcher Geary, Sanjay Gounder, Gage Buss, Jonas K.
 * @brief This file contains the physics world for the queen objects
 * @version 0.1
 * @date 2022-12-08
 *
 * @copyright Copyright (c) 2022, U of U CS3505
 *
 */

#include "physicsworld.h"
#include <iostream>

PhysicsWorld::PhysicsWorld(QWidget *parent)
    : QWidget{parent},
      _world{new b2World{b2Vec2{0.0f, -10.0f}}},
      _timerId{startTimer(1000 / 60)},
      _transform{QTransform::fromScale(10.0f, -10.0f)}
{
    _objects.reserve(32);

    setMouseTracking(true);

    // add a ball to the world every 2 seconds
    timer = new QTimer(this);
    connect(timer, SIGNAL(timeout()), this, SLOT(addBall()));
    timer->start(700);

    _transform.translate(0.0f, -64.0f);

    // create the ground
    b2BodyDef groundBodyDef;
    groundBodyDef.type = b2_staticBody;
    groundBodyDef.position.Set(0.0f, -6.0f);
    b2Body *groundBody = _world->CreateBody(&groundBodyDef);
    b2PolygonShape groundBox;
    groundBox.SetAsBox(500.0f, 10.0f);
    groundBody->CreateFixture(&groundBox, 0.0f);

    // the balls should bounce off the ground
    b2FixtureDef fixtureDef;
    fixtureDef.restitution = 0.5f;
}

Object PhysicsWorld::createBall(const b2Vec2 position, float radius)
{
    Object o;
    // body
    b2BodyDef bd;
    bd.type = b2_dynamicBody;
    bd.position = position;
    o.body = _world->CreateBody(&bd);

    o.body->SetLinearVelocity(b2Vec2(0.0f, 0.0f));
    // shape
    b2CircleShape shape;
    shape.m_radius = radius;
    // fixture
    b2FixtureDef fd;

    fd.shape = &shape;
    fd.density = 1.0f;
    fd.friction = 1.0f;
    fd.restitution = 0.6f;
    o.fixture = o.body->CreateFixture(&fd);
    o.type = BallObject;
    return o;
}

void PhysicsWorld::paintEvent(QPaintEvent *event)
{
    QPainter painter{this};
    painter.setRenderHint(QPainter::Antialiasing);
    painter.setTransform(_transform);

    for (const Object &object : _objects)
    {
        b2Vec2 size(object.fixture->GetShape()->m_radius, object.fixture->GetShape()->m_radius);
        switch (object.type)
        {
        case BallObject:
            drawPiece(painter, object.body->GetPosition(), object.fixture->GetShape()->m_radius);
            break;
        }
    }
}

void PhysicsWorld::drawPiece(QPainter &painter, const b2Vec2 position, float radius)
{
    QImage piece(":/pieces/pieces/queen-black.png");
    QImage flippedPiece = piece.mirrored(false, true); // flips our piece right side up
    painter.drawImage(QRect(position.x, position.y, radius * 10, radius * 10), flippedPiece);
}

void PhysicsWorld::timerEvent(QTimerEvent *event)
{
    if (event->timerId() == _timerId)
    {
        _world->Step(1.0f / 60.0f, 8, 3);
        update();
    }
}

void PhysicsWorld::addBall()
{
    // ball should be spread out in the x direction
    int dx = arc4random() % 56;
    int dy = arc4random() % 2;
    _objects.append(createBall(b2Vec2(18.0f + dx, 62.0f - dy), 1.0f));
}

void PhysicsWorld::addBall(const QPoint &point)
{
    _objects.append(createBall(b2Vec2(point.x(), point.y()), 1.0f));
}

void PhysicsWorld::mousePressEvent(QMouseEvent *event)
{
    if (event->button() == Qt::LeftButton)
    {
        // translate the mouse position to the physics world
        QPoint p = _transform.inverted().map(event->pos());
        addBall(p);
    }
}
