/* Name: focuser.c
 * Project: Hypnofocus
 * Author: Eric Holmes
 * Creation Date: 2010-10-01
 * Copyright: (c) 2010 Eric Holmes
 * License: GNU GPL v2
 * $Id$
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
