/**
 * Jonathan Gage Buss
 * CS 3505
 * Assignment 3
 **/

#ifndef NODE_H
#define NODE_H
#include "Node.h"
#include <vector>
#include <iostream>
using namespace std;

class Node
{
    int alphabetLength = 26;

public:
    Node *alphabetArr[26];   // alphabet array of size 26, has derefernced pointer
    bool isAWord;            // boolean flag to signal if we are at a full word
    Node();                  // constructor that fills alphabet array full of nullptrs
    ~Node();                 // destructor
    Node(const Node &other); // copy constructor
    vector<string> getPrefixWords(
        vector<string> *prefixVector,
        string prefixString,
        string currentWord); // helper method that recursively gets the prefix words at each node
};

#endif