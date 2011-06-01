#ifndef __util_h_
#define __util_h_

/*
 * Delays for "ms" ms
 */
void delay(uint16_t ms);

/*
 * Returns a 16 bit unsigned int from lsb and msb
 */
uint16_t make_uint(uint8_t lsb, uint8_t msb);

/*
 * Returns the least significant bit of val
 */
uint8_t lsb(uint16_t val);

/*
 * Returns the most significant bit of val
 */
uint8_t msb(uint16_t val);

#endif
