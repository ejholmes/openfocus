/* Name: util.c
 * Project: Hypnofocus
 * Author: Eric Holmes
 * Creation Date: 2010-10-01
 * Copyright: (c) 2010 Eric Holmes
 * License: GNU GPL v2
 * $Id$
 */

#include <stdlib.h>
#include "util.h"

/*
 * Delays for "ms" ms
 */
void delay(uint16_t ms){
    while(ms--){
        _delay_ms(1);
    }
}

uint16_t make_uint(uint8_t lsb, uint8_t msb){
    return (msb << 8) | lsb;
}

uint8_t lsb(uint16_t val){
    return val & 0xff;
}

uint8_t msb(uint16_t val){
    return val >> 8;
}
