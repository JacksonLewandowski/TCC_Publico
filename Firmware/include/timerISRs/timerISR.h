#ifndef TIMERISR_H
#define TIMERISR_H
#include <Arduino.h>
hw_timer_t *TimerAnalogRead = NULL;
const unsigned char TimerDiv = 80;
const unsigned short Timer_1ms = 1000;
volatile unsigned short counterTimer = 0;
unsigned char timer10ms = 0;
void initTimers();
IRAM_ATTR void ISR_analogRead();
void armatureCurrentRead();
void armatureVoltageRead();
void fieldCurrentRead();
#endif