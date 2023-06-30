#ifndef EEPROM_CPP
#define EEPROM_CPP
#include "Eeprom.h"
void disableTimers()
{
    if(Timer0 != NULL)
        timerAlarmDisable(Timer0);
    if(Timer1 != NULL)
        timerAlarmDisable(Timer1);
}
void enableTimers()
{
    if(Timer0 != NULL)
        timerAlarmEnable(Timer0);
    if(Timer1 != NULL)
        timerAlarmEnable(Timer1);
}
unsigned char EepromCheckByte(unsigned short address, unsigned char byte)
{
    if (EEPROM.read(address) == byte)
        return 1;
    return 0;
}
unsigned char EepromWrite(unsigned short address, unsigned char byte)
{
    if (EepromCheckByte(address, byte))
        return eepromOk;
    disableTimers();
    EEPROM.write(address,byte);
    EEPROM.commit();
    enableTimers();
    if (EepromCheckByte(address, byte))
        return eepromOk;
    return eepromError;
}
unsigned char EepromWrite(unsigned short address, unsigned short word16b)
{
    for (unsigned char i = 0; i < sizeof(unsigned short); i++)
        if(EepromWrite(address + i, (unsigned char) (word16b >> (8*i))) == eepromError)
            return eepromError;
    return eepromOk;
}
unsigned char EepromWrite(unsigned short address, unsigned int word32b)
{
     for (unsigned char i = 0; i < sizeof(unsigned int); i++)
        if(EepromWrite(address + i, (unsigned char) (word32b >> (8*i))) == eepromError)
            return eepromError;
    return eepromOk;
}
unsigned char EepromWrite(unsigned short address, unsigned long long word64b)
{
     for (unsigned char i = 0; i < sizeof(unsigned long long); i++)
        if(EepromWrite(address + i, (unsigned char) (word64b >> (8*i))) == eepromError)
            return eepromError;
    return eepromOk;
}
unsigned char EepromWrite(unsigned short address, unsigned char *ptr, unsigned short dim)
{
    for (unsigned char i = 0; i < dim; i++)
        if(EepromWrite(address + i, (unsigned char)ptr[i]) == eepromError)
            return eepromError;
    return eepromOk;
}
unsigned char EepromWrite(unsigned short address, unsigned short *ptr, unsigned short dim)
{
    return 0;
}
unsigned char EepromWrite(unsigned short address, unsigned int *ptr, unsigned short dim)
{
    return 0;
}
unsigned char EepromRead(unsigned short address)
{
    unsigned char byte;
    byte = EEPROM.read(address);
    return byte;
}
unsigned char EepromRead(unsigned short address, unsigned char *ptr, unsigned short dim)
{
    for (unsigned short i = 0; i < dim; i++)
        ptr[i] = EepromRead(address+i);
    return eepromOk;
}
unsigned char EepromRead(unsigned short address, unsigned short *ptr, unsigned short dim)
{
    return 0;
}
unsigned char EepromRead(unsigned short address, unsigned int *ptr, unsigned short dim)
{
    return 0;
}
unsigned short EepromReadShort(unsigned short address)
{
    unsigned short value = 0;
    value = EepromRead(address);
    value += (unsigned short)EepromRead(address+1) << 8;
    return value;
}
unsigned char EepromReset()
{
    for (unsigned short i = 0; i < eepromSize; i++)
        EepromWrite(i,constDefaultEeprom);
    return eepromOk;
}
#endif
