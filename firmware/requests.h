/* Name: requests.h
 * Project: Hypnofocus
 * Author: Eric Holmes
 * Creation Date: 2010-10-01
 * Copyright: (c) 2010 Eric Holmes
 * License: GNU GPL v2
 * $Id$
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
