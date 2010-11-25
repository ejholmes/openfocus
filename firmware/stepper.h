/*
 * File: stepper.h
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
 */


#ifndef __stepper_h_
#define __stepper_h_

#ifndef STEPS_PER_REV
#define STEPS_PER_REV 400
#endif

/* Motor data direction ports */
#define MOTOR_DDR   DDRD
#define MOTOR_PORT  PORTD
#define MOTOR_PIN   PIND


/* Motor pins */
#define MOTOR_PIN_A PD3   /* green */
#define MOTOR_PIN_B PD5   /* yellow */
#define MOTOR_PIN_C PD4   /* red */
#define MOTOR_PIN_D PD6   /* blue */


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

typedef void(*Callback)(uint8_t, void*);

/*
 * Initialize motor pins and enable pins, setup timer and enable global interrupts
 */
void stepper_init(void);

/*
 * Essentially a wrapper for Stepper::oneStep()
 * Moves the desired steps and delays based on current speed
 */
void stepper_step(int16_t steps);

/*
 * Releases motor pins
 *
 * Some motors get pretty hot if the coils are kept activated
 * call this function to release the coils
 */
void stepper_release(void);

/*
 * Stops the stepper motor
 */
void stepper_stop(void);

void registerEventCallback(Callback cb);

#endif
