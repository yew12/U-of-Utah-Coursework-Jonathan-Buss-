#include <cassert>
#include <gtest/gtest.h>
#include <algorithm>
#include <iostream>
#include <assert.h>
#include "Trie.h"

// g++ -I/googletest/googletest/include -o TrieTests TrieTests.cpp -L/googletest/lib -lgtest -lpthread
// Alternate: g++ -I./googletest/googletest/include -o TrieTests TrieTests.cpp -L./googletest/lib -lgtest -lpthread
// FilePath: ~/github-classroom/University-of-Utah-CS3505/a4-refactor-and-test-Gage-Sanjay

///  @author: Sanjay Gounder & Jonathan Gage Buss
///  @date  : 3 October 2022
///  @class : CS 3505
///  @brief : Trie testing file

/**
 * @brief Simple test to check for words that do exist in
 * isAWord()
 * @param
 * @param
 */
TEST(TrieTestSimple, TrieTestEnsuringWordsExist)
{
    Trie trie;
    trie.addAWord("hi");
    trie.addAWord("high");
    trie.addAWord("him");
    EXPECT_TRUE(trie.isAWord("hi"));
    EXPECT_FALSE(trie.isAWord("bit"));
    EXPECT_TRUE(trie.isAWord("high"));
    EXPECT_TRUE(trie.isAWord("him"));
}

/**
 * @brief Simple test to check for words that don't exist in
 * isAWord()
 * @param
 * @param Test
 * @param
 * @param
 */
TEST(TrieTestSimple, TrieTestForWordsThatDontExist)
{
    Trie trie;
    trie.addAWord("i");
    trie.addAWord("do");
    trie.addAWord("exist");

    EXPECT_FALSE(trie.isAWord("he"));
    EXPECT_FALSE(trie.isAWord("doesnt"));
    EXPECT_FALSE(trie.isAWord("show"));
}

/**
 * @brief Simple test for our allWordsBeginningWithPrefix() method
 * @param
 * @param
 */
TEST(TrieTestSimple, TrieTestForWordsInPrefix)
{
    Trie trie;
    trie.addAWord("hi");
    trie.addAWord("high");
    trie.addAWord("him");
    vector<std::string> s = trie.allWordsBeginningWithPrefix("hi");

    // Makes sure vector of prefix "hi" has 3 elements from the Trie
    ASSERT_TRUE(s.size() == 3);
}

/**
 * @brief Test for ensuring the correct words are stored in the vector
 * @param
 * @param
 */
TEST(TrieTestSimple, TrieTestForWordsItselfInPrefix)
{
    Trie trie;
    trie.addAWord("hi");
    trie.addAWord("high");
    trie.addAWord("him");
    vector<std::string> s = trie.allWordsBeginningWithPrefix("hi");

    // Makes sure vector of prefix "hi" has 3 elements from the Trie

    //Way to ensure a word is contained in the prefix
    ASSERT_TRUE(find(s.begin(), s.end(), "hi") != s.end());
    ASSERT_TRUE(find(s.begin(), s.end(), "high") != s.end());
    ASSERT_TRUE(find(s.begin(), s.end(), "him") != s.end());
    ASSERT_FALSE(find(s.begin(), s.end(), "la") != s.end());
}

/**
 * @brief Simple test for having no words in our allWordsBeginningWithPrefix() method
 * @param
 * @param
 */
TEST(TrieTestSimple, TrieTestForNoWordsWithPrefix)
{
    Trie trie;
    trie.addAWord("hi");
    trie.addAWord("high");
    trie.addAWord("him");
    vector<std::string> s = trie.allWordsBeginningWithPrefix("el");

    // Make sure a vector of prefix 'el' has no values stored in it as no words contain prefix in the trie
    ASSERT_TRUE(s.size() == 0);
}

/**
 * @brief Test for ensuring the correct words are stored in the vector,
 * then adding a word to the trie and creating a new vector again which will be correct words
 * @param
 * @param
 */
TEST(TrieTestSimple, TrieTestForWordsInPrefixTrieUpdated)
{
    Trie trie;
    trie.addAWord("hi");
    trie.addAWord("high");
    trie.addAWord("him");
    vector<std::string> s = trie.allWordsBeginningWithPrefix("hi");

    // Makes sure vector of prefix "hi" has 3 elements from the Trie

    //Way to ensure a word is contained in the prefix
    ASSERT_TRUE(find(s.begin(), s.end(), "hi") != s.end());
    ASSERT_TRUE(find(s.begin(), s.end(), "high") != s.end());
    ASSERT_TRUE(find(s.begin(), s.end(), "him") != s.end());
    ASSERT_FALSE(find(s.begin(), s.end(), "la") != s.end());

    trie.addAWord("hill");
    vector<std::string> prefixHi = trie.allWordsBeginningWithPrefix("hi");
    //Way to ensure a word is contained in the prefix
    ASSERT_TRUE(find(prefixHi.begin(), prefixHi.end(), "hi") != prefixHi.end());
    ASSERT_TRUE(find(prefixHi.begin(), prefixHi.end(), "high") != prefixHi.end());
    ASSERT_TRUE(find(prefixHi.begin(), prefixHi.end(), "him") != prefixHi.end());
    ASSERT_FALSE(find(prefixHi.begin(), prefixHi.end(), "la") != prefixHi.end());
    ASSERT_TRUE(find(prefixHi.begin(), prefixHi.end(), "hill") != prefixHi.end());
}

/**
 * @brief Simple test for our copy constructor
 * @param
 * @param
 */
TEST(TrieTestCopyConstructor, TestCopyConstructor)
{
    Trie originalTrie;

    Trie copyTrie = originalTrie; // copy the originalTrie into our copy trie

    // tests if this works correctly
    copyTrie.addAWord("copy");
    ASSERT_TRUE(copyTrie.isAWord("copy"));
}

/**
 * @brief Simple test for our copy constructor with a loaded Trie
 * @param
 * @param
 */
TEST(TrieTestCopyConstructor, TestCopyConstructorWithLoadedTree)
{
    Trie originalTrie;

    originalTrie.addAWord("one");
    originalTrie.addAWord("two");
    originalTrie.addAWord("three");
    originalTrie.addAWord("four");

    Trie copyTrie = originalTrie; // copy the originalTrie into our copy trie

    // tests if this works correctly
    ASSERT_TRUE(copyTrie.isAWord("one"));
    ASSERT_TRUE(copyTrie.isAWord("two"));
    ASSERT_TRUE(copyTrie.isAWord("three"));
    ASSERT_TRUE(copyTrie.isAWord("four"));
}

/**
 * @brief Simple test for our copy constructor with a a non loaded Trie
 * @param
 * @param
 */
TEST(TrieTestCopyConstructor, TestCopyConstructorWithUnLoadedTree)
{
    Trie originalTrie;

    originalTrie.addAWord("one");
    originalTrie.addAWord("two");
    originalTrie.addAWord("three");
    originalTrie.addAWord("four");

    Trie copyTrie = originalTrie; // copy the originalTrie into our copy trie

    // tests if this works correctly
    ASSERT_FALSE(copyTrie.isAWord("five"));
    ASSERT_FALSE(copyTrie.isAWord("six"));
    ASSERT_FALSE(copyTrie.isAWord("seven"));
    ASSERT_FALSE(copyTrie.isAWord("eight"));
}

/**
 * @brief Simple test for our copy constructor with nothing in it, then adding and seeing what is in the original and copied one
 * @param
 * @param
 */
TEST(TrieTestCopyConstructor, TestCopyConstructorWithNothing)
{
    Trie originalTrie;

    Trie copyTrie = originalTrie; // copy the originalTrie into our copy trie

    // tests if this works correctly
    ASSERT_FALSE(copyTrie.isAWord("five"));
    ASSERT_FALSE(copyTrie.isAWord("six"));
    ASSERT_FALSE(copyTrie.isAWord("seven"));
    ASSERT_FALSE(copyTrie.isAWord("eight"));

    copyTrie.addAWord("five");
    copyTrie.addAWord("six");

    // Words should be in the copy
    ASSERT_TRUE(copyTrie.isAWord("five"));
    ASSERT_TRUE(copyTrie.isAWord("six"));
    // Words shouldn't be in original
    ASSERT_FALSE(originalTrie.isAWord("five"));
    ASSERT_FALSE(originalTrie.isAWord("six"));
}

TEST(TrieTestCopyConstructor, TestPointers)
{
    Trie originalTrie;
    Trie* ptrTrie = &originalTrie;

    ptrTrie->addAWord("hey");
    ptrTrie->addAWord("beach");
    ptrTrie->addAWord("believe");
    ptrTrie->addAWord("behave");

    //Since ptrTrie is a pointer, should affect the originalTrie
    ASSERT_TRUE(originalTrie.isAWord("hey"));
    ASSERT_TRUE(originalTrie.isAWord("behave"));
    ASSERT_FALSE(originalTrie.isAWord("grow"));

    //Size of this prefix is 3
    vector <std::string> bePrefix = originalTrie.allWordsBeginningWithPrefix("be");
    ASSERT_TRUE(bePrefix.size() == 3);
}

TEST(TrieTestEdgeCases, TestEdgeCase1)
{
    Trie trie;
    trie.addAWord("cat");
    trie.addAWord("cats");
    trie.addAWord("car");
    trie.addAWord("behave");

    ASSERT_TRUE(trie.isAWord("cat"));
    ASSERT_TRUE(trie.isAWord("cats"));
    ASSERT_FALSE(trie.isAWord("cable"));
    ASSERT_TRUE(trie.isAWord("car"));

    vector <std::string> caPrefix = trie.allWordsBeginningWithPrefix("ca");
    ASSERT_TRUE(caPrefix.size() == 3);
}

TEST(TrieTestEdgeCases, TestEdgeCase2)
{
    Trie trie;
    //Trie with 8 words beginning with same prefix of 'ca'
    trie.addAWord("cat");
    trie.addAWord("cats");
    trie.addAWord("car");
    trie.addAWord("cable");
    trie.addAWord("cab");
    trie.addAWord("cars");
    trie.addAWord("carts");
    trie.addAWord("phone");
    trie.addAWord("cart");

    vector <std::string> caPrefix = trie.allWordsBeginningWithPrefix("ca");
    ASSERT_TRUE(caPrefix.size() == 8);
    ASSERT_FALSE(caPrefix.size() == 9);
    ASSERT_TRUE(trie.isAWord("phone"));
    ASSERT_TRUE(trie.isAWord("cart"));
    ASSERT_TRUE(trie.isAWord("carts"));
}

TEST(TrieTestEdgeCases, TestEdgeCase3)
{
    Trie trie;

    trie.addAWord("abcdefghijklmnopqrstuvwxyz");
    bool flagCheck = trie.isAWord("abcdefghijklmnopqrstuvwxyz");
    ASSERT_TRUE(flagCheck);

    trie.addAWord("abcdefghi");
    bool flagCheck2 = trie.isAWord("abcdefghi");
    ASSERT_TRUE(flagCheck2);

    //Now this prefix should contain the two words we added in
    vector <std::string> prefix = trie.allWordsBeginningWithPrefix("abc");
    ASSERT_TRUE(find(prefix.begin(), prefix.end(), "abcdefghijklmnopqrstuvwxyz") != prefix.end());
    ASSERT_TRUE(find(prefix.begin(), prefix.end(), "abcdefghi") != prefix.end());
    //This word, while part of the words of both the first and second is not an actual word
    ASSERT_FALSE(find(prefix.begin(), prefix.end(), "abdefg") != prefix.end());

    trie.addAWord("abdefg");
    vector <std::string> prefix2 = trie.allWordsBeginningWithPrefix("abc");
    ASSERT_TRUE(find(prefix2.begin(), prefix2.end(), "abcdefghijklmnopqrstuvwxyz") != prefix.end());
    ASSERT_TRUE(find(prefix2.begin(), prefix2.end(), "abcdefghi") != prefix.end());
    //Now this word is a part of the vector now that it has been added in
    ASSERT_TRUE(find(prefix2.begin(), prefix2.end(), "abdefg") != prefix.end());
}

TEST(TrieTestEdgeCases, TestEdgeCase4)
{
    //Testing empty strings for isAWord and addAWord
    Trie trie;
    trie.addAWord("");
    trie.addAWord("hi");
    trie.addAWord("");
    trie.addAWord("both");
    trie.addAWord("get");
    ASSERT_FALSE(trie.isAWord(""));
    ASSERT_TRUE(trie.isAWord("hi"));

    Trie trie2;
    ASSERT_FALSE(trie2.isAWord(""));

    //Should have a list containing "hi", "both", and "get".
    vector<std::string> v = trie.allWordsBeginningWithPrefix("");
    ASSERT_TRUE(v.size() == 3);
    ASSERT_TRUE(find(v.begin(), v.end(), "hi") != v.end());
    ASSERT_TRUE(find(v.begin(), v.end(), "both") != v.end());
    ASSERT_TRUE(find(v.begin(), v.end(), "get") != v.end());
    ASSERT_FALSE(find(v.begin(), v.end(), "") != v.end());

}

TEST(TrieTestEdgeCases, TestEdgeCase5)
{
    //Similiar to the test above just a little more complicated with more words
    Trie trie;
    trie.addAWord("between");
    trie.addAWord("beneath");
    trie.addAWord("xylophone");
    trie.addAWord("abcdefg");
    trie.addAWord("conundrum");
    trie.addAWord("superfluous");
    trie.addAWord("abcdefghello");

    vector <std::string> allWords = trie.allWordsBeginningWithPrefix("");
    ASSERT_TRUE(allWords.size() == 7);
    //Just making sure test Trie did not add extra words
    ASSERT_FALSE(allWords.size() == 8);

    ASSERT_TRUE(find(allWords.begin(), allWords.end(), "conundrum") != allWords.end());
    ASSERT_TRUE(find(allWords.begin(), allWords.end(), "between") != allWords.end());
    ASSERT_TRUE(find(allWords.begin(), allWords.end(), "superfluous") != allWords.end());
    ASSERT_TRUE(find(allWords.begin(), allWords.end(), "abcdefghello") != allWords.end());
    ASSERT_TRUE(find(allWords.begin(), allWords.end(), "abcdefg") != allWords.end());
    ASSERT_TRUE(find(allWords.begin(), allWords.end(), "xylophone") != allWords.end());
    ASSERT_TRUE(find(allWords.begin(), allWords.end(), "beneath") != allWords.end());

    //Empty string not contained
    ASSERT_FALSE(find(allWords.begin(), allWords.end(), "") != allWords.end());
}


