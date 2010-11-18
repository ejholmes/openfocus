NAME = focus

INCLUDES = `libusb-config --cflags` -I../win32 -I../firmware
LIBS = `libusb-config --libs`
CFLAGS = $(USBFLAGS) $(INCLUDES) -shared -Wall -O -g
CC = gcc
OBJECTS = ../Win32/hypnofocus.c $(NAME).o

PROGRAM = $(NAME)

all: $(PROGRAM)

.c.o:
	$(CC) $(CFLAGS) -c $< -o $@

$(PROGRAM): $(OBJECTS)
	$(CC) $(CFLAGS) -o $(PROGRAM) $(OBJECTS) $(LIBS)

clean:
	rm -f *.o $(PROGRAM)
