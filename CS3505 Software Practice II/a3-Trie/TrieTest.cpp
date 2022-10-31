/**
 * Jonathan Gage Buss
 * CS 3505
 * Assignment 3
 **/

#include <iostream>
#include <fstream>
#include <string>
#include "Trie.h"
#include "Node.h"
using namespace std;

/**
 * Main that takes in two arguments:
 *  (1) File of words, each on their own line, with the words all lowercase and only made up of characters a-z
 *  (2) File of queries, each word on its own line and also of acceptable characters.
 **/
int main(int argc, char *argv[])
{
    // first check if we have the correct number of files inputed
    if (argc < 2)
    {
        cout << "Need the correct number of input files - (2)" << endl;
        return 0; // print and end
    }

    vector<string> fileVector;     // otherwise, put files into our vector variable
    fileVector.push_back(argv[1]); // push first file
    fileVector.push_back(argv[2]); // push second file

    Trie trie; // initialize our Trie object

    /**
     * Reading the first file
     * https://cplusplus.com/doc/tutorial/files/
     *
     **/
    string word;
    ifstream file1(fileVector[0]); // creates stream for first file
    if (file1.is_open())
    {
        // loop through our file line by line
        while (getline(file1, word))
        {
            trie.addAWord(word);
        }

        file1.close();
    }
    else
    {
        cout << "Unable to open file";
    }

    /**
     * Reading the second file
     * https://cplusplus.com/doc/tutorial/files/
     *
     **/
    word = "";
    ifstream file2(fileVector[1]); // creates stream for first file
    if (file2.is_open())
    {
        while (getline(file2, word))
        {
            // see if word exists using our isAWord() method
            if (trie.isAWord(word))
            {
                // if word is found, print to console
                cout << word << " is found" << endl;
            }
            // if not, word is not found, get all the words starting with the prefix in our word
            else
            {
                cout << word << " is not found, did you mean: " << endl;

                vector<string> vectorWords = trie.allWordsBeginningWithPrefix(word); // get all words with same prefix as our word

                // check to see if there were any alternative found
                if (vectorWords.size() != 0)
                {
                    // loop through our vector and print the words
                    for (string word : vectorWords)
                        cout << word << "   "; // word with the listing indented 3 spaces;
                }
                else
                {
                    cout << "no alternatives found"
                         << "   "; // we didn't find any alternatives
                }

                cout << endl; // add the end of the line
            }
        }

        file2.close();
    }
    else
    {
        cout << "Unable to open file";
    }

    return 0;
}
