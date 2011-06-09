#include <avr/io.h>
#include <inttypes.h>
#include <util/delay.h>

#include "focuser.h"

static uint16_t current_position = 0;
static uint8_t is_moving         = false;
static uint8_t duty_cycle        = 0;

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
            if (duty_cycle == 0)
                stepper_release();
            else
                stepper_pwm_hold(duty_cycle);
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

int focuser_set_pwm_holding(uint8_t duty)
{
    duty_cycle = duty;

    if (duty_cycle == 0) {
        stepper_release();
    }
    else {
        stepper_pwm_hold(duty_cycle);
    }

    return duty_cycle;
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
