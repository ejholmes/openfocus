NAME = hypnofocus

ifeq (`which libusb-config`, '')
	USBFLAGS = -I../libusb
	USBLIBS = -L../libusb -lusb
else
	USBFLAGS = `libusb-config --cflags`
	USBLIBS = `libusb-config --libs`
endif
CFLAGS = $(USBFLAGS) -I../firmware -shared -Wall -O -g
CC = gcc
OBJECTS = $(NAME).o

PROGRAM = $(NAME).dll

all: $(PROGRAM)

.c.o:
	$(CC) $(CFLAGS) -c $< -o $@

$(PROGRAM): $(OBJECTS)
	$(CC) $(CFLAGS) -o $(PROGRAM) $(OBJECTS) $(USBLIBS)

clean:
	rm -f *.o $(PROGRAM)
