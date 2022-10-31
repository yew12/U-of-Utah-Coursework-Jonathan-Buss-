#include "Trie.h"
#include <iostream>
#include <string.h>
#include <algorithm>
using std::find;
using std::swap;

///  @author: Sanjay Gounder & Jonathan Gage Buss
///  @date  : 3 October 2022
///  @class : CS 3505
///  @brief : Trie data structure stores characters in it's structure through branching and nodes
///           Backing structure is a map consisting of a character key and a Node object element.

/// @brief : Adds a word to the Trie
/// @param wordToAdd : The word being added to Trie
void Trie::addAWord(string wordToAdd)
{
  if (wordToAdd == "")
    return;

  // Get the rootAlphabet and store it as another variable so not to override it
  Node *currentNode = &rootNode;

  for (unsigned int i = 0; i < wordToAdd.length(); i++)
  {
    // checks if Node isn't already there at given key, then add a new node in
    if (!(currentNode->nodeMap.count(wordToAdd.at(i))))
    {
      Node newNode;
      currentNode->nodeMap[wordToAdd[i]] = newNode;
    }
    currentNode = &currentNode->nodeMap[wordToAdd[i]]; // traverse to next node
  }

  // https://stackoverflow.com/questions/6277646/in-c-check-if-stdvectorstring-contains-a-certain-value
    // This will essentially ensure we are not adding duplicate words into the vector, keeps track of all words in Trie essentially
    if (!(find(allWordsInTrie.begin(), allWordsInTrie.end(), wordToAdd) != allWordsInTrie.end()))
      allWordsInTrie.push_back(wordToAdd);

  // Now we are at the final character which was just added in so set this node's endOfWord flag to true
  currentNode->endOfWord = true;
}

/// @brief : Checks to see a word exists inside the Trie
/// @param wordToCheck : Word being checked to see if it exists in Trie
/// @return : a boolean indicating whether the word is contained (true) or not contained (false)
bool Trie::isAWord(string wordToCheck)
{
  // if the word is an empty string, return false
  if (wordToCheck == "")
    return false;

  Node *currentNode = &rootNode;

  for (unsigned int i = 0; i < wordToCheck.length(); i++)
  {
    // check to see if char exists in trie
    if (!(currentNode->nodeMap.count(wordToCheck.at(i))))
      return false; // character doesn't exist, meaning word won't exist, return false

    // traverse to next node
    currentNode = &currentNode->nodeMap[wordToCheck[i]];
  }
  // returns true if end of word flag is true and false otherwise
  return (currentNode->endOfWord);
}

/// @brief : Returns a vector containing all words inside the Trie which start with the prefix passed in
/// @param stringToCheck : The prefix passed into the trie
/// @return : A vector containing all words inside the Trie which start with the prefix passed in
vector<std::string> Trie::allWordsBeginningWithPrefix(std::string stringToCheck)
{
  if(stringToCheck == "")
    return allWordsInTrie;
  // Vector which will be passed into buildPrefixList and added to when a word is found containing prefix
  vector<std::string> words;

  // Gets list of prefixes
  vector<std::string> prefixList = buildPrefixList(stringToCheck, words);
  return prefixList;
}

/// @brief : Recursive helper method for building the vector
/// @param stringToCheck : the prefix being checked
/// @param words : the vector which is passed by reference due to recursion and updated as words are added
/// @return : The vector contianing all words starting with initial prefix passed in
vector<std::string> Trie::buildPrefixList(std::string stringToCheck, vector<std::string> &words)
{
  Node *currentNode = &rootNode;
  // Loops through prefix and ensures it is contained in the data structure, else returns an empty vector
  for (unsigned int i = 0; i < stringToCheck.length(); i++)
  {
    // check to see if char exists in trie
    if (!(currentNode->nodeMap.count(stringToCheck.at(i))))
      return words; // character doesn't exist, meaning word won't exist, return false

    // traverse to next node
    currentNode = &currentNode->nodeMap[stringToCheck[i]];
  }

  // If the prefix is a word itself in the Trie, add it to the vector
  if (currentNode->endOfWord)
  {
    // https://stackoverflow.com/questions/6277646/in-c-check-if-stdvectorstring-contains-a-certain-value
    // This will essentially ensure we are not adding duplicate words into the vector
    if (!(find(words.begin(), words.end(), stringToCheck) != words.end()))
      words.push_back(stringToCheck);
  }

  // Iterator
  for (auto it = currentNode->nodeMap.begin(); it != currentNode->nodeMap.end(); it++)
  {
    // Get next character after the prefix characters
    char charToAdd = it->first;
    // Build string to have prefix + the character
    string stringBeingBuilt = stringToCheck + charToAdd;

    // Recursively call this method and allow the stringBeingBuilt to be passed in as the word now to check
    buildPrefixList(stringBeingBuilt, words);
  }

  return words;
}
