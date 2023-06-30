#ifndef EEPROM_H
#define EEPROM_H 
#include <EEPROM.h> 
const unsigned short eepromSize = 256;
const unsigned char constDefaultEeprom = 255;
const unsigned char eepromOk = 1;
const unsigned char eepromError = 0;
hw_timer_t *Timer0 = NULL;
hw_timer_t *Timer1 = NULL;
void disableTimers();
void enableTimers();
unsigned char EepromCheckByte(unsigned short address, unsigned char byte);
unsigned char EepromWrite(unsigned short address, unsigned char byte);
unsigned char EepromWrite(unsigned short address, unsigned short word16b);
unsigned char EepromWrite(unsigned short address, unsigned int word32b);
unsigned char EepromWrite(unsigned short address, unsigned long long word64b);
unsigned char EepromWrite(unsigned short address, unsigned char *ptr, unsigned short dim);
unsigned char EepromWrite(unsigned short address, unsigned short *ptr, unsigned short dim);
unsigned char EepromWrite(unsigned short address, unsigned int *ptr, unsigned short dim);
unsigned char EepromRead(unsigned short address);
unsigned char EepromRead(unsigned short address, unsigned char *ptr, unsigned short dim);
unsigned char EepromRead(unsigned short address, unsigned short *ptr, unsigned short dim);
unsigned char EepromRead(unsigned short address, unsigned int *ptr, unsigned short dim);
unsigned short EepromReadShort(unsigned short address);
unsigned char EepromReset();

#endif