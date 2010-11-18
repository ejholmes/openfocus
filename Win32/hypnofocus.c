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

    return usbOpenDevice(&handle, vid, vendor, pid, product, NULL);
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
        error = COMMUNICATION_ERROR;
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
        error = COMMUNICATION_ERROR;
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
        error = COMMUNICATION_ERROR;
        return 1;
    }
}

DLLEXPORT void focuser_halt(void){
    uint8_t buffer[2];
    usb_control_msg(handle, USB_TYPE_VENDOR | USB_RECIP_DEVICE | USB_ENDPOINT_OUT, FOCUSER_HALT, 0, 0, buffer, 0, 5000);
    error = NO_ERROR;
}


uint8_t usbOpenDevice(usb_dev_handle **device, int vendorID, char *vendorNamePattern, int productID, char *productNamePattern, char *serialNamePattern){
    struct usb_bus *bus;
    struct usb_device *dev;
    usb_dev_handle *handle;
    int error = DEVICE_NOT_FOUND_ERROR;

    usb_find_busses();
    usb_find_devices();

    for(bus = usb_get_busses(); bus; bus = bus->next){
        for(dev = bus->devices; dev; dev = dev->next){
            if((vendorID == 0 || dev->descriptor.idVendor == vendorID) && (productID ==0 || dev->descriptor.idProduct == productID)){
                char vendor[256], product[256], serial[256];
                int len;
                handle = usb_open(dev);
                if(!handle){
                    error = USBOPEN_ACCESS_ERROR;
                    usb_close(handle);
                    handle = NULL;
                    return error;
                }
                else {
                    *device = handle;
                    error = NO_ERROR;
                }
            }
        }
    }

    return error;
}

int usbGetStringAscii(usb_dev_handle *dev, int index, char *buf, int buflen)
{
    char    buffer[256];
    int     rval, i;

    if((rval = usb_get_string_simple(dev, index, buf, buflen)) >= 0) /* use libusb version if it works */
        return rval;
    if((rval = usb_control_msg(dev, USB_ENDPOINT_IN, USB_REQ_GET_DESCRIPTOR, (USB_DT_STRING << 8) + index, 0x0409, buffer, sizeof(buffer), 5000)) < 0)
        return rval;
    if(buffer[1] != USB_DT_STRING){
        *buf = 0;
        return 0;
    }
    if((unsigned char)buffer[0] < rval)
        rval = (unsigned char)buffer[0];
    rval /= 2;
    /* lossy conversion to ISO Latin1: */
    for(i=1;i<rval;i++){
        if(i > buflen)              /* destination buffer overflow */
            break;
        buf[i-1] = buffer[2 * i];
        if(buffer[2 * i + 1] != 0)  /* outside of ISO Latin1 range */
            buf[i-1] = '?';
    }
    buf[i-1] = 0;
    return i-1;
}

