/*
 * File: config.h
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

#ifdef __CONFIG_H_
#define __CONFIG_H_ 

/* Circuit board revision */
#ifndef BRD_REV
    #define BRD_REV 4
#endif

/* Number of times the sensor is sampled each request */
#ifndef TEMP_SENSOR_COUNT
    #define TEMP_SENSOR_COUNT 3
#endif

/* ADC pin that sensor is on. Can be 0-7. (ex. ADC0 = 0, ADC1 = 1) */
#ifndef TEMP_SENSOR_PIN
    #define TEMP_SENSOR_PIN 0
#endif

#endif /* __CONFIG_H_ */
