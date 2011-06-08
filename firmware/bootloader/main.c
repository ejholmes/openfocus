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

static void leaveBootloader() __attribute__((__noreturn__));

#include "usbdrv.h"

#define USB_RQ_REBOOT             0x01
#define USB_RQ_WRITE_FLASH_BLOCK  0x02
#define USB_RQ_GET_REPORT         0x03
#define USB_RQ_WRITE_EEPROM_BLOCK 0x04

#define WRITE_EEPROM_BLOCK 0x01
#define WRITE_FLASH_BLOCK 0x02

/* ------------------------------------------------------------------------ */

#ifndef ulong
#   define ulong    unsigned long
#endif
#ifndef uint
#   define uint     unsigned int
#endif

#define addr_t           uint

static addr_t           currentAddress; /* in bytes */
static uchar            offset;         /* data already processed in current transfer */
#if BOOTLOADER_CAN_EXIT
static uchar            exitMainloop;
#endif
static uchar            cmd;

/* compatibility with ATMega88 and other new devices: */
#ifndef GICR
#define GICR    MCUCR
#endif

static void (*nullVector)(void) __attribute__((__noreturn__));

static void leaveBootloader()
{
	PORTB &= ~_BV(PB0);
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
		offset = 0;
        cmd = WRITE_FLASH_BLOCK;
		return USB_NO_MSG;
	}
	else if (rq->bRequest == USB_RQ_WRITE_EEPROM_BLOCK) {
        cmd = WRITE_EEPROM_BLOCK;
		return USB_NO_MSG;
	}
	else if (rq->bRequest == USB_RQ_GET_REPORT) {
        usbMsgPtr = replyBuffer;
        return 6;
    }
#if BOOTLOADER_CAN_EXIT
	else if (rq->bRequest == USB_RQ_REBOOT) {
		exitMainloop = 1;
		return USB_NO_MSG;
	}
#endif
    return 0;
}

uchar usbFunctionWrite(uchar *data, uchar len)
{
	if (cmd == WRITE_EEPROM_BLOCK) {
		union {
			addr_t l;
			uchar c[sizeof(addr_t)];
		} address;
		address.c[0] = data[0];
		address.c[1] = data[1];
		address.c[2] = data[2];
		address.c[3] = 0;
		
		data += 3;
		len -= 3;
		
		eeprom_write_block(data, (void *)address.l, len);
		eeprom_busy_wait();
		
		return 1;
	}
	else if (cmd == WRITE_FLASH_BLOCK) {
		union {
			addr_t  l;
			uint    s[sizeof(addr_t)/2];
			uchar   c[sizeof(addr_t)];
        } address;
		uchar   isLast;

		address.l = currentAddress;
		if (offset == 0){
			address.c[0] = data[0];
			address.c[1] = data[1];

			data += 2;
			len -= 2;
		}
		offset += len;
		isLast = offset & 0x80; /* != 0 if last block received */
		do {
			addr_t prevAddr;
			uchar pageAddr;

			pageAddr = address.s[0] & (SPM_PAGESIZE - 1);
			if (pageAddr == 0){              /* if page start: erase */
				cli();
				boot_page_erase(address.l); /* erase page */
				sei();
				boot_spm_busy_wait();       /* wait until page is erased */
			}

			cli();
			boot_page_fill(address.l, *(short *)data);
			sei();

			prevAddr = address.l;
			address.l += 2;
			data += 2;

			/* write page when we cross page boundary */
			pageAddr = address.s[0] & (SPM_PAGESIZE - 1);
			if (pageAddr == 0) {
				cli();
				boot_page_write(prevAddr);
				sei();
				boot_spm_busy_wait();
			}

			len -= 2;
		} while(len);

		currentAddress = address.l;
		return isLast;
	}
    return 1;
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
	DDRB |= _BV(PB0);
	
	if (bootLoaderCondition()) {
		if (eeprom_read_stay_in_bootloader() != 0)
			eeprom_clear_stay_in_bootloader();
			
		PORTB |= _BV(PB0);
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
