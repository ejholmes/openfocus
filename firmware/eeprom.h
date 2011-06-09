#ifndef __eeprom_h_
#define __eeprom_h_ 

#include <inttypes.h>
#include <avr/eeprom.h>
#include <avr/wdt.h>
#include <util/delay.h>

#include "usbdrv.h"

#define MAX_SERIAL_LEN 255

/* EEPROM Address Map */
#define EEPROM_ADDRESS_STAY_IN_BOOTLOADER   0x0000
#define EEPROM_ADDRESS_SERIAL_NUMBER_LEN 	0x0040
#define EEPROM_ADDRESS_SERIAL_NUMBER     	0x0041

#define eeprom_read_stay_in_bootloader() eeprom_read_byte((const uint8_t*)EEPROM_ADDRESS_STAY_IN_BOOTLOADER)
#define eeprom_set_stay_in_bootloader() eeprom_write_byte((uint8_t*)EEPROM_ADDRESS_STAY_IN_BOOTLOADER, 1)
#define eeprom_clear_stay_in_bootloader() eeprom_write_byte((uint8_t*)EEPROM_ADDRESS_STAY_IN_BOOTLOADER, 0)
#define reboot_to_bootloader() eeprom_set_stay_in_bootloader(); \
        wdt_enable(WDTO_15MS); \
        _delay_ms(14)


#endif /* __eeprom_h_ */
