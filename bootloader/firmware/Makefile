# Name: Makefile
# Author: Eric J. Holmes
# Copyright: Copyright (c) 2010 Eric J. Holmes
# License: GPL v2.1

# This is a prototype Makefile. Modify it according to your needs.
# You should at least check the settings for
# DEVICE ....... The AVR device you compile for
# CLOCK ........ Target AVR clock rate in Hertz
# OBJECTS ...... The object files created from your source files. This list is
#                usually the same as the list of source files with suffix ".o".
# PROGRAMMER ... Options to avrdude which define the hardware you use for
#                uploading to the AVR and the interface where this hardware
#                is connected.
# FUSES ........ Parameters for avrdude to flash the fuses appropriately.

DEVICE     = atmega328p
CLOCK      = 16000000
PORT       = com5
PROGRAMMER = -c avrisp
OBJECTS    = usbdrv/usbdrv.o usbdrv/usbdrvasm.o usbdrv/oddebug.o main.o
FUSES      = -U hfuse:w:0xda:m -U lfuse:w:0xff:m
ADDRESS    = 7800
LDFLAGS   += -Wl,--relax,--gc-sections -Wl,--section-start=.text=$(ADDRESS)
CFLAGS 	   = 

AVRDUDE = avrdude $(PROGRAMMER) -p $(DEVICE) -P $(PORT) -b 19200
COMPILE = avr-gcc -Wall -Os -Iusbdrv -I. -DF_CPU=$(CLOCK) -mmcu=$(DEVICE) -DDEBUG_LEVEL=0 $(CFLAGS)

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

# Xcode uses the Makefile targets "", "clean" and "install"
install: flash fuse

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
