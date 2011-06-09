/*
 * File: main.c
 * Package: OpenFocus Bootloader
 *
 * Based on BootloadHID by Christian Starkjohann (http://www.obdev.at/products/vusb/bootloadhid.html)
 *
 * See LICENSE.txt for licensing information.
 *
 */

#include <avr/io.h>
#include <avr/interrupt.h>
#include <avr/pgmspace.h>
#include <avr/wdt.h>
#include <avr/boot.h>
#include <string.h>
#include <util/delay.h>

#include "config.h"
#include "usbdrv.h"
#include "../eeprom.h"

static void leaveBootloader() __attribute__((__noreturn__));

static inline void  bootLoaderInit(void)
{
    PORTD |= _BV(PD7); /* Activiate internal pullup on PD7 */
    _delay_us(10);  /* wait for levels to stabilize */
}

#define bootLoaderCondition() (((PIND & _BV(PD7)) == 0) || eeprom_read_stay_in_bootloader() == 1)

#define USB_RQ_REBOOT             0x01
#define USB_RQ_WRITE_FLASH_BLOCK  0x02
#define USB_RQ_GET_REPORT         0x03
#define USB_RQ_WRITE_EEPROM_BLOCK 0x04

#define WRITE_FLASH_BLOCK  0x01
#define WRITE_EEPROM_BLOCK 0x02

static uint8_t cmd;
static uchar bytesRemaining;
static uint8_t startPage = 0;
static uint16_t address;
static uint16_t pageAddress;

static uchar exitMainloop;

/* compatibility with ATMega88 and other new devices: */
#ifndef GICR
#define GICR    MCUCR
#endif

static void (*nullVector)(void) __attribute__((__noreturn__));

static void leaveBootloader()
{
    statusLedOff();
    cli();
    boot_rww_enable();
    USB_INTR_ENABLE = 0;
    USB_INTR_CFG = 0;       /* also reset config bits */

    GICR = (1 << IVCE);     /* enable change of interrupt vectors */
    GICR = (0 << IVSEL);    /* move interrupts to application flash section */
/* We must go through a global function pointer variable instead of writing
 *  ((void (*)(void))0)();
 * because the compiler optimizes a constant 0 to "rcall 0" which is not
 * handled correctly by the assembler.
 */
    nullVector();
}

usbMsgLen_t   usbFunctionSetup(uchar data[8])
{
	usbRequest_t    *rq = (void *)data;
	static uchar    replyBuffer[6] = {
        SPM_PAGESIZE & 0xff,
        SPM_PAGESIZE >> 8,
        ((long)FLASHEND + 1) & 0xff,
        (((long)FLASHEND + 1) >> 8) & 0xff,
        (((long)FLASHEND + 1) >> 16) & 0xff,
        (((long)FLASHEND + 1) >> 24) & 0xff
    };

    if (rq->bRequest == USB_RQ_WRITE_FLASH_BLOCK) {
        startPage = 1;
        cmd = WRITE_FLASH_BLOCK;
        bytesRemaining = rq->wLength.word;
		return USB_NO_MSG;
	}
	else if (rq->bRequest == USB_RQ_WRITE_EEPROM_BLOCK) {
        startPage = 1;
        cmd = WRITE_EEPROM_BLOCK;
        bytesRemaining = rq->wLength.word;
		return USB_NO_MSG;
	}
	else if (rq->bRequest == USB_RQ_GET_REPORT) {
        usbMsgPtr = replyBuffer;
        return 6;
    }
#if BOOTLOADER_CAN_EXIT
	else if (rq->bRequest == USB_RQ_REBOOT) {
		exitMainloop = 1;
		return 0;
	}
#endif
    return 0;
}


uchar usbFunctionWrite(uchar *data, uchar len)
{
    if (bytesRemaining == 0)
        return 1;
    if (len > bytesRemaining)
        len = bytesRemaining;

    if (cmd == WRITE_EEPROM_BLOCK) {

    }
    else if (cmd == WRITE_FLASH_BLOCK) {
        /* startPage is set when we receive a WRITE_FLASH_BLOCK command over
         * usb.
         *
         * We get the address from the first 2 bytes and adjust the data buffer
         * accordingly
         * */
        if (startPage) {
            address = (uint16_t)((data[1] << 8) | (data[0] & 0xff)); /* 2 byte address */
            pageAddress = address;

            /* Remove address from data */
            len -= 2;
            bytesRemaining -= 2;
            data += 2;
        }

        do {
            /* if we're at the start of a page, erase the page */
            if (startPage){
                cli();
                boot_page_erase(pageAddress);
                sei();
                boot_spm_busy_wait();
                startPage = 0;
            }

            /* Fill the page buffer */
            cli();
            boot_page_fill(address, *(short *)data);
            sei();

            /* We wrote 2 bytes */
            address += 2;
            data += 2;
            len -= 2;
            bytesRemaining -= 2;

            /* bytesRemaining = 0, so we must be at the end of a page */
            if (bytesRemaining == 0) {
                cli();
                boot_page_write(pageAddress);
                sei();
                boot_spm_busy_wait();
                return 1;
            }

        } while(len);
    }
    return 0;
}

static void initForUsbConnectivity(void)
{
	uchar   i = 0;
	
    usbInit();
	usbDeviceDisconnect();  /* enforce re-enumeration, do this while interrupts are disabled! */
	i = 0;
	while (--i) {             /* fake USB disconnect for > 250 ms */
		wdt_reset();
		_delay_ms(1);
	}
	usbDeviceConnect();
	DDRD |= _BV(PD0);
	sei();
}

int __attribute__((noreturn)) main(void)
{
	bootLoaderInit();
    initStatusLed();
	
	if (bootLoaderCondition()) {
		if (eeprom_read_stay_in_bootloader() != 0)
			eeprom_clear_stay_in_bootloader();
			
        statusLedOn();
		uchar i = 0, j = 0;
		
		GICR = (1 << IVCE); /* enable change of interrupt vectors */
		GICR = (1 << IVSEL); /* move interrupts to boot flash section */
		
		initForUsbConnectivity();
		for (;;) { /* main event loop */
            wdt_reset();
            usbPoll();
#if BOOTLOADER_CAN_EXIT
            if (exitMainloop) {
                if (--i == 0) {
                    if (--j == 0)
                        break;
                }
            }
#endif
        }
	}
	leaveBootloader();
}

