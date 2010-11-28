NAME = focus

UNAME := $(shell uname)

ifeq ($(UNAME), Darwin)
	USBFLAGS := $(shell libusb-config --cflags)
	USBLIBS := $(shell libusb-config --libs)
else
	USBFLAGS = -I../driver/libusb-win32
	USBLIBS = -L../driver/libusb-win32 -lusb
endif

CFLAGS = $(USBFLAGS) -I../driver -I../firmware -shared -Wall -O -g
LIBS = $(USBLIBS)
CC = gcc
OBJECTS = ../driver/openfocus.o $(NAME).o

PROGRAM = $(NAME)

all: $(PROGRAM)

.c.o:
	$(CC) $(CFLAGS) -c $< -o $@

$(PROGRAM): $(OBJECTS)
	$(CC) $(CFLAGS) -o $(PROGRAM) $(OBJECTS) $(LIBS)

clean:
	rm -rf *.o $(PROGRAM) $(NAME).dS*
