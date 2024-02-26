/**
 * @file mainwindow.cpp - main window for the chess game
 * @author Tate Reynolds, Thatcher Geary, Sanjay Gounder, Gage Buss, Jonas K.
 * @brief This file contains the main window for the chess game
 * @version 0.1
 * @date 2022-12-08
 *
 * @copyright Copyright (c) 2022 U of U CS3505
 *
 */

#include "mainwindow.h"
#include "ui_mainwindow.h"
#include <QLayout>
#include <iostream>
#include <QAudioOutput>

/**
 * @brief MainWindow::MainWindow
 * @param parent
 */
MainWindow::MainWindow(QWidget *parent)
    : QMainWindow(parent), ui(new Ui::MainWindow)
{
    // initialize physicsWorld
    physicsWorld = new PhysicsWorld(this);
    physicsWorld->show();
    physicsWorld->resize(800, 700);

    ui->setupUi(this);
    // brings all of our buttons and labels to the front
    ui->gameLabel->raise();
    ui->startButton->raise();
    ui->tutorialButton->raise();

    connect(ui->startButton,
            &QPushButton::clicked,
            this,
            &MainWindow::startButtonClicked);

    connect(ui->tutorialButton,
            &QPushButton::clicked,
            this,
            &MainWindow::tutorialButtonClicked);

    // Sends the frame number from dialog.cpp to mainwindow.cpp
    connect(&popUp,
            &Dialog::tutorialStage,
            this,
            &MainWindow::getFrameNumber);

    //    // Lets the chessboard.cpp know tutorial has started

    connect(&popUp,
            &Dialog::askForCompletion,
            this,
            &MainWindow::askIfCompletedSlot);

    connect(this,
            &MainWindow::completionCheck,
            &popUp,
            &Dialog::completionCheckSlot);

    QAudioOutput *audioOutput = new QAudioOutput;
    music = new QMediaPlayer();
    audioOutput->setVolume(20);
    music->setAudioOutput(audioOutput);
    music->setSource(QUrl("qrc:///sound/pieces/sounds/background.mp3"));
    music->setLoops(500);
    music->play();
}

MainWindow::~MainWindow()
{
    delete ui;
}

/**
 * @brief Tracks when the view is clicked and where to highlight the board
 *
 * @param field
 */
void MainWindow::viewClicked(const QPoint &field)
{
    if (_clickPoint.isNull())
    {
        if (_view->board()->data(field.x(), field.y()) != ' ')
        {
            _clickPoint = field;
            _selectedField = new ChessView::FieldHighlight(
                field.x(), field.y(), QColor(255, 0, 0, 50));
            _view->addHighlight(_selectedField);
        }
    }
    else
    {
        if (field != _clickPoint)
        {
            _view->board()->movePiece(
                _clickPoint.x(), _clickPoint.y(), field.x(), field.y());
        };
        _clickPoint = QPoint();
        _view->removeHighlight(_selectedField);
        delete _selectedField;
        _selectedField = nullptr;
    }
}

/**
 * @brief Starts a local game
 *
 */
void MainWindow::startButtonClicked()
{
    // hide our start screen elements
    ui->startButton->hide();
    ui->gameLabel->hide();
    ui->tutorialButton->hide();
    physicsWorld->timer->stop(); // stops and hides our timer
    physicsWorld->hide();        // hides any remaining pieces

    _view = new ChessView;
    _algorithm = new ChessModel(this);
    _algorithm->newGame();
    _view->setBoard(_algorithm->board());
    setCentralWidget(_view);
    _view->setSizePolicy(QSizePolicy::Fixed, QSizePolicy::Fixed);
    _view->setFieldSize(QSize(50, 50));
    layout()->setSizeConstraint(QLayout::SetFixedSize);

    // add pieces to the board
    _view->setPiece('P', QIcon(":/pieces/pieces/pawn-white.png"));   // pawn
    _view->setPiece('K', QIcon(":/pieces/pieces/king-white.png"));   // king
    _view->setPiece('Q', QIcon(":/pieces/pieces/queen-white.png"));  // queen
    _view->setPiece('R', QIcon(":/pieces/pieces/rook-white.png"));   // rook
    _view->setPiece('N', QIcon(":/pieces/pieces/knight-white.png")); // knight
    _view->setPiece('B', QIcon(":/pieces/pieces/bishop-white.png")); // bishop

    _view->setPiece('p', QIcon(":/pieces/pieces/pawn-black.png"));   // pawn
    _view->setPiece('k', QIcon(":/pieces/pieces/king-black.png"));   // king
    _view->setPiece('q', QIcon(":/pieces/pieces/queen-black.png"));  // queen
    _view->setPiece('r', QIcon(":/pieces/pieces/rook-black.png"));   // rook
    _view->setPiece('n', QIcon(":/pieces/pieces/knight-black.png")); // knight
    _view->setPiece('b', QIcon(":/pieces/pieces/bishop-black.png")); // bishop

    connect(_view, &ChessView::clicked,
            this, &MainWindow::viewClicked);
    _selectedField = nullptr;
    _chessBoard = _algorithm->_board;
}

//*** TUTORIAL GAME*** ///

/**
 * @brief  starts the tutorial
 *
 */
void MainWindow::tutorialButtonClicked()
{
    // hide our start screen elements
    ui->startButton->hide();
    ui->gameLabel->hide();
    ui->tutorialButton->hide();
    physicsWorld->timer->stop(); // stops and hides our timer
    physicsWorld->hide();        // hides any remaining pieces

    popUp.move(900, 400);
    popUp.show();
    _view = new ChessView;
    _algorithm = new ChessModel(this);
    // tutorial for pawn loading in
    _algorithm->setupBoard();
    _view->setBoard(_algorithm->board());
    setCentralWidget(_view);
    _view->setSizePolicy(QSizePolicy::Fixed, QSizePolicy::Fixed);
    _view->setFieldSize(QSize(50, 50));
    layout()->setSizeConstraint(QLayout::SetFixedSize);

    // add pieces to the board
    _view->setPiece('P', QIcon(":/pieces/pieces/pawn-white.png"));   // pawn
    _view->setPiece('K', QIcon(":/pieces/pieces/king-white.png"));   // king
    _view->setPiece('Q', QIcon(":/pieces/pieces/queen-white.png"));  // queen
    _view->setPiece('R', QIcon(":/pieces/pieces/rook-white.png"));   // rook
    _view->setPiece('N', QIcon(":/pieces/pieces/knight-white.png")); // knight
    _view->setPiece('B', QIcon(":/pieces/pieces/bishop-white.png")); // bishop

    _view->setPiece('p', QIcon(":/pieces/pieces/pawn-black.png"));   // pawn
    _view->setPiece('k', QIcon(":/pieces/pieces/king-black.png"));   // king
    _view->setPiece('q', QIcon(":/pieces/pieces/queen-black.png"));  // queen
    _view->setPiece('r', QIcon(":/pieces/pieces/rook-black.png"));   // rook
    _view->setPiece('n', QIcon(":/pieces/pieces/knight-black.png")); // knight
    _view->setPiece('b', QIcon(":/pieces/pieces/bishop-black.png")); // bishop

    connect(_view, &ChessView::clicked,
            this, &MainWindow::viewClicked);

    _selectedField = nullptr;

    _chessBoard = _algorithm->_board;
}

/**
 * @brief Gets the frame number of the tutorial
 *
 * @param frameNumber
 */
void MainWindow::getFrameNumber(int frameNumber)
{
    switch (frameNumber)
    {
    case 2:
        _algorithm->pawnTutorial();
        break;
    case 3:
        _algorithm->pawnTutorial();
        break;
    case 4:
        _algorithm->pawnToQueenTutorial();
        break;
    case 8:
        _algorithm->enPassantTutorial();
        break;
    case 10:
        _algorithm->rookTutorial();
        break;
    case 11:
        _algorithm->rookTakePieceTutorial();
        break;
    case 13:
        _algorithm->knightTutorial();
        break;
    case 14:
        _algorithm->knightTakePieceTutorial();
        break;
    case 16:
        _algorithm->bishopTutorial();
        break;
    case 17:
        _algorithm->bishopTakePieceTutorial();
        break;
    case 19:
        _algorithm->queenTutorial();
        break;
    case 20:
        _algorithm->queenTutorial();
        break;
    case 24:
        _algorithm->kingShortCastle();
        break;
    case 25:
        _algorithm->kingLongCastle();
        break;
    case 26:
        _algorithm->newGame();
        break;
    default:
        _algorithm->emptyBoard();
    }

    resetBoardTutorial();
}

/**
 * @brief checks if screen is completed
 *
 */
void MainWindow::askIfCompletedSlot()
{
    emit completionCheck(_chessBoard->complete);
    _chessBoard->complete = false;
}

/**
 * @brief Resets the board for the tutorial
 *
 */
void MainWindow::resetBoardTutorial()
{
    _view->setBoard(_algorithm->board());

    // add pieces to the board
    _view->setPiece('P', QIcon(":/pieces/pieces/pawn-white.png"));   // pawn
    _view->setPiece('K', QIcon(":/pieces/pieces/king-white.png"));   // king
    _view->setPiece('Q', QIcon(":/pieces/pieces/queen-white.png"));  // queen
    _view->setPiece('R', QIcon(":/pieces/pieces/rook-white.png"));   // rook
    _view->setPiece('N', QIcon(":/pieces/pieces/knight-white.png")); // knight
    _view->setPiece('B', QIcon(":/pieces/pieces/bishop-white.png")); // bishop

    _view->setPiece('p', QIcon(":/pieces/pieces/pawn-black.png"));   // pawn
    _view->setPiece('k', QIcon(":/pieces/pieces/king-black.png"));   // king
    _view->setPiece('q', QIcon(":/pieces/pieces/queen-black.png"));  // queen
    _view->setPiece('r', QIcon(":/pieces/pieces/rook-black.png"));   // rook
    _view->setPiece('n', QIcon(":/pieces/pieces/knight-black.png")); // knight
    _view->setPiece('b', QIcon(":/pieces/pieces/bishop-black.png")); // bishop
}

/**
 * @brief Mutes music
 *
 * @param checked
 */
void MainWindow::on_muteButton_toggled(bool checked)
{
    checked ? music->stop() : music->play();
}
