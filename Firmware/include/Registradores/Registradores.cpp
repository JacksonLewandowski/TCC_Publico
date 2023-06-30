#ifndef REGISTRADORES_CPP
#define REGISTRADORES_CPP
#include "Registradores.h"
void initRegisters()
{
    initHoldingReg();
    initHoldingInputReg();
    initInputReg();
    initOutputReg();
}
void initHoldingReg()
{
    
    VetRegisters[HoldingRegOffset+RegAddressPWMarmature] = new Register(0,eepromDontUse);
    VetRegisters[HoldingRegOffset+RegAddressRPM] = new Register(0,eepromDontUse);
    VetRegisters[HoldingRegOffset+RegAddressPWMfield] = new Register(DefaultPWMfield,eepromDontUse);
    VetRegisters[HoldingRegOffset+RegAddressState] = new Register(DefaultState,eepromDontUse);
    if (EepromRead(0) == 255)
    {
        EEPROM.write(0,0);
        VetRegisters[HoldingRegOffset+RegAddressID] = new Register(DefaultID,eepromAddressID);
        VetRegisters[HoldingRegOffset+RegAddressKp] = new Register(DefaultKp,eepromAddressKp);
        VetRegisters[HoldingRegOffset+RegAddressKi] = new Register(DefaultKi,eepromAddressKi);
        VetRegisters[HoldingRegOffset+RegAddressKd] = new Register(DefaultKd,eepromAddressKd);
        VetRegisters[HoldingRegOffset+RegAddressPWMinit] = new Register(DefaultPWMinit,eepromAddressPWMinit);
        VetRegisters[HoldingRegOffset+RegAddressPWMmin] = new Register(DefaultPWMmin,eepromAddressPWMmin);
        VetRegisters[HoldingRegOffset+RegAddressPWMmax] = new Register(DefaultPWMmax,eepromAddressPWMmax);
    }else
        {
            VetRegisters[HoldingRegOffset+RegAddressID] = new Register(EepromReadShort(eepromAddressID),eepromAddressID);
            VetRegisters[HoldingRegOffset+RegAddressKp] = new Register(EepromReadShort(eepromAddressKp),eepromAddressKp);
            VetRegisters[HoldingRegOffset+RegAddressKi] = new Register(EepromReadShort(eepromAddressKi),eepromAddressKi);
            VetRegisters[HoldingRegOffset+RegAddressKd] = new Register(EepromReadShort(eepromAddressKd),eepromAddressKd);
            VetRegisters[HoldingRegOffset+RegAddressPWMinit] = new Register(EepromReadShort(eepromAddressPWMinit),eepromAddressPWMinit);
            VetRegisters[HoldingRegOffset+RegAddressPWMmin] = new Register(EepromReadShort(eepromAddressPWMmin),eepromAddressPWMmin);
            VetRegisters[HoldingRegOffset+RegAddressPWMmax] = new Register(EepromReadShort(eepromAddressPWMmax),eepromAddressPWMmax);
        }
}
void initHoldingInputReg()
{
    VetRegisters[HoldingInputOffset+RegAddressArmatureVoltage] = new Register(0,eepromDontUse);
    VetRegisters[HoldingInputOffset+RegAddressArmatureCurrent] = new Register(0,eepromDontUse);
    VetRegisters[HoldingInputOffset+RegAddressRPMread] = new Register(0,eepromDontUse);
}
void initInputReg(){}
void initOutputReg(){}
Register::Register(unsigned short initValue, unsigned short eepromAdress)
{
    _value = initValue;
    _eepromAddress = eepromAdress;
    if (_eepromAddress)
        EepromWrite(_eepromAddress, _value);
}
Register::~Register(){}
volatile unsigned short Register::read()
{
    unsigned short value = _value;
    return value;
}
volatile unsigned short Register::write(unsigned short value)
{
    _value = value;
    if (_eepromAddress)
        EepromWrite(_eepromAddress, _value);
    return value;
}
#endif