#ifndef BLUETOOTH_MODBUS_H
#define BLUETOOTH_MODBUS_H
#include <driver\uart.h>
#include <FunctionsModbus/functions.cpp>
#include <BitManipulation/bitManipulation.cpp>
#include <BluetoothSerial.h>


BluetoothSerial SerialBT;

void initialize_bluetooth();

static void bluetooth_task(void *pvParameters);

unsigned short returnCRC16(unsigned char *data, unsigned short dimension);

void processMessage(unsigned char* data, unsigned char dimension);

unsigned char ChecksCRC(unsigned char* data, unsigned char dimension);

void bluetooth_write(unsigned char* data, unsigned short dimension);

#endif