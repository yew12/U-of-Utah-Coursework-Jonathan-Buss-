#include "Trie.h"
// basic file operations
#include <iostream>
#include <fstream>

using std::cout;
using std::endl;
using std::ifstream;
/// @author : Sanjay Gounder
/// @date : 23 September 2022
/// @class : CS 3505
/// @brief
/// @param argc
/// @param argv
/// @return
int main(int argc, char **argv)
{
    if (argc > 3)
    {
        cout << "Incorrect file input" << endl;
        return 0;
    }

    vector<std::string> files;
    // Get first file in vector
    files.push_back(argv[1]);
    // Get second file in vector
    files.push_back(argv[2]);

    Trie trieFile;

    string word;
    // create a stream for the file of words
    ifstream fileofwords(files[0], ifstream::in);
    if (fileofwords.is_open())
    {
        // loop through file by line
        while (getline(fileofwords, word))
        {
            // Adds word from fileofwords.txt to the trie
            trieFile.addAWord(word);
        }

        fileofwords.close();
    }
    else
    {
        cout << "File not able to be opened";
    }
    // Clean up word to be an empty string which we will use now for second loop of the queries
    word = "";
    // Now we create a stream for the queries
    ifstream fileofqueries(files[1], ifstream::in);
    if (fileofqueries.is_open())
    {
        // loop through lines in fileofqueries.txt
        while (getline(fileofqueries, word))
        {
            // See if word we are querying exists in the Trie
            if (trieFile.isAWord(word))
            {
                cout << "word is found: " << word << endl;
            }
            else
            {
                cout << word << " is not found, did you mean: " << endl;
                vector<std::string> vectorOfWords = trieFile.allWordsBeginningWithPrefix(word);
                if (vectorOfWords.size() != 0)
                {
                    for (string word : vectorOfWords)
                    {
                        cout << word << "   "; // words indented by three spaces
                    }
                }
                else
                {
                    cout << "No alternatives found"
                         << "   ";
                }
                cout << endl;
            }
        }
        fileofqueries.close();
    }
    else
    {
        cout << "Can't open file";
    }

    return 0;
}