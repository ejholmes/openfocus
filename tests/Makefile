NAME = test

USBFLAGS = `libusb-config --cflags`
USBLIBS = `libusb-config --libs`

LIBS = $(USBLIBS)
CFLAGS = $(USBFLAGS) -I../driver -I../firmware -Wall -O -g

CC = gcc
OBJECTS = ../driver/openfocus.c $(NAME).o

PROGRAM = $(NAME)

all: $(PROGRAM)

.c.o:
	$(CC) $(CFLAGS) -c $< -o $@

$(PROGRAM): $(OBJECTS)
	$(CC) $(CFLAGS) -o $(PROGRAM) $(OBJECTS) $(LIBS)

clean:
	rm -rf *.o $(PROGRAM) *.exe $(NAME).dSYM
