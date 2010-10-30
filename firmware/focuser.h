/* Name: focuser.h
 * Project: Hypnofocus
 * Author: Eric Holmes
 * Creation Date: 2010-10-01
 * Copyright: (c) 2010 Eric Holmes
 * License: GNU GPL v2
 * $Id$
 */

#ifndef __focuser_h_
#define __focuser_h_

#include <inttypes.h>
#include <avr/io.h>
#include <util/delay.h>
#include <stdbool.h>
#include "requests.h"
#include "stepper.h"

void focuser_init(void);
int16_t focuser_move_to(uint16_t position);
void focuser_set_position(uint16_t position);
void focuser_halt();
bool focuser_is_moving();
uint16_t focuser_position();

#endif
