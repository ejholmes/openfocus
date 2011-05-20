OpenFocus
=========
OpenFocus is an open source implementation of a computer controlled focuser for
telescope systems. It's based off the ATMega328p AVR chip from Atmel. It uses
the [V-USB stack][vusb] and [libusb][libusb] and also includes an
[ASCOM][ascom] driver for use on Windows.

Features include:

* Repeatable positions by using a high resolution stepper motor and absolute positioning.
* Temperature Sensing and Compensation for combating thermal expansion and contraction while imaging.
* Firmware Upgradable.


Builing Bootloader:
---------

Requirements:  [WinAVR][winavr] (if on Windows) or [CrossPack][crosspack] (if on Mac OS X), [MSYS][msys] (if on Windows)

Build:

- Edit bootloader/firmware/Makefile according to your ISP programmer.
- Run: `cd bootloader/firmware`, `make`
- Flash with: `make install`

Builing Firmware:
---------

Requirements:  [WinAVR][winavr] (if on Windows) or [CrossPack][crosspack] (if on Mac OS X), [MSYS][msys] (if on Windows)

Build:

- Run: `cd firmware`, `make`
- Flash with bootloadHID.exe: `make softflash` **or** Flash with HIDBootFlash: `open bootloader/HIDBootFlashexe`

Building ASCOM Driver
-----------

Requirements: [Visual C# Express 2008][c#], [ASCOM Platform][ascom]

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
