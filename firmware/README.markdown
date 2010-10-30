## Hypnofocus Firmware
Firmware for the Hypnofocus.
 
### Requirements
* Git, WinAVR/Crosspack

### Building from Source and Flashing to an AVR Microcontroller
1. Connect an ATMega328p to an AVR ISP Programmer.
2. Update the Makefile for your programmer type. Default is avrisp.
3. Run `make` in the root directory.
4. Run `make install` to upload the code and burn fuses.
   
### Major Contributors
* Eric Holmes - Author
