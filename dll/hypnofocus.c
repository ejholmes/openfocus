#include <stdlib.h>
#include <string.h>
#include <stdio.h>
#include <inttypes.h>
#include <usb.h>
#include "opendevice.h"

#include "usbconfig.h"
#include "requests.h"

usb_dev_handle *handle = NULL;
const uint8_t VID[2] = {USB_CFG_VENDOR_ID}, PID[2] = {USB_CFG_DEVICE_ID};
char vendor[] = {USB_CFG_VENDOR_NAME, 0}, product[] = {USB_CFG_DEVICE_NAME, 0};
int vid, pid;

__declspec(dllexport) int focuser_connect(void){
    usb_init();
    vid = VID[1] * 256 + VID[0];
    pid = PID[1] * 256 + PID[0];

    if(usbOpenDevice(&handle, vid, vendor, pid, product, NULL, NULL, NULL) != 0){
        return -1;
    }
#if 0
    int retries = 1, usbConfiguration = 1, usbInterface = 0;
    if(usb_set_configuration(handle, usbConfiguration) && showWarnings){
        return -1;
    }
    /*  now try to claim the interface and detach the kernel HID
     *  driver on
     *       * Linux and other operating systems which support the
     *       call. */
    while((len = usb_claim_interface(handle, usbInterface)) != 0 && retries-- > 0){
#ifdef LIBUSB_HAS_DETACH_KERNEL_DRIVER_NP
        if(usb_detach_kernel_driver_np(handle, 0) < 0 && showWarnings){
            return -1;
        }
#endif
    }
#endif
    return 0;
}

__declspec(dllexport) int focuser_disconnect(void){
    usb_close(handle);
    return 0;
}

__declspec(dllexport) void focuser_move_to(uint16_t position){
    uint8_t buffer[2];
    usb_control_msg(handle, USB_TYPE_VENDOR | USB_RECIP_DEVICE | USB_ENDPOINT_OUT, FOCUSER_MOVE_TO, position, 2, buffer, 0, 5000);
}

__declspec(dllexport) int focuser_is_moving(void){
    int cnt;
    uint8_t buffer[1];
    cnt = usb_control_msg(handle, USB_TYPE_VENDOR | USB_RECIP_DEVICE | USB_ENDPOINT_IN, FOCUSER_GET_STATUS, 0, 0, buffer, sizeof(buffer), 5000);
    if(cnt > 0){
        return buffer[0];
    }
    else {
        return -1;
    }
}

__declspec(dllexport) int focuser_get_position(void){
    int cnt;
    uint8_t buffer[2];
    cnt = usb_control_msg(handle, USB_TYPE_VENDOR | USB_RECIP_DEVICE | USB_ENDPOINT_IN, FOCUSER_GET_POSITION, 0, 0, buffer, sizeof(buffer), 5000);
    if(cnt > 0){
        return (buffer[1] << 8) | buffer[0];
    }
    else {
        return -1;
    }
}

__declspec(dllexport) void focuser_halt(void){
    uint8_t buffer[2];
    usb_control_msg(handle, USB_TYPE_VENDOR | USB_RECIP_DEVICE | USB_ENDPOINT_OUT, FOCUSER_HALT, 0, 0, buffer, 0, 5000);
}
