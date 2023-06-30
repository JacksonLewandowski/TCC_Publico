#ifndef MAIN_H
#define MAIN_H
#define DELTA_T_MS 100
#include <Arduino.h>
#include <UartSlaveModbus/uartModbus.cpp>
#include <timerISRs/timerISR.cpp>
#include <Outputs/Outputs.cpp>
#include <PID/PID.cpp>
unsigned char previousState = 1;
unsigned int timeStartRunning = 0;
// RPM_MAX = 1800
// NUMBER_OF_ENCODER_TEETHS = 4
// ENCODER_MAX_FREQUENCY = RPM_MAX * NUMBER_OF_ENCODER_TEETHS / 60 SECONDS = 120Hz
// ENCODER_MINUMUM_PERIOD = 1 / ENCODER_MAX_FREQUENCY = 8.33ms
const unsigned long MinumumHighStateTime = 6000; // 5ms

const unsigned int ControllerSaturation = 50;
const unsigned int ControllerMinOutputValue = 0;
void Manual();
void Automatic();
void pid();
void stateIdle();
void stateRunning();
void stateFailure();
void changeState();
#endif