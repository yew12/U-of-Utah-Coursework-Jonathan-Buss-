/**
 * Jonathan Gage Buss
 * CS 3505
 * Assignment 3
 **/

#ifndef TRIE_H
#define TRIE_H
#include "Node.h"
#include <vector>
#include <iostream>
using namespace std;

class Trie
{
public:
    Node *root;                                                                                           // root of our trie
    string word;                                                                                          // built through default constructor
    Trie();                                                                                               // constructor
    ~Trie();                                                                                              // destructor
    Trie(const Trie &other);                                                                              // copy constructor
    Trie &operator=(Trie other);                                                                          // assignment operator
    void addAWord(string wordToAdd);                                                                      // inserts word into Trie
    bool isAWord(string word);                                                                            // searches for word in Trie
    vector<string> allWordsBeginningWithPrefix(string prefixString);                                      // gets all words beginning with a prefix in Trie
    vector<string> getPrefixWords(vector<string> &prefixVector, string updatedPrefixString);              // helper method for allWordsBeginningWithPrefix()
    void checkPrefixWordDuplicates(Node *currentNode, vector<string> &prefixVector, string prefixString); // helper method for allWordsBeginningWithPrefix()
};

#endif