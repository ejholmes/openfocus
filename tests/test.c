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

#include "test.h"
#include "openfocus.h"

int main(int argc, const char *argv[])
{
    test_focuser_connect();
    test_focuser_disconnect();
    test_focuser_move_to();
    test_focuser_halt();

    return 0;
}

void test_focuser_connect()
{
    printf("Testing focuser_connect()\n");
    int i = focuser_connect();
    assert(i == 0);
    printf("Test Passed!\n\n");
}

void test_focuser_disconnect()
{
    printf("Testing focuser_disconnect()\n");
    int i = focuser_disconnect();
    assert(i == 0);
    printf("Test Passed!\n\n");
}

void test_focuser_move_to()
{
    focuser_connect();

    printf("Testing focuser_move_to()\n");
    focuser_move_to(100);

    printf("Testing focuser_is_moving()\n");
    assert(focuser_is_moving() == true);
    printf("Test Passed!\n\n");

    printf("Waiting for focuser to stop...\n");
    while(focuser_is_moving());
    printf("Focuser stopped\n");

    printf("Testing focuser_get_position()\n");
    assert(focuser_get_position() == 100);
    printf("Test Passed!\n\n");

    focuser_disconnect();
}

void test_focuser_halt()
{

}
