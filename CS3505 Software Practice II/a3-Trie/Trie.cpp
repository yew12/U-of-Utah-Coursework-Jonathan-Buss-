/**
 * Jonathan Gage Buss
 * CS 3505
 * Assignment 3
 **/

#include "Trie.h"
#include "Node.h"
#include <vector>
#include <iostream>
#include <algorithm> // find - https://stackoverflow.com/questions/6277646/in-c-check-if-stdvectorstring-contains-a-certain-value
#include <string>
using namespace std;

/**
 * Constructor
 **/
Trie::Trie()
{
    root = new Node();    // creates a new root Node object on the heap
    word = root->isAWord; // initialize our isAWord variable
}

/**
 * Destructor
 **/
Trie::~Trie()
{
    delete root; //  delete the root
}

/**
 * Copy constructor
 **/
Trie::Trie(const Trie &other)
{
    word = other.word;              // copy our word
    root = new Node(*(other.root)); // other.left -> pointer to an object | *(other.left) -> is the object
}

/**
 * Overloaded assignment operator
 **/
Trie &Trie::operator=(Trie other)
{
    /**
     * example x = y
     * y is copied to "other", other now has same value as y with different space in memory
     *
     * whatever was in x, now has "other"'s address and value
     */
    swap(root, other.root);

    return *this; // returning object by dereferencing it
}

/**
 * Inserts a word into our trie
 *
 * https://www.geeksforgeeks.org/trie-insert-and-search/
 **/
void Trie::addAWord(string wordToAdd)
{
    Node *currentNode = root;

    // loop through our wordToAdd
    for (unsigned int i = 0; i < wordToAdd.length(); i++)
    {
        /*
         * This shifts the ASCII values to conform with our values from 0-25
         * - https://stackoverflow.com/questions/8118802/what-exactly-does-arrays-charati-a-do
         */
        int asciiIndex = wordToAdd[i] - 'a';

        // if path does not yet exist, create a new node
        if (currentNode->alphabetArr[asciiIndex] == nullptr)
        {
            currentNode->alphabetArr[asciiIndex] = new Node(); // creates new node
        }

        currentNode = currentNode->alphabetArr[asciiIndex]; // otherwise, move onto the next node
    }

    // once inserting entire string, update our boolean on isAWord to true
    currentNode->isAWord = true;
}

/**
 * Searches in our trie to see if the word parameter exists or not
 *
 * https://www.geeksforgeeks.org/trie-insert-and-search/
 **/
bool Trie::isAWord(string word)
{
    Node *currentNode = root; // sets our current node to the root node

    // loop through our word parameter
    for (unsigned int i = 0; i < word.length(); i++)
    {
        /*
         * This shifts the ASCII values to conform with our values from 0-25
         * - https://stackoverflow.com/questions/8118802/what-exactly-does-arrays-charati-a-do
         */
        int asciiIndex = word[i] - 'a';

        // if path does not exist, word is not present and return false
        if (currentNode->alphabetArr[asciiIndex] == nullptr)
        {
            return false;
        }

        currentNode = currentNode->alphabetArr[asciiIndex]; // otherwise, move onto the next node
    }

    // check to see if isAWord at currentNode is true or false
    if (currentNode->isAWord)
    {
        return true; // we have found a word, return false
    }
    else
    {
        return false; // else, we have not found our word so we return false
    }
}

/**
 * Returns a vector containing every word in our given Trie that begins with our "prefixString"
 **/
vector<string> Trie::allWordsBeginningWithPrefix(string prefixString)
{
    vector<string> prefixVector;                                                     // intialize our vector of words
    vector<string> updatedPrefixVector = getPrefixWords(prefixVector, prefixString); // retrieve list of our prefix words with our new initiatilized vector
    return updatedPrefixVector;                                                      // return our updated prefixVector
}

/**
 * Goes through and builds our list of prefix words and returns a vector of those strings
 *
 * TODO: fix
 **/
vector<string> Trie::getPrefixWords(vector<string> &prefixVector, string prefixString)
{
    Node *currentNode = root; // sets our current node to the root node

    // first check to see if we have an empty prefix
    for (unsigned int i = 0; i < prefixString.length(); i++)
    {
        /*
         * This shifts the ASCII values to conform with our values from 0-25
         * - https://stackoverflow.com/questions/8118802/what-exactly-does-arrays-charati-a-do
         */
        int asciiIndex = word[i] - 'a';

        // check to see if it is null at current node
        if (currentNode->alphabetArr[asciiIndex] == nullptr)
        {
            return prefixVector; // return our prefix vector
        }

        currentNode = currentNode->alphabetArr[asciiIndex]; // otherwise, move onto the next node
    }

    checkPrefixWordDuplicates(currentNode, prefixVector, prefixString); // vector duplicates checker for our vector

    // iterate the length of the alphabet to eventually fill out our vector
    for (unsigned int i = 0; i < 26; i++)
    {
        // check to see if it is null at current node at index i
        if (currentNode->alphabetArr[i] == nullptr)
        {
            /*
             * This shifts the ASCII values to conform with our values from 0-25
             * - https://stackoverflow.com/questions/8118802/what-exactly-does-arrays-charati-a-do
             */
            char asciiIndex = word[i] + 'a';

            string updatedPrefixString = prefixString + asciiIndex; // build our updated prefix string

            getPrefixWords(prefixVector, updatedPrefixString); // recursively call our method with updated string
        }
    }

    return prefixVector;
}

/**
 * Helper method that checks our prefix vector for duplicates to avoid adding into our vector
 **/
void Trie::checkPrefixWordDuplicates(Node *currentNode, vector<string> &prefixVector, string prefixString)
{
    // http://en.cppreference.com/w/cpp/algorithm/find
    auto vectorElement = find(prefixVector.begin(), prefixVector.end(), prefixString); // searches for an element equal to our prefix string

    // check if current node is a word
    if (currentNode->isAWord)
    {
        // if so, check for duplicates
        if (vectorElement != prefixVector.end())
        {
            prefixVector.push_back(prefixString); // not a duplicate, add to vector
        }
    }
}