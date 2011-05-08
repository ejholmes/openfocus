/*
 * File: temperature.c
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

#include "temperature.h"

void temperature_init(int pin)
{
    ADMUX = _BV(REFS0) | (0x0f & pin); /* Select AVcc and select channel. Mask pin to 4 bits */

    ADCSRA |= _BV(ADPS2) | _BV(ADPS1) | _BV(ADPS0); /* Set prescaller to 128 */

    ADCSRA |= _BV(ADEN); /* Enable ADC */
}

/*
 * times should not be greater than 64
 */
uint16_t temperature_read(uint8_t times)
{
    times = 0x3f & times; /* Mask to 6 bits */
    int i;
    uint16_t current = 0;
    for (i = 0; i < times; i++) {
        current += temperature_sample();
    }

    return current / times;
}

uint16_t temperature_sample()
{
    ADCSRA |= _BV(ADSC); /* Start ADC conversion */

    /* ADSC = 1 while a conversion is in progress */
    while ((ADCSRA & _BV(ADSC)) == _BV(ADSC));

    uint8_t lsb = ADCL; /* Read LSB */
    uint8_t msb = ADCH; /* Read MSB */

    return (msb << 8) | lsb;
}
