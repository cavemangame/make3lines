CPP=g++

CFLAGS= -I ../../include/
SOURCES=test.cpp
OBJECTS=test.o
LIBS= ../../lib/gcc/libhge.a 

all: test.exe

clean:
	del *.o *.exe
	
test.exe: $(OBJECTS)
	$(CPP) -o $@ $(OBJECTS) $(LIBS)
	
%.o: %.cpp
	$(CPP) -c $(SOURCES)
