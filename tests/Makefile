NAME = test

INCLUDES = -I../driver/libusb-win32 -I../driver -I../firmware
LIBS = -L../driver/libusb-win32 -lusb

CFLAGS = $(INCLUDES) -Wall -O -g
CC = gcc
OBJECTS = ../driver/openfocus.c $(NAME).o

PROGRAM = $(NAME)

all: $(PROGRAM)

.c.o:
	$(CC) $(CFLAGS) -c $< -o $@

$(PROGRAM): $(OBJECTS)
	$(CC) $(CFLAGS) -o $(PROGRAM) $(OBJECTS) $(LIBS)

clean:
	rm -rf *.o $(PROGRAM) *.exe
