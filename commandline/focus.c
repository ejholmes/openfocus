/* Name: focus.c
 */

/*
General Description:
This is the host-side driver for the custom-class example device. It searches
the USB for the LEDControl device and sends the requests understood by this
device.
This program must be linked with libusb on Unix and libusb-win32 on Windows.
See http://libusb.sourceforge.net/ or http://libusb-win32.sourceforge.net/
respectively.
*/

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <inttypes.h>
#include <usb.h>        /* this is libusb */
#include "opendevice.h" /* common code moved to separate module */

#include "requests.h"   /* custom request numbers */
#include "usbconfig.h"  /* device's VID/PID and names */

static void usage(char *name)
{
    fprintf(stderr, "usage:\n");
    /*fprintf(stderr, "  %s move ....... turn on LED\n", name);*/
    /*fprintf(stderr, "  %s off ...... turn off LED\n", name);*/
    /*fprintf(stderr, "  %s status ... ask current status of LED\n", name);*/
}

int main(int argc, char **argv)
{
    usb_dev_handle      *handle = NULL;
    const uint8_t rawVid[2] = {USB_CFG_VENDOR_ID}, rawPid[2] = {USB_CFG_DEVICE_ID};
    char                vendor[] = {USB_CFG_VENDOR_NAME, 0}, product[] = {USB_CFG_DEVICE_NAME, 0};
    char                buffer[4];
    int                 cnt, vid, pid;

    usb_init();
    if(argc < 2){   /* we need at least one argument */
        usage(argv[0]);
        exit(1);
    }
    /* compute VID/PID from usbconfig.h so that there is a central source of information */
    vid = rawVid[1] * 256 + rawVid[0];
    pid = rawPid[1] * 256 + rawPid[0];
    /* The following function is in opendevice.c: */
    if(usbOpenDevice(&handle, vid, vendor, pid, product, NULL, NULL, NULL) != 0){
        fprintf(stderr, "Could not find USB device \"%s\" with vid=0x%x pid=0x%x\n", product, vid, pid);
        exit(1);
    }
    /* Since we use only control endpoint 0, we don't need to choose a
     * configuration and interface. Reading device descriptor and setting a
     * configuration and interface is done through endpoint 0 after all.
     * However, newer versions of Linux require that we claim an interface
     * even for endpoint 0. Enable the following code if your operating system
     * needs it: */
#if 0
    int retries = 1, usbConfiguration = 1, usbInterface = 0;
    if(usb_set_configuration(handle, usbConfiguration) && showWarnings){
        fprintf(stderr, "Warning: could not set configuration: %s\n", usb_strerror());
    }
    /* now try to claim the interface and detach the kernel HID driver on
     * Linux and other operating systems which support the call. */
    while((len = usb_claim_interface(handle, usbInterface)) != 0 && retries-- > 0){
#ifdef LIBUSB_HAS_DETACH_KERNEL_DRIVER_NP
        if(usb_detach_kernel_driver_np(handle, 0) < 0 && showWarnings){
            fprintf(stderr, "Warning: could not detach kernel driver: %s\n", usb_strerror());
        }
#endif
    }
#endif

    if(strcasecmp(argv[1], "move") == 0){
        uint16_t position = (uint16_t)atoi(argv[2]);
        usb_control_msg(handle, USB_TYPE_VENDOR | USB_RECIP_DEVICE | USB_ENDPOINT_OUT, FOCUSER_MOVE_TO, position, 0, buffer, 0, 5000);
        printf("Moving to: %d\n", position);
    }else if (strcasecmp(argv[1], "halt") == 0){
        usb_control_msg(handle, USB_TYPE_VENDOR | USB_RECIP_DEVICE | USB_ENDPOINT_OUT, FOCUSER_HALT, 0, 0, buffer, 0, 5000);
    }else if (strcasecmp(argv[1], "position") == 0){
        cnt = usb_control_msg(handle, USB_TYPE_VENDOR | USB_RECIP_DEVICE | USB_ENDPOINT_IN, FOCUSER_GET_POSITION, 0, 0, buffer, sizeof(buffer), 5000);
        if(cnt < 1){
            if(cnt < 0){
                fprintf(stderr, "USB error: %s\n", usb_strerror());
            }else{
                fprintf(stderr, "only %d bytes received.\n", cnt);
            }
        }
        else {
            uint8_t l = buffer[0], h = buffer[1];
            uint16_t position = (h << 8) | l;
            printf("Position: %d\n", position);
        }
    }else if (strcasecmp(argv[1], "status") == 0){
        cnt = usb_control_msg(handle, USB_TYPE_VENDOR | USB_RECIP_DEVICE | USB_ENDPOINT_IN, FOCUSER_GET_STATUS, 0, 0, buffer, sizeof(buffer), 5000);
        if(cnt < 1){
            if(cnt < 0){
                fprintf(stderr, "USB error: %s\n", usb_strerror());
            }else{
                fprintf(stderr, "only %d bytes received.\n", cnt);
            }
        }
        else {
            uint8_t moving = (uint8_t)buffer[0];
            if(moving == 0){
                printf("Moving...\n");
            }
            else {
                printf("Stopped\n");
            }
        }
    }else{
        usage(argv[0]);
        exit(1);
    }
    usb_close(handle);
    return 0;
}
