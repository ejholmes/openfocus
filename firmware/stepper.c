#include <inttypes.h>
#include <avr/io.h>
#include <avr/interrupt.h>
#include <stdlib.h>

#include "config.h"
#include "stepper.h"
#include "util.h"


int8_t direction              = FORWARD;
uint8_t reversed              = 0;  
volatile uint16_t steps_to_go = 0;
static uint8_t step           = 0;
Callback step_event_cb        = NULL;

void registerEventCallback(Callback cb)
{
    step_event_cb = cb;
}

void stepper_init(void)
{
    MOTOR_DDR  |= _BV(MOTOR_PIN_A) 
        | _BV(MOTOR_PIN_B) 
        | _BV(MOTOR_PIN_C) 
        | _BV(MOTOR_PIN_D);                     /* Set pins to outputs */

    MOTOR_PINS_OFF();                           /* Turn all pins off */

    PWM_DDR |= _BV(PWM1) | _BV(PWM2);           /* Make enable pins outputs */

    PWM_PORT |= _BV(PWM1) | _BV(PWM2);          /* Enable both output ports on the H-Bridge */

    TCCR0B |= _BV(CS02) | _BV(CS00);            /* Setup timer with 1024 prescale */

    sei();                                      /* Enable global interrupts */
}

void stepper_release(void)
{
    MOTOR_PINS_OFF();
}


void stepper_stop(void)
{
    STOP_TIMER();                               /* Stop overflow interrupt */
    steps_to_go = 0;                            /* No more steps to go */
    step_event_cb(EVT_MOVE_COMPLETE, NULL);     /* Let the event handler know that we're done */
}

void stepper_reverse(uint8_t reverse)
{
    reversed = reverse;
}

void stepper_step(int16_t steps)
{
    if (steps > 0) {
        direction = (reversed)?BACKWARD:FORWARD;
    }
    else if (steps < 0) {
        direction = (reversed)?FORWARD:BACKWARD;
    }
    else {                                      /* Steps must == 0, nothing to be done */
        return;
    }

    steps_to_go = abs(steps);
    TCNT0 = 0;
    START_TIMER();
}

void stepper_do_step(int8_t direction)
{
    /* makes things cleaner.. */
    uint8_t a, b, c, d;
    a = _BV(MOTOR_PIN_A);
    b = _BV(MOTOR_PIN_B);
    c = _BV(MOTOR_PIN_C);
    d = _BV(MOTOR_PIN_D);

    /* determine what step we're on */
    if ((MOTOR_PIN & (a | b)) == (a | b))           /* At 1 */
        step = (direction == FORWARD) ? 2 : 0;
    else if ((MOTOR_PIN & (b | c)) == (b | c))      /* At 3 */
        step = (direction == FORWARD) ? 4 : 2;
    else if ((MOTOR_PIN & (c | d)) == (c | d))      /* At 5 */
        step = (direction == FORWARD) ? 6 : 4;
    else if ((MOTOR_PIN & (d | a)) == (d | a))      /* At 7 */
        step = (direction == FORWARD) ? 0 : 6;
    else if (MOTOR_PIN & a)                         /* At 0 */
        step = (direction == FORWARD) ? 1 : 7;
    else if (MOTOR_PIN & b)                         /* At 2 */
        step = (direction == FORWARD) ? 3 : 1;
    else if (MOTOR_PIN & c)                         /* At 4 */
        step = (direction == FORWARD) ? 5 : 3;
    else if (MOTOR_PIN & d)                         /* At 6 */
        step = (direction == FORWARD) ? 7 : 5;
    else {                                          /* At an odd or unknown step, use last step as refernece for next step */
        if ((step == 7) && (direction == FORWARD))
            step = 0;
        else if ((step == 0) && (direction == BACKWARD))
            step = 7;
        else
            step = (direction == FORWARD) ? (step + 1) : (step - 1);
    }

    switch(step) {              /* Then step */
        case 0: /* a on */
            if (direction == FORWARD)
                MOTOR_PORT &= ~d; 
            else
                MOTOR_PORT &= ~b;
            break;
        case 1: /* a and b on */
            if (direction == FORWARD)
                MOTOR_PORT |= b; 
            else
                MOTOR_PORT |= a;
            break;
        case 2: /* b on */
            if (direction == FORWARD)
                MOTOR_PORT &= ~a;
            else
                MOTOR_PORT &= ~c;
            break;
        case 3: /* b and c on*/
            if (direction == FORWARD)
                MOTOR_PORT |= c;
            else
                MOTOR_PORT |= b;
            break;
        case 4: /* c on */
            if (direction == FORWARD)
                MOTOR_PORT &= ~b;
            else
                MOTOR_PORT &= ~d;
            break;
        case 5: /* c and d on */
            if (direction == FORWARD)
                MOTOR_PORT |= d;
            else
                MOTOR_PORT |= c;
            break;
        case 6: /* d on */
            if (direction == FORWARD)
                MOTOR_PORT &= ~c;
            else
                MOTOR_PORT &= ~a;
            break;
        case 7: /* d and a on */
            if (direction == FORWARD)
                MOTOR_PORT |= a;
            else
                MOTOR_PORT |= d;
            break;
    }

}

/*
 * Overflow interrupt that gets triggered consistently over a period of time
 * This handles the actually stepping of the motor so that it is not blocking
 */
ISR(TIMER0_OVF_vect)
{
    stepper_do_step(direction);                         /* Perform step */
    steps_to_go--;                                      /* Decrement step counter */
    step_event_cb(EVT_STEP_COMPLETE, &direction);       /* Call event handler for step complete */
    if (steps_to_go == 0) {
        stepper_stop();                                 /* Finished */
    }
};
