/* Name: util.h
 * Project: Hypnofocus
 * Author: Eric Holmes
 * Creation Date: 2010-10-01
 * Copyright: (c) 2010 Eric Holmes
 * License: GNU GPL v2
 * $Id$
 */

#ifndef __util_h_
#define __util_h_

#include <inttypes.h>
#include <util/delay.h>

void delay(uint16_t ms);
uint16_t make_uint(uint8_t lsb, uint8_t msb);
uint8_t lsb(uint16_t val);
uint8_t msb(uint16_t val);

#endif
