## Hypnofocus DLL
DLL used to communicate with the firmware. Provides the following functions:
`focuser_connect`,
`focuser_disconnect`,
`focuser_move_to`,
`focuser_halt`,
`focuser_is_moving`,
`focuser_get_position`
 
### Requirements
* libusb driver installed (libusb0.sys)
* MinGW to compile source

### Building from Source
1. If on Windows, install cygwin with MinGW
2. Run `make`
   
### Major Contributors
* Eric Holmes - Author
