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


static void usage(char *name)
{
    fprintf(stderr, "usage:\n");
    /*fprintf(stderr, "  %s move ....... turn on LED\n", name);*/
    /*fprintf(stderr, "  %s off ...... turn off LED\n", name);*/
    /*fprintf(stderr, "  %s status ... ask current status of LED\n", name);*/
}

int main(int argc, char **argv)
{
    focuser_connect();
    if(argc < 2){   /* we need at least one argument */
        usage(argv[0]);
        exit(1);
    }
    if(strcasecmp(argv[1], "move") == 0){
        uint16_t position = (uint16_t)atoi(argv[2]);
        focuser_move_to(position);
        printf("Moving to: %d\n", position);
    }else if (strcasecmp(argv[1], "halt") == 0){
        focuser_halt();
    }else if (strcasecmp(argv[1], "position") == 0){
        printf("Position: %d", focuser_get_position());
    }else if (strcasecmp(argv[1], "status") == 0){
        uint8_t moving = focuser_is_moving();
        if(moving == 0)
            printf("Moving\n");
        else
            printf("Stopped\n");
    }else if (strcasecmp(argv[1], "capabilities") == 0){
        printf("Capabilities: %d\n", focuser_get_capabilities());
    }else{
        usage(argv[0]);
        exit(1);
    }
    focuser_disconnect();
    return 0;
}
