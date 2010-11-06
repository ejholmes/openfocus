/* Name: main.c
 * Project: Hypnofocus
 * Author: Eric Holmes
 * Creation Date: 2010-10-01
 * Copyright: (c) 2010 Eric Holmes
 * License: GNU GPL v2
 * $Id: bce98e4a2f403467696df402210999e8102c7f18 $
 */

#include <avr/io.h>
#include <avr/wdt.h>
#include <avr/interrupt.h>
#include <avr/pgmspace.h>

#include "usbdrv.h"

#include "focuser.h"
#include "util.h"

#ifdef __HYPNOFOCUS_V1__
const uint8_t capabilities = CAP_ABSOLUTE;
#else
const uint8_t capabilities = 0x00;
#endif

usbMsgLen_t usbFunctionSetup(uchar data[8])
{
    usbRequest_t *rq = (void *)data;

    if(rq->bRequest == FOCUSER_MOVE_TO){
        uint8_t l = rq->wValue.bytes[0];
        uint8_t h = rq->wValue.bytes[1];
        focuser_move_to(make_uint(l, h));
        return 0;
    }
    else if (rq->bRequest == FOCUSER_HALT){
        focuser_halt();
        return 0;
    }
    else if (rq->bRequest == FOCUSER_GET_POSITION){
        uint16_t position = focuser_position();
        static uchar buffer[2];
        buffer[0] = lsb(position);
        buffer[1] = msb(position);
        usbMsgPtr = buffer;
        return sizeof(buffer);
    }
    else if (rq->bRequest == FOCUSER_IS_MOVING){
        bool moving = focuser_is_moving();
        static uchar buffer[1];
        buffer[0] = moving ? 0 : 1;
        usbMsgPtr = buffer;
        return sizeof(buffer);
    }
    else if (rq->bRequest == FOCUSER_GET_CAPABILITIES){
        static uchar buffer[1];
        buffer[0] = capabilities;
        usbMsgPtr = buffer;
        return sizeof(buffer);
    }
    return 0;   /* default for not implemented requests: return no data back to host */
}


int __attribute__((noreturn)) main(void){
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
    while(--i){             /* fake USB disconnect for > 250 ms */
        wdt_reset();
        _delay_ms(1);
    }
    usbDeviceConnect();
    DDRD |= _BV(PD0);
    focuser_init();
    sei();
    for(;;){                /* main event loop */
        wdt_reset();
        usbPoll();
    }
}
