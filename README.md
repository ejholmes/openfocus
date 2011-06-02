OpenFocus
=========
OpenFocus is an open source implementation of a computer controlled focuser for
telescopes based around the ATMega328p AVR chip from Atmel. It uses
the [V-USB stack][vusb] and [libusb][libusb] and also includes an
[ASCOM][ascom] driver for use on Windows.

Features include:

* Repeatable positions by using a high resolution stepper motor and absolute positioning.
* Temperature Sensing and Compensation for combating thermal expansion and contraction while imaging.
* Firmware Upgradeable.
* Quickly and easily import FocusMax temperature compensation data logs.

Installation
------------

Prerequisites: [ASCOM Platform 5.5][ascom]

To download the most recent driver installer, visit the [downloads][downloads] page.

To update your device to the most recent firmware version, visit the [wiki article][updatefirmware].


Building from source
-------------------
See the [wiki article][buildfromsource] for instructions on building from source.

[vusb]:http://www.obdev.at/products/vusb/index.html
[libusb]:http://www.libusb.org/
[winavr]:http://winavr.sourceforge.net/
[crosspack]:http://www.obdev.at/products/crosspack/index.html
[msys]:http://www.mingw.org/wiki/MSYS
[ascom]:http://ascom-standards.org/
[c#]:http://www.microsoft.com/express/Downloads/#2008-Visual-CS
[downloads]:https://github.com/CortexAstronomy/OpenFocus/downloads
[buildfromsource]:https://github.com/CortexAstronomy/OpenFocus/wiki/Building
[updatefirmware]:https://github.com/CortexAstronomy/OpenFocus/wiki/HowTo%3A-Update-Firmware