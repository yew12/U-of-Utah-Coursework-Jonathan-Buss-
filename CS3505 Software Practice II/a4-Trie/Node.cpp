#include "Node.h"
using std::swap;

/// @brief : This class helps us for creating the Trie data structure in Trie.cpp. Contains only
/// a constructor to intialize our boolean

/**
 * @brief Constructor for our node class to intialize our endOfWord
 * boolean flag
 */
Node::Node()
{
  // boolean for indicating we are at the end of a word
  endOfWord = false;
}