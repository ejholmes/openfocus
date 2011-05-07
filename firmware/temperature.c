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

void temperature_init(int pin)
{
    /* ADMUX = _BV(REFS0) | _BV(MUX0); [> Set voltage reference to AVcc with external cap on AREF <] */
    ADMUX = _BV(REFS0);

    ADCSRA |= _BV(ADPS2) | _BV(ADPS1) | _BV(ADPS0); /* Set prescaller to 128 */

    ADCSRA |= _BV(ADEN); /* Enable ADC */
}

uint16_t temperature_read()
{
    ADCSRA |= _BV(ADSC); /* Start ADC conversion */

    /* ADSC = 1 while a conversion is in progress */
    while ((ADCSRA & _BV(ADSC)) == _BV(ADSC));

    uint8_t lsb = ADCL; /* Read LSB */
    uint8_t msb = ADCH; /* Read MSB */

    return (msb << 8) | lsb;

    /* return count; */
}
