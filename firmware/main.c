/*
 * File: main.c
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
#include <avr/wdt.h>
#include <avr/interrupt.h>
#include <avr/pgmspace.h>
#include <util/delay.h>

#include "usbdrv.h"
#include "focuser.h"
#include "util.h"

const uint8_t capabilities = CAP_ABSOLUTE;

usbMsgLen_t usbFunctionSetup(uchar data[8])
{
    usbRequest_t *rq = (void *)data;

    if (rq->bRequest == FOCUSER_MOVE_TO) {
        uint8_t l = rq->wValue.bytes[0];
        uint8_t h = rq->wValue.bytes[1];
        focuser_move_to(make_uint(l, h));
        return 0;
    }
    else if (rq->bRequest == FOCUSER_HALT) {
        focuser_halt();
        return 0;
    }
    else if (rq->bRequest == FOCUSER_GET_POSITION) {
        uint16_t position = focuser_get_position();
        static uchar buffer[2];
        buffer[0] = lsb(position);
        buffer[1] = msb(position);
        usbMsgPtr = buffer;
        return sizeof(buffer);
    }
    else if (rq->bRequest == FOCUSER_SET_POSITION) {
        uint8_t l = rq->wValue.bytes[0];
        uint8_t h = rq->wValue.bytes[1];
        focuser_set_position(make_uint(l, h));
        return 0;
    }
    else if (rq->bRequest == FOCUSER_IS_MOVING) {
        static uchar buffer[1];
        buffer[0] = focuser_is_moving();
        usbMsgPtr = buffer;
        return sizeof(buffer);
    }
    else if (rq->bRequest == FOCUSER_GET_CAPABILITIES) {
        static uchar buffer[1];
        buffer[0] = capabilities;
        usbMsgPtr = buffer;
        return sizeof(buffer);
    }
    return 0;   /* default for not implemented requests: return no data back to host */
}


int __attribute__((noreturn)) main(void)
{
    uchar   i;

    wdt_enable(WDTO_1S);
    /* Even if you don't use the watchdog, turn it off here. On newer devices,
     * the status of the watchdog (on/off, period) is PRESERVED OVER RESET!
     */
    /* RESET status: all port bits are inputs without pull-up.
     * That's the way we need D+ and D-. Therefore we don't need any
     * additional hardware initialization.
     */
    usbInit();
    usbDeviceDisconnect();  /* enforce re-enumeration, do this while interrupts are disabled! */
    i = 0;
    while (--i) {             /* fake USB disconnect for > 250 ms */
        wdt_reset();
        _delay_ms(1);
    }
    usbDeviceConnect();
    DDRD |= _BV(PD0);
    focuser_init();
    sei();
    for (;;) {                /* main event loop */
        wdt_reset();
        usbPoll();
    }
}
