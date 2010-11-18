/*
 * File: focuser.h
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
