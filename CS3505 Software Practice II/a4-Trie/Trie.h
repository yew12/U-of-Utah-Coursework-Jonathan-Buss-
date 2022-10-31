#ifndef _TRIE_H_
#define _TRIE_H_

#include <string>
#include <vector>
#include "Node.h"

using std::string;
using std::vector;

/**
 * @brief This file contains everything needed to create a Trie. While utilizing a helper Node class
 * containing our backing structure of a map.
 * @author Jonathan Gage Buss & Sanjay Gounder
 * @since Mon Oct 03 2022
 */
class Trie
{
  Node rootNode; // root node

  vector <std::string> allWordsInTrie; //Keeping track of all words in the Trie
public:
  void addAWord(string wordToAdd);                                                            // adds a word to our Trie
  bool isAWord(string wordToCheck);                                                           // searches Trie to see if word exists
  vector<std::string> allWordsBeginningWithPrefix(std::string stringToCheck);                 // gets all the words in the Trie beginning with the prefix of passed in word
  vector<std::string> buildPrefixList(std::string stringToCheck, vector<std::string> &words); // helper method for allWordsBeginningWithPrefix()
};

#endif