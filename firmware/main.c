#include <avr/io.h>
#include <avr/wdt.h>
#include <avr/interrupt.h>
#include <avr/pgmspace.h>
#include <util/delay.h>
#include <avr/eeprom.h>

#include <string.h>

#include "requests.h"
#include "usbdrv.h"
#include "focuser.h"
#include "temperature.h"
#include "util.h"
#include "eeprom.h"

#include "config.h"

static uint8_t capabilities = CAPABILITY(ABSOLUTE_POSITIONING_ENABLED, CAP_ABSOLUTE) | CAPABILITY(TEMPERATURE_COMPENSATION_ENABLED, CAP_TEMP_COMP);

#ifdef EEPROM_SERIAL_NUMBER
usbMsgLen_t usbFunctionDescriptor(struct usbRequest *rq)
{
    int length = eeprom_read_byte((const uint8_t*)EEPROM_ADDRESS_SERIAL_NUMBER_LEN);
    eeprom_busy_wait();
    static uint16_t serialNumberDescriptor[MAX_SERIAL_LEN];
    serialNumberDescriptor[0] = USB_STRING_DESCRIPTOR_HEADER(length);
    eeprom_read_block(&serialNumberDescriptor[1], (const uint8_t*)EEPROM_ADDRESS_SERIAL_NUMBER, length * 2);

    switch (rq->wValue.bytes[1])
    {
        case USBDESCR_STRING:
            usbMsgPtr = (uchar *)serialNumberDescriptor;
            return ((length + 1) * sizeof(uint16_t));
    }
    return 0;
}
#endif

usbMsgLen_t usbFunctionSetup(uchar data[8])
{
    usbRequest_t *rq = (void *)data;

    if (rq->bRequest == FOCUSER_MOVE_TO) {
        focuser_move_to((uint16_t)rq->wValue.word);
        return 0;
    }
    else if (rq->bRequest == FOCUSER_HALT) {
        focuser_halt();
        return 0;
    }
    else if (rq->bRequest == FOCUSER_GET_POSITION) {
        static uint16_t position;
        position = focuser_get_position();
        usbMsgPtr = (uchar *)&position;
        return sizeof(position);
    }
    else if (rq->bRequest == FOCUSER_SET_POSITION) {
        focuser_set_position((uint16_t)rq->wValue.word);
        return 0;
    }
    else if (rq->bRequest == REBOOT_TO_BOOTLOADER) {
        reboot_to_bootloader();
        return 0;
    }
    else if (rq->bRequest == FOCUSER_REVERSE) {
        focuser_reverse(rq->wValue.bytes[0]);
        return 0;
    }
    else if (rq->bRequest == FOCUSER_IS_MOVING) {
        static uchar buffer;
        buffer = focuser_is_moving();
        usbMsgPtr = &buffer;
        return sizeof(buffer);
    }
    else if (rq->bRequest == FOCUSER_GET_CAPABILITIES) {
        usbMsgPtr = &capabilities;
        return sizeof(capabilities);
    }
    else if (rq->bRequest == FOCUSER_GET_TEMPERATURE) {
        static uint16_t temperature;
        temperature = temperature_read(TEMP_SENSOR_COUNT);
        usbMsgPtr = (uchar *)&temperature;
        return sizeof(temperature);
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
    temperature_init(TEMP_SENSOR_PIN);
    sei();
    for (;;) {                /* main event loop */
        wdt_reset();
        usbPoll();
    }
}
