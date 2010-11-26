NAME = openfocus

USBFLAGS = -Ilibusb-win32
USBLIBS = -Llibusb-win32 -lusb

CFLAGS = $(USBFLAGS) -I../firmware -shared -Wall -O -g
CC = gcc
OBJECTS = $(NAME).o

PROGRAM = $(NAME).dll

all: $(PROGRAM)

release: $(all)
	strip $(PROGRAM)

.c.o:
	$(CC) $(CFLAGS) -c $< -o $@

$(PROGRAM): $(OBJECTS)
	$(CC) $(CFLAGS) -o $(PROGRAM) $(OBJECTS) $(USBLIBS)

clean:
	rm -f *.o $(PROGRAM)
