#ifndef _NODE_H_
#define _NODE_H_

#include <string>
#include <map>

using std::map;
using std::string;

const int SIZE_OF_ALPHABET = 26;

/**
 * @brief Node object for our Trie class. Contains a map, boolean flag and constructor.
 * @author Jonathan Gage Buss & Sanjay Gounder
 * @since Mon Oct 03 2022
 */
class Node
{

public:
  // Node *children[SIZE_OF_ALPHABET];

  map<char, Node> nodeMap;
  // boolean is true if the node is at the end of a word
  bool endOfWord;
  Node();
};

#endif