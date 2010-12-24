/*
 * File: focus.c
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


#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <inttypes.h>

#include "openfocus.h"


static void usage(char *name)
{
    fprintf(stderr, "usage:\n");
    fprintf(stderr, "  %s move <position>\n", name);
    fprintf(stderr, "  %s halt\n", name);
    fprintf(stderr, "  %s status\n", name);
    fprintf(stderr, "  %s capabilities\n", name);
}

int main(int argc, char **argv)
{
    if (focuser_connect(NULL) != 0) {
        printf("Could not OpenFocus device!\n");
        return 1;
    }
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
