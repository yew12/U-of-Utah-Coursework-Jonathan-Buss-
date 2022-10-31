# use variables to define compilation 
# properties
CC = g++
CFLAGS = -Wall
LIBHARU = ./libharu

spiralPDF: spiralPDF.o HaruPDF.o Spiral.o
	$(CC) $(CFLAGS) -I$(LIBHARU)/include -o spiralPDF spiralPDF.o HaruPDF.o Spiral.o -L$(LIBHARU)/src -lhpdfs
spiralPDF.o: spiralPDF.cpp HaruPDF.o
	$(CC) $(CFLAGS) -I$(LIBHARU)/include -c spiralPDF.cpp 
HaruPDF.o: HaruPDF.h HaruPDF.cpp 
	$(CC) $(CFLAGS) -I$(LIBHARU)/include -c HaruPDF.cpp  
Spiral.o: Spiral.h Spiral.cpp 
	$(CC) $(CFLAGS) -c Spiral.cpp  
clean:
	rm -f spiralPDF *.o 