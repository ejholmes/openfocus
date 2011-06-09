# Name: Makefile
# Author: Eric J. Holmes
# Copyright: Copyright (c) 2010 Eric J. Holmes

DEVICE     = atmega328p
CLOCK      = 16000000
PORT       = com5
PROGRAMMER = -c avrisp
OBJECTS    = ../usbdrv/usbdrv.o ../usbdrv/usbdrvasm.o ../usbdrv/oddebug.o main.o
FUSES      = -U hfuse:w:0xc8:m -U lfuse:w:0xff:m
# For atmega328p, the bootloader address for a 2048 word boot section is at address 7000
ADDRESS    = 7000
LDFLAGS   += -Wl,--relax,--gc-sections -Wl,--section-start=.text=$(ADDRESS)
CFLAGS 	   = 

AVRDUDE = avrdude $(PROGRAMMER) -p $(DEVICE) -P $(PORT) -b 19200
COMPILE = avr-gcc -Wall -Os -I../usbdrv -I. -DF_CPU=$(CLOCK) -mmcu=$(DEVICE) -DDEBUG_LEVEL=0 $(CFLAGS)

# symbolic targets:
all:	main.hex

.cpp.o:
	$(COMPILE) -c $< -o $@

.c.o:
	$(COMPILE) -c $< -o $@

.S.o:
	$(COMPILE) -x assembler-with-cpp -c $< -o $@
# "-x assembler-with-cpp" should not be necessary since this is the default
# file type for the .S (with capital S) extension. However, upper case
# characters are not always preserved on Windows. To ensure WinAVR
# compatibility define the file type manually.

.c.s:
	$(COMPILE) -S $< -o $@

flash:	all
	$(AVRDUDE) -U flash:w:main.hex:i

fuse:
	$(AVRDUDE) $(FUSES)
	
lock:
	$(AVRDUDE) -U lock:w:0x2f:m
	
eeprom:
	$(AVRDUDE) -U eeprom:w:eeprom.hex:i

readeeprom:
	$(AVRDUDE) -U eeprom:r:readeeprom.hex:i

# Xcode uses the Makefile targets "", "clean" and "install"
install: flash fuse lock

clean:
	rm -f main.hex main.elf $(OBJECTS)

# file targets:
main.elf: $(OBJECTS)
	$(COMPILE) -o main.elf $(OBJECTS) $(LDFLAGS)

main.hex: main.elf
	rm -f main.hex
	avr-objcopy -j .text -j .data -O ihex main.elf main.hex
	avr-size main.hex
# If you have an EEPROM section, you must also create a hex file for the
# EEPROM and add it to the "flash" target.

# Targets for code debugging and analysis:
disasm:	main.elf
	avr-objdump -d main.elf

cpp:
	$(COMPILE) -E main.c
