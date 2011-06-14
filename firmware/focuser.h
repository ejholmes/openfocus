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

/*
 * Reverses the direction that the focuser moves
 */
void focuser_reverse(uint8_t reverse);

#endif
