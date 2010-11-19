/*
 * File: openfocus.h
 * Package: OpenFocus
 *
 * Copyright (c) 2010 Eric J. Holmes
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General
 * Public License along with this library; if not, write to the
 * Free Software Foundation, Inc., 59 Temple Place, Suite 330,
 * Boston, MA  02111-1307  USA
 *
 */

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
uint8_t usbOpenDevice(usb_dev_handle **device, int vendorID, char *vendorNamePattern, int productID, char *productNamePattern, char *serialNamePattern);
int usbGetStringAscii(usb_dev_handle *dev, int index, char *buf, int buflen);

#endif /* __hypnofocus_h_ */
