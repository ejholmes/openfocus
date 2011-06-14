#ifndef __requests_h
#define __requests_h

/* Focuser commands */
#define FOCUSER_MOVE_TO                      0x00
#define FOCUSER_HALT                         0x01
#define FOCUSER_SET_POSITION                 0x02
#define FOCUSER_REVERSE                      0x03
#define REBOOT_TO_BOOTLOADER				 0x04
#define FOCUSER_GET_POSITION                 0x10
#define FOCUSER_IS_MOVING                    0x11
#define FOCUSER_GET_CAPABILITIES             0x12
#define FOCUSER_GET_TEMPERATURE              0x13

/* Capabilities */
#define CAP_ABSOLUTE  0x01 /* _BV(0) */
#define CAP_TEMP_COMP 0x02 /* _BV(1) */

#endif
