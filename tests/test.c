/*
 * File: test.c
 * Package: OpenFocus Test Suite
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
#include <assert.h>
#include <time.h>

#include "test.h"
#include "openfocus.h"
#include "errors.h"

int main(int argc, const char *argv[])
{
    /* Connect */
    func("focuser_connect()");
    int i = focuser_connect("c33f7d1c-65f4-4536-9528-a2863e5b500d");
    assert(i == OF_NO_ERROR);
    passed();

    /* Disconnect */
    func("focuser_disconnect()");

    i = focuser_disconnect();
    assert(i == OF_NO_ERROR);

    passed();

    focuser_connect("c33f7d1c-65f4-4536-9528-a2863e5b500d");

    /* Set position then get position */
    func("focuser_set_position()");
    func("focuser_get_position()");

    focuser_set_position(100);
    assert(focuser_get_position() == 100);

    focuser_set_position(5262);
    assert(focuser_get_position() == 5262);

    focuser_set_position(0);
    assert(focuser_get_position() == 0);

    passed();

    /* Make sure focuser reads as stopped */
    func("focuser_is_moving()");
    assert(focuser_is_moving() == false);
    passed();

    func("focuser_move_to()");
    focuser_move_to(100);
    assert(focuser_is_moving() == true);

    while(focuser_is_moving());
    assert(focuser_get_position() == 100);
    passed();

    func("focuser_halt()");
    focuser_move_to(0);
    focuser_halt();
    assert(focuser_is_moving() == false);

    focuser_disconnect();
    return 0;
}

void func(const char *function)
{
    printf("Testing \033[1m%s\033[0m\n", function);
}

void passed()
{
    printf("\033[01;32mTest Passed!\033[00m\n\n");
}
