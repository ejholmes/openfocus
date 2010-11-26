NAME = focus

INCLUDES = `libusb-config --cflags` -I../win32 -I../firmware
LIBS = `libusb-config --libs`
CFLAGS = $(USBFLAGS) $(INCLUDES) -shared -Wall -O -g
CC = gcc
OBJECTS = ../win32/openfocus.c $(NAME).o

PROGRAM = $(NAME)

all: $(PROGRAM)

.c.o:
	$(CC) $(CFLAGS) -c $< -o $@

$(PROGRAM): $(OBJECTS)
	$(CC) $(CFLAGS) -o $(PROGRAM) $(OBJECTS) $(LIBS)

clean:
	rm -rf *.o $(PROGRAM) $(NAME).dS*
