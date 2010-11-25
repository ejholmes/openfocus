/*
 * File: util.h
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
