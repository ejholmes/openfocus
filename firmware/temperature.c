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
