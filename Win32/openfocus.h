/*
 * File: openfocus.h
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

#ifndef __hypnofocus_h_
#define __hypnofocus_h_ 

#include <inttypes.h>

#ifdef __WINDOWS__
#define DLLEXPORT __declspec(dllexport)
#else
#define DLLEXPORT
#endif

/*
 * Attempts to establish a connection with the focuser.
 *
 * Returns NO_ERROR if successful
 */
DLLEXPORT uint8_t focuser_connect(void);

/*
 * Disconnects the focuser 
 */
DLLEXPORT uint8_t focuser_disconnect(void);

/*
 * Returns the last error that was encountered
 */
DLLEXPORT uint8_t focuser_get_error(void);

/*
 * Moves to focuser to position <position>
 */
DLLEXPORT void focuser_move_to(uint16_t position);

/*
 * Returns true if focuser is moving
 */
DLLEXPORT uint8_t focuser_is_moving(void);

/*
 * Returns the current position of the focuser
 */
DLLEXPORT uint16_t focuser_get_position(void);

/*
 * Returns an 8 bit character with the capabilities of the focuser
 */
DLLEXPORT uint8_t focuser_get_capabilities(void);

/*
 * Halts the focuser
 */
DLLEXPORT void focuser_halt(void);

#endif /* __hypnofocus_h_ */
