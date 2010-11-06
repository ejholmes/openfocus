#include "hypnofocus.h"

usb_dev_handle *handle = NULL;
const uint8_t VID[2] = {USB_CFG_VENDOR_ID}, PID[2] = {USB_CFG_DEVICE_ID};
char vendor[] = {USB_CFG_VENDOR_NAME, 0}, product[] = {USB_CFG_DEVICE_NAME, 0};
int vid, pid;
static uint8_t error = NO_ERROR;

DLLEXPORT uint8_t focuser_connect(void){
    usb_init();
    vid = VID[1] * 256 + VID[0];
    pid = PID[1] * 256 + PID[0];

    if(usbOpenDevice(&handle, vid, vendor, pid, product, NULL, NULL, NULL) != 0){
        return DEVICE_NOT_FOUND_ERROR;
    }
    return NO_ERROR;
}

DLLEXPORT uint8_t focuser_disconnect(void){
    usb_close(handle);
    return NO_ERROR;
}

DLLEXPORT uint8_t focuser_get_error(void){
    return error;
}

DLLEXPORT void focuser_move_to(uint16_t position){
    uint8_t buffer[2];
    usb_control_msg(handle, USB_TYPE_VENDOR | USB_RECIP_DEVICE | USB_ENDPOINT_OUT, FOCUSER_MOVE_TO, position, 2, buffer, 0, 5000);
    error = NO_ERROR;
}

DLLEXPORT uint8_t focuser_is_moving(void){
    int cnt;
    uint8_t buffer[1];
    cnt = usb_control_msg(handle, USB_TYPE_VENDOR | USB_RECIP_DEVICE | USB_ENDPOINT_IN, FOCUSER_IS_MOVING, 0, 0, buffer, sizeof(buffer), 5000);
    if(cnt > 0)
    {
        error = NO_ERROR;
        return buffer[0];
    }
    else
    {
        error = UNDEFINED_ERROR;
        return 1;
    }
}

DLLEXPORT uint16_t focuser_get_position(void){
    int cnt;
    uint8_t buffer[2];
    cnt = usb_control_msg(handle, USB_TYPE_VENDOR | USB_RECIP_DEVICE | USB_ENDPOINT_IN, FOCUSER_GET_POSITION, 0, 0, buffer, sizeof(buffer), 5000);
    if(cnt > 0)
    {
        error = NO_ERROR;
        return (buffer[1] << 8) | buffer[0];
    }
    else
    {
        error = UNDEFINED_ERROR;
        return 0;
    }
}

DLLEXPORT uint8_t focuser_get_capabilities(void){
    int cnt;
    uint8_t buffer[1];
    cnt = usb_control_msg(handle, USB_TYPE_VENDOR | USB_RECIP_DEVICE | USB_ENDPOINT_IN, FOCUSER_GET_CAPABILITIES, 0, 0, buffer, sizeof(buffer), 5000);
    if(cnt > 0)
    {
        error = NO_ERROR;
        return buffer[0];
    }
    else
    {
        error = UNDEFINED_ERROR;
        return 1;
    }
}

DLLEXPORT void focuser_halt(void){
    uint8_t buffer[2];
    usb_control_msg(handle, USB_TYPE_VENDOR | USB_RECIP_DEVICE | USB_ENDPOINT_OUT, FOCUSER_HALT, 0, 0, buffer, 0, 5000);
    error = NO_ERROR;
}


int usbOpenDevice(usb_dev_handle **device, int vendorID, char *vendorNamePattern, int productID, char *productNamePattern, char *serialNamePattern, FILE *printMatchingDevicesFp, FILE *warningsFp){
    return NO_ERROR;
}
