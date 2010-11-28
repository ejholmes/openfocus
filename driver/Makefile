NAME = openfocus

UNAME := $(shell uname)

ifeq ($(UNAME), Darwin)
	USBFLAGS := $(shell libusb-config --cflags) -fPIC
	USBLIBS := $(shell libusb-config --libs) -W1,-soname,$(NAME).so.1
	PROGRAM = $(NAME).so
else
	USBFLAGS = -Ilibusb-win32
	USBLIBS = -Llibusb-win32 -lusb
	PROGRAM = $(NAME).dll
endif

CFLAGS = $(USBFLAGS) -I../firmware -shared -Wall -O -g
CC = gcc
OBJECTS = $(NAME).o

all: $(PROGRAM)

release: $(all)
	strip $(PROGRAM)

.c.o:
	$(CC) $(CFLAGS) -c $< -o $@

$(PROGRAM): $(OBJECTS)
	$(CC) $(CFLAGS) -o $(PROGRAM) $(OBJECTS) $(USBLIBS)

clean:
	rm -f *.o $(PROGRAM)
