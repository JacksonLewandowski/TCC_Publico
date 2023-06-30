#ifndef INPUTREAD_CPP
#define INPUTREAD_CPP
#include "inputRead.h"
void initInputs()
{
    pinMode(pinReadRPM, INPUT_PULLUP);
    //attachInterrupt(pinReadRPM, &ISR_RPM, RISING);//desativei a interrupção de leitura de rpm
    pinMode(pinArmatureOvercurrent, INPUT_PULLUP);
}
IRAM_ATTR void ISR_RPM()
{
    if (!digitalRead(pinReadRPM))
        return;
    RPMcounter++;
}
void ISR_Overcurrent()
{
    VetRegisters[HoldingRegOffset + RegAddressState]->write(StateFailure);
    detachInterrupt(pinArmatureOvercurrent);
}
analogInput::analogInput(unsigned char pin, adc_attenuation_t attenuation)
{
    _pin = pin;
    adcAttachPin(_pin);
    analogSetPinAttenuation(_pin,attenuation);
}
analogInput::~analogInput(){}
unsigned short analogInput::read(){return analogRead(_pin);}
#endif