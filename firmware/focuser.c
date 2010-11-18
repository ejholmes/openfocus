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


#include "focuser.h"

static uint16_t _current_position = 0;
static bool _moving               = false;

/*
 * Handles events sent by the stepper motor
 */
void step_event_handler(uint8_t evt, void *data){
    switch(evt){
        case EVT_STEP_COMPLETE:
            _current_position += *(int8_t *)data;
            break;
        case EVT_MOVE_COMPLETE:
            stepper_release();
            _moving = false;
            break;
    }
}

/*
 * Setup stepper motor and register callbacks 
 */
void focuser_init(void){
    stepper_init();
    registerEventCallback(&step_event_handler);
    focuser_set_position(0);
}

/*
 * Stops the focuser
 */
void focuser_halt(void){
    stepper_stop();
}

/*
 * Returns the current position of the focuser
 */
uint16_t focuser_position(void){
    return _current_position;
}

/*
 * Returns true if the focuser is currently moving to a position, else false
 */
bool focuser_is_moving(){
    return _moving;
}

/*
 * Moves the focuser to a set position
 */
int16_t focuser_move_to(uint16_t position){
    int16_t steps_to_move = (int16_t)(position - _current_position);
    if(steps_to_move != 0){
        if(_moving){
            stepper_stop();
            while(_moving)
                _delay_ms(1);
        }
        _moving = true;
        stepper_step(steps_to_move);
    }
    return steps_to_move;
}

/*
 * Sets the focusers currrent position
 */
void focuser_set_position(uint16_t position){
    _current_position = position;
}
