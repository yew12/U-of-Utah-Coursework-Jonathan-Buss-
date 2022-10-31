/**
 * Jonathan Gage Buss
 * CS 3505
 * Assignment 3
 **/

#include "Node.h"
#include <vector>
#include <iostream>
using namespace std;

/**
 * Node constructor that initializes our alphabet array and our
 * isAWord boolean
 **/
Node::Node()
{
    isAWord = false; // initialize our boolean to false

    // loop through length of alphabet and initialize our array w/ nullptr
    for (int i = 0; i < alphabetLength; i++)
    {
        alphabetArr[i] = nullptr;
    }
}

/**
 * Node destructor - loops through alphabet length and deletes at given index
 **/
Node::~Node()
{
    // loop through and delete each node at each index
    for (int i = 0; i < alphabetLength; i++)
    {
        if (alphabetArr[i] != nullptr)
        {
            delete alphabetArr[i]; // deletes at given index
        }
    }
}

/**
 * Copy constructor
 **/
Node::Node(const Node &other)
{
    isAWord = other.isAWord;
    alphabetLength = other.alphabetLength;

    // loop through and copy each item in the node array
    for (int i = 0; i < alphabetLength; i++)
    {
        alphabetArr[i] = nullptr;
        // check to see if item at index i is not null
        if (alphabetArr[i] != nullptr)
        {
            alphabetArr[i] = new Node(*(other.alphabetArr[i])); // copy at given index i
        }
    }
}

/**
 * Helper method that recursively searches each node for words then fills and returns an
 * updated vector
 **/
vector<string> Node::getPrefixWords(vector<string> *prefixVector, string prefixString, string currentWord)
{

    // check to see if prefix string is empty, if so return all words in Trie
    if (prefixString.empty())
    {
        // loop through alphabetArr
        for (int i = 0; i < alphabetLength; i++)
        {
            /**
             * https://stackoverflow.com/questions/29586564/what-does-the-97-mean-in-the-following-code
             **/
            char newLocation = i + 97;                      // mapping location for that char to an index between 0-25
            string updatedWord = currentWord + newLocation; // gets the updated location for our word

            // check to see our current location is a word
            if (alphabetArr[i]->isAWord)
            {
                prefixVector->push_back(updatedWord); // add our updated word to vector
            }

            alphabetArr[i]->getPrefixWords(prefixVector, prefixString, updatedWord); // recursively call method - this goes to our next node
        }
    }

    return *prefixVector; // return our dereferenced/updated prefix vector
}
