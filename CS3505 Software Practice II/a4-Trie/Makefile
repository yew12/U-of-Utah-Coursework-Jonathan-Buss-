CC = g++ -g -fsanitize=undefined -fsanitize=address
CFLAGS = -std=c++14 -Wall
GOOGLETEST = /googletest


TrieTests: TrieTests.o Trie.o Node.o 
	$(CC) $(CFLAGS) -o TrieTests -I$(GOOGLETEST)/googletest/include TrieTests.o Trie.o Node.o -L$(GOOGLETEST)/lib -lgtest_main -lgtest -lpthread

TrieTests.o: TrieTests.cpp Trie.o Node.o
	$(CC) $(CFLAGS) -I$(GOOGLETEST)/googletest/include -c TrieTests.cpp 

Trie.o: Trie.h Trie.cpp 
	$(CC) $(CFLAGS) -c Trie.cpp 

Node.o: Node.h Node.cpp
	$(CC) $(CFLAGS) -c Node.cpp

clean: 
	rm -f TrieTests *.o
