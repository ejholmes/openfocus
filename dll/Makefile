NAME = hypnofocus

INCLUDES = -I../include -I../firmware
LIBS = -L../lib -lusb
CFLAGS = $(USBFLAGS) $(INCLUDES) -shared -Wall -O -g
CC = gcc
OBJECTS = ../lib/opendevice.o $(NAME).o

PROGRAM = $(NAME).dll

all: $(PROGRAM)

.c.o:
	$(CC) $(CFLAGS) -c $< -o $@

$(PROGRAM): $(OBJECTS)
	$(CC) $(CFLAGS) -o $(PROGRAM) $(OBJECTS) $(LIBS)

clean:
	rm -f *.o $(PROGRAM)
