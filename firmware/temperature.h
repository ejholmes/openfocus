/* 
 * temperature (kelvin) = ((5 * [adc val] * 100) / 1024)
 */


#ifndef __temperature_h_
#define __temperature_h_

#include <inttypes.h>

void temperature_init(int pin);
uint16_t temperature_read(uint8_t times);
uint16_t temperature_sample();

#endif
