#ifndef REGADDRESS_H
#define REGADDRESS_H
//input address
const unsigned char RegAddressArmatureVoltage = 1;
const unsigned char RegAddressArmatureCurrent = 2;
const unsigned char RegAddressRPMread = 3;
//holding address
const unsigned char RegAddressID = 0;
const unsigned char RegAddressKp = 1;
const unsigned char RegAddressKi = 2;
const unsigned char RegAddressKd = 3;
const unsigned char RegAddressPWMarmature = 4;//this
const unsigned char RegAddressPWMinit = 5;
const unsigned char RegAddressPWMmin = 6;
const unsigned char RegAddressPWMmax = 7;
const unsigned char RegAddressPWMfield = 8;//this
const unsigned char RegAddressRPM = 9;
const unsigned char RegAddressState = 10;
const unsigned char RegAddressArmatureMaxCurrent = 11;
//Default values
const unsigned short DefaultID = 1;
const unsigned short DefaultKp = 0;
const unsigned short DefaultKi = 0;
const unsigned short DefaultKd = 0;
const unsigned short DefaultPWMinit = 0;
const unsigned short DefaultPWMmin = 0;
const unsigned short DefaultPWMmax = 1000;
const unsigned short DefaultPWMfield = 1000;
const unsigned short DefaultRPM = 0;
const unsigned short DefaultState = 1;
const unsigned short DefaultPWMFrequency = 20000;
const unsigned char DefaultIdleCurrent = 50;
const unsigned char DefaultStartCurrent = 70;
//States
const unsigned char StateIdle = 1;
const unsigned char StateRunning = 2;
const unsigned char StateFailure = 3;
//Registers Quantity
const unsigned short RegistersQuantity = 20;
const unsigned short HoldingRegOffset = 0;
const unsigned short HoldingInputOffset = 15;
const unsigned short InputRegOffset = 20;
const unsigned short OutputRegOffset = 25;
//Reg eeprom Adress
const unsigned short eepromDontUse = 0;
const unsigned short eepromAddressID = 1;
const unsigned short eepromAddressKp = 3;
const unsigned short eepromAddressKi = 5;
const unsigned short eepromAddressKd = 7;
const unsigned short eepromAddressPWMinit = 9;
const unsigned short eepromAddressPWMmin = 11;
const unsigned short eepromAddressPWMmax = 13;
//Pinos de entrada e saida
const unsigned char pinInputVoltage = 33;
//const unsigned char pinSwitchVoltage = 32;
const unsigned char pinArmatureCurrent = 35;
const unsigned char pinRefArmatureCurrent = 25;
//const unsigned char pinRefFieldCurrent = 26;
//const unsigned char pinFieldOvercurrent = 27;
const unsigned char pinArmatureOvercurrent = 14;
const unsigned char pinReadRPM = 13;
const unsigned char pinFieldPWM = 23;
const unsigned char pinArmaturePWM = 22;
/*const unsigned char pinArmatureCurrent = 33;
const unsigned char pinRefArmatureCurrent = 25;
const unsigned char pinSwitchVoltage = 27;
const unsigned char pinInputVoltage = 14;
const unsigned char pinReadRPM = 12;
const unsigned char pinFieldPWM = 18;
const unsigned char pinArmaturePWM = 5;*/
//others
volatile int RPMcounter = 0;
volatile unsigned int ZeroCurrent = 0;
volatile unsigned char ISR_PIDFlag = 0;
unsigned char dimMediaMovel = 10;
//******************************************************
unsigned char lastStateRpm = 0;
#endif
