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
#include "requests.h"
#include "stepper.h"

#define true 1
#define false 0

/*
 * Setup stepper motor and register callbacks 
 */
void focuser_init(void);

/*
 * Moves the focuser to a set position
 */
int16_t focuser_move_to(uint16_t position);

/*
 * Sets the focusers currrent position
 */
void focuser_set_position(uint16_t position);

/*
 * Stops the focuser
 */
void focuser_halt();

/*
 * Returns true if the focuser is currently moving to a position, else false
 */
int focuser_is_moving();

/*
 * Returns the current position of the focuser
 */
uint16_t focuser_get_position();

#endif
