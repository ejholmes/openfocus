NAME = focus

OBJECTS = ../lib/opendevice.o $(NAME).o

#INCLUDES = -I../firmware
#LIBS     = -L../lib -lusb
INCLUDES = `libusb-config --cflags` -I../firmware
LIBS	 = `libusb-config --libs`
CC       = gcc
CFLAGS   = $(CPPFLAGS) $(INCLUDES) -O -g -Wall

PROGRAM = $(NAME)


all: $(PROGRAM)

.c.o:
	$(CC) $(CFLAGS) -c $< -o $@

$(PROGRAM): $(OBJECTS)
	$(CC) $(CFLAGS) -o $(PROGRAM) $(OBJECTS) $(LIBS)

strip: $(PROGRAM)
	strip $(PROGRAM)

clean:
	rm -f *.o $(PROGRAM)
