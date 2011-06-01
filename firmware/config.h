#ifndef __CONFIG_H_
#define __CONFIG_H_ 

#include "requests.h"

/* Number of times the sensor is sampled each request */
#ifndef TEMP_SENSOR_COUNT
    #define TEMP_SENSOR_COUNT 3
#endif

/* ADC pin that sensor is on. Can be 0-7. (ex. ADC0 = 0, ADC1 = 1) */
#ifndef TEMP_SENSOR_PIN
    #define TEMP_SENSOR_PIN 0
#endif

/* Absolute positioning */
#ifndef ABSOLUTE_POSITIONING_ENABLED
	#define ABSOLUTE_POSITIONING_ENABLED 1
#endif

/* Temperature Compensation */
#ifndef TEMPERATURE_COMPENSATION_ENABLED
	#define TEMPERATURE_COMPENSATION_ENABLED 1
#endif

#define CAPABILITY(enable, cap) (enable & cap)

#endif /* __CONFIG_H_ */
