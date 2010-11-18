/*
 * File: requests.h
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


#ifndef __requests_h
#define __requests_h

/* Focuser commands */
#define FOCUSER_MOVE_TO          0x00
#define FOCUSER_HALT             0x01
#define FOCUSER_GET_POSITION     0x10
#define FOCUSER_IS_MOVING        0x11
#define FOCUSER_GET_CAPABILITIES 0x12

/* Capabilities */
#define CAP_ABSOLUTE  0x01 /* _BV(0) */
#define CAP_TEMP_COMP 0x02 /* _BV(1) */

#endif
