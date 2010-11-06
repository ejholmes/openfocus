#ifndef __hypnofocus_h_
#define __hypnofocus_h_ 

#include <stdlib.h>
#include <string.h>
#include <stdio.h>
#include <inttypes.h>
#include <usb.h>

#include "errors.h"
#include "usbconfig.h"
#include "requests.h"

#ifdef __WINDOWS__
#define DLLEXPORT __declspec(dllexport)
#else
#define DLLEXPORT
#endif

DLLEXPORT uint8_t focuser_connect(void);
DLLEXPORT uint8_t focuser_disconnect(void);
DLLEXPORT uint8_t focuser_get_error(void);
DLLEXPORT void focuser_move_to(uint16_t position);
DLLEXPORT uint8_t focuser_is_moving(void);
DLLEXPORT uint16_t focuser_get_position(void);
DLLEXPORT uint8_t focuser_get_capabilities(void);
DLLEXPORT void focuser_halt(void);
int usbOpenDevice(usb_dev_handle **device, int vendorID, char *vendorNamePattern, int productID, char *productNamePattern, char *serialNamePattern, FILE *printMatchingDevicesFp, FILE *warningsFp);

#endif /* __hypnofocus_h_ */
