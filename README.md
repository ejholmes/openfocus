OpenFocus
=========
OpenFocus is an open source implementation of a computer controlled focuser for
telescopes based around the ATMega328p AVR chip from Atmel. It uses
the [V-USB stack][vusb] and [libusb][libusb] and also includes an
[ASCOM][ascom] driver for use on Windows.

Features include:

* Repeatable positions by using a high resolution stepper motor and absolute positioning.
* Temperature Sensing and Compensation for combating thermal expansion and contraction while imaging.
* Firmware Upgradable.

Installation
------------
Download the latest setup executable from the [[downloads|https://github.com/CortexAstronomy/OpenFocus/downloads]] page.


Building from source
-------------------
See the [[wiki article|https://github.com/CortexAstronomy/OpenFocus/wiki/Building]] for instructions on building from source.