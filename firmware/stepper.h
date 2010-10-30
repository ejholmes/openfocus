/* Name: stepper.h
 * Project: Hypnofocus
 * Author: Eric Holmes
 * Creation Date: 2010-10-01
 * Copyright: (c) 2010 Eric Holmes
 * License: GNU GPL v2
 * $Id$
 */

/*
 *         SN754410/L293D                
 *                                       
 *            +--v--+                    
 *  PB1 en1,2 |     | Vcc (Logic)                
 *    PD3  1A |     | 4A  PD5               
 * (Green) 1Y |     | 4Y (Yellow)         
 *        GND |     | GND                
 *        GND |     | GND                
 *   (Red) 2Y |     | 3Y (Blue)           
 *    PD4  2A |     | 3A  PD6               
 *        Vcc |     | en3,4  PB2         
 *            +-----+                    
 *
 *
 *
 *
 *               Stepper Motor
 *
 *
 *      Red -------+  
 *                 |
 *    Green -------+
 *                   +---+                           
 *                   |   |              
 *                   |   |              
 *                   |   |              
 *              Yellow   Blue                           
 *                                       
 *
 * Coil Activation Sequence
 * --------------------------
 * Red -> Yellow -> Green -> Blue
 * Red -> Red/Yellow -> Yellow -> Yellow/Green -> Green -> Green/Blue -> Blue
 *
 * Stepper Pairs
 * -------------------
 * Red/Green
 * Yellow/Blue
 *
 * TODO:
 * PWM Holding - PWM enable ports for holding heavier focus loads
 *
 */


#ifndef __stepper_h_
#define __stepper_h_

#define STEPS_PER_REV 400

/* Motor data direction ports */
#define MOTOR_DDR   DDRD
#define MOTOR_PORT  PORTD
#define MOTOR_PIN   PIND


/* Motor pins */
#define MOTOR_PIN_A PD3   // green
#define MOTOR_PIN_B PD5   // yellow
#define MOTOR_PIN_C PD4   // red
#define MOTOR_PIN_D PD6   // blue


/* PWM/Enable Ports */
#define PWM_DDR     DDRB
#define PWM_PORT    PORTB
#define PWM_PIN     PINB

/* PWM/Enable pins */
#define PWM1        PB1
#define PWM2        PB2

/* Helper macro for turning off motor pins */
#define MOTOR_PINS_OFF() MOTOR_PORT &= ~(_BV(MOTOR_PIN_A) | _BV(MOTOR_PIN_B) | _BV(MOTOR_PIN_C) | _BV(MOTOR_PIN_D))

#define STOP_TIMER() TIMSK0 &= ~_BV(TOIE0)
#define START_TIMER() TIMSK0 |= _BV(TOIE0)

/* Movement Directions */
#define FORWARD           1
#define BACKWARD          -1

/* Events */
#define EVT_STEP_COMPLETE  0x00
#define EVT_MOVE_COMPLETE  0x01

#include <inttypes.h>
#include <avr/io.h>
#include <avr/interrupt.h>
#include <stdlib.h>

typedef void(*Callback)(uint8_t, void*);

void stepper_init(void);
void stepper_step(int16_t steps);
void stepper_release(void);
void stepper_stop(void);
void registerEventCallback(Callback cb);

#endif
