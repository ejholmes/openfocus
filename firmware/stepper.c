/* Name: stepper.c
 * Project: Hypnofocus
 * Author: Eric Holmes
 * Creation Date: 2010-10-01
 * Copyright: (c) 2010 Eric Holmes
 * License: GNU GPL v2
 * $Id$
 */

#include "stepper.h"
#include "util.h"


int8_t _direction              = FORWARD;
volatile uint16_t _steps_to_go = 0;
Callback step_event_cb         = NULL;
static uint8_t step            = 0;

void registerEventCallback(Callback cb){
    step_event_cb = cb;
}

/*
 * Initialize motor pins and enable pins, setup timer and enable global interrupts
 */
void stepper_init(void){
    /*Set pins to outputs*/
    MOTOR_DDR  |= _BV(MOTOR_PIN_A) | _BV(MOTOR_PIN_B) | _BV(MOTOR_PIN_C) | _BV(MOTOR_PIN_D); 

    /*Turn all pins off*/
    MOTOR_PINS_OFF();

    /*Make enable pins outputs*/
    PWM_DDR |= _BV(PWM1) | _BV(PWM2);

    /*Enable both output ports on the H-Bridge*/
    PWM_PORT |= _BV(PWM1) | _BV(PWM2);

    /*Setup timer with 1024 prescale*/
    TCCR0B |= _BV(CS02) | _BV(CS00); 

    /*Enable global interrupts */
    sei();
}

/*
 * Releases motor pins
 *
 * Some motors get pretty hot if the coils are kept activated
 * call this function to release the coils
 */
void stepper_release(void){
    MOTOR_PINS_OFF();
}


void stepper_stop(void){
    STOP_TIMER(); /* Stop overflow interrupt */
    _steps_to_go = 0; /* No more steps to go */
    step_event_cb(EVT_MOVE_COMPLETE, NULL); /* Let the event handler know that we're done */
}

/*
 * Essentially a wrapper for Stepper::oneStep()
 * Moves the desired steps and delays based on current speed
 */
void stepper_step(int16_t steps){
    if(steps > 0){
        _direction = FORWARD;
    }
    else if(steps < 0) {
        _direction = BACKWARD;
    }
    else {
        /* Steps must == 0, nothing to be done */
        return;
    }

    _steps_to_go = abs(steps);
    TCNT0 = 0;
    START_TIMER();
}

void stepper_do_step(int8_t direction){
    /*makes things cleaner..*/
    uint8_t a, b, c, d;
    a = _BV(MOTOR_PIN_A);
    b = _BV(MOTOR_PIN_B);
    c = _BV(MOTOR_PIN_C);
    d = _BV(MOTOR_PIN_D);

    /*determine what step we're on*/
    if((MOTOR_PIN & (a | b)) == (a | b)) /* at 1 */
        step = (direction == FORWARD)?2:0;
    else if ((MOTOR_PIN & (b | c)) == (b | c)) /* at 3 */
        step = (direction == FORWARD)?4:2;
    else if ((MOTOR_PIN & (c | d)) == (c | d)) /* at 5 */
        step = (direction == FORWARD)?6:4;
    else if ((MOTOR_PIN & (d | a)) == (d | a)) /* at 7 */
        step = (direction == FORWARD)?0:6;
    else if (MOTOR_PIN & a) /* at 0 */
        step = (direction == FORWARD)?1:7;
    else if (MOTOR_PIN & b) /* at 2 */
        step = (direction == FORWARD)?3:1;
    else if (MOTOR_PIN & c) /* at 4 */
        step = (direction == FORWARD)?5:3;
    else if (MOTOR_PIN & d) /* at 6 */
        step = (direction == FORWARD)?7:5;
    else { /* At an odd or unknown step, use last step as refernece for next step */
        if(step == 7 && direction == FORWARD)
            step = 0;
        else if (step == 0 && direction == BACKWARD)
            step = 7;
        else
            step = (direction == FORWARD)?step+1:step-1;
    }

    /*turn pins off*/
    MOTOR_PINS_OFF();

    /*then step*/
    switch(step){
        case 0:
            MOTOR_PORT |= a;
            break;
        case 1:
            MOTOR_PORT |= a | b;
            break;
        case 2:
            MOTOR_PORT |= b;
            break;
        case 3:
            MOTOR_PORT |= b | c;
            break;
        case 4:
            MOTOR_PORT |= c;
            break;
        case 5:
            MOTOR_PORT |= c | d;
            break;
        case 6:
            MOTOR_PORT |= d;
            break;
        case 7:
            MOTOR_PORT |= d | a;
            break;
    }

}

/*
 * Overflow interrupt that gets triggered consistently over a period of time
 * This handles the actually stepping of the motor so that it is not blocking
 *
 * Implements two callback events: EVT_STEP_COMPLETE and EVT_MOVE_COMPLETE
 */
ISR(TIMER0_OVF_vect){
    stepper_do_step(_direction); /* perform step */
    _steps_to_go--; /* Decrement step counter */
    step_event_cb(EVT_STEP_COMPLETE, &_direction); /* Call event handler for step complete */
    if(_steps_to_go == 0){
        stepper_stop(); /* Finished */
    }
};
