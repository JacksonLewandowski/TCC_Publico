#ifndef REGISTRADORES_H
#define REGISTRADORES_H
#include <Arduino.h>
#include "RegAddress.h"
#include "Eeprom/Eeprom.cpp"
void initRegisters();
void initHoldingReg();
void initHoldingInputReg();
void initInputReg();
void initOutputReg();
class Register
{
private:
    volatile unsigned short _value = 0;
    unsigned short _eepromAddress = 0;
public:
    Register(unsigned short initValue, unsigned short eepromAdress);
    ~Register();
    volatile unsigned short read();
    volatile unsigned short write(unsigned short value);
};
Register *VetRegisters[RegistersQuantity] = {NULL};



#endif