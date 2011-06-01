#ifdef __CONFIG_H_
#define __CONFIG_H_ 

/* Circuit board revision */
#ifndef BRD_REV
    #define BRD_REV 4
#endif

/* Firmware Version */
// #ifndef USB_CFG_DEVICE_VERSION
    #define USB_CFG_DEVICE_VERSION  0x01, 0x00 /* Minor, Major */
// #endif

/* Number of times the sensor is sampled each request */
#ifndef TEMP_SENSOR_COUNT
    #define TEMP_SENSOR_COUNT 3
#endif

/* ADC pin that sensor is on. Can be 0-7. (ex. ADC0 = 0, ADC1 = 1) */
#ifndef TEMP_SENSOR_PIN
    #define TEMP_SENSOR_PIN 0
#endif

#endif /* __CONFIG_H_ */
