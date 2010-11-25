/*
 * File: focuser.c
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

#include <avr/io.h>
#include <inttypes.h>
#include <util/delay.h>

#include "focuser.h"

static uint16_t current_position = 0;
static int is_moving             = false;

/*
 * Handles events sent by the stepper motor
 */
void step_event_handler(uint8_t evt, void *data)
{
    switch(evt) {
        case EVT_STEP_COMPLETE:
            current_position += *(int8_t *)data;
            break;
        case EVT_MOVE_COMPLETE:
            stepper_release();
            is_moving = false;
            break;
    }
}

void focuser_init(void)
{
    stepper_init();
    registerEventCallback(&step_event_handler);
    focuser_set_position(0);
}

void focuser_halt(void)
{
    stepper_stop();
}

uint16_t focuser_get_position(void)
{
    return current_position;
}

int focuser_is_moving()
{
    return is_moving;
}

int16_t focuser_move_to(uint16_t position)
{
    int16_t steps_to_move = (int16_t)(position - current_position);
    if (steps_to_move != 0) {
        if (is_moving) {
            stepper_stop();
            while (is_moving)
                _delay_ms(1);
        }
        is_moving = true;
        stepper_step(steps_to_move);
    }
    return steps_to_move;
}

void focuser_set_position(uint16_t position)
{
    current_position = position;
}
