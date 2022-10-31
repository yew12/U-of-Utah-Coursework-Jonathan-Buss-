# use variables to define compilation 
# properties
CC = g++
CFLAGS = -Wall -std=c++11 -g
INCLUDE = ./
FSANITIZE = -fsanitize=undefined


TrieTest: TrieTest.o Trie.o Node.o
	$(CC) $(CFLAGS) $(FSANITIZE) -o TrieTest TrieTest.o Trie.o Node.o
TrieTest.o: TrieTest.cpp 
	$(CC) $(CFLAGS) -I$(INCLUDE) -c TrieTest.cpp
Trie.o: Trie.cpp 
	$(CC) $(CFLAGS) -I$(INCLUDE) -c Trie.cpp
Node.o: Node.cpp 
	$(CC) $(CFLAGS) -I$(INCLUDE) -c Node.cpp
clean:
	rm -f TrieTrest *.o 