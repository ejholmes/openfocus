OpenFocus
=========
OpenFocus is an open source implementation of a computer controlled focuser for
telescope systems. It's based off the ATMega328p AVR chip from Atmel. It uses
the [V-USB stack][vusb] and [libusb][libusb] and also includes an
[ASCOM][ascom] driver for use of Windows.

Building From Source
-------------------

Firmware:
---------

Requirements:

- [WinAVR][winavr] (if on Windows) or [CrossPack][crosspack] (if on Mac OS X)
- [MSYS][msys] (if on Windows)

Build:

- Edit firmware/Makefile according to your ISP programmer.
- Run:

    `make`

    `make install`


Driver
------

Requirements:

- [MSYS][msys] (if on Windows)
- [libusb][libusb]

Build:

- Run:

    `make`


ASCOM Driver
-----------

Requirements:

- [Visual C# Express 2008][c#]
- [ASCOM Platform][ascom]

Build:

- Open ascom/OpenFocus.sln in Visual C# Express 2008
- Run **Build**

[vusb]:http://www.obdev.at/products/vusb/index.html
[libusb]:http://www.libusb.org/
[winavr]:http://winavr.sourceforge.net/
[crosspack]:http://www.obdev.at/products/crosspack/index.html
[msys]:http://www.mingw.org/wiki/MSYS
[ascom]:http://ascom-standards.org/
[c#]:http://www.microsoft.com/express/Downloads/#2008-Visual-CS
