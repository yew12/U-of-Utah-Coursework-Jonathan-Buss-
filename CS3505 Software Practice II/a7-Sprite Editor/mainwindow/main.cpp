/**
 * @file main.cpp
 * @author Tate, Thatcher, Gage, Sanjay
 * @brief  This file starts the application
 * @version 0.1
 * @date 2022-11-15
 *
 * @copyright Copyright (c) 2022, U of U CS 3505
 *
 * @section no code style review neccasary.
 *
 */
#include "mainwindow.h"

#include <QApplication>
#include <QColorDialog>

int main(int argc, char *argv[])
{
    QApplication a(argc, argv);

    Model model;
    MainWindow w(model);

    w.show();
    return a.exec();
}
