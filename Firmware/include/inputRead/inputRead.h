#ifndef INPUTREAD_H
#define INPUTREAD_H
#include <Arduino.h>
#include <Registradores/Registradores.cpp>
#include <mediaMovel/mediaMovel.cpp>
#include <PID\PID.cpp>
void initInputs();
IRAM_ATTR void ISR_RPM();
void ISR_Overcurrent();
class analogInput
{
private:
    unsigned char _pin = 0;
public:
    analogInput(unsigned char pin, adc_attenuation_t attenuation);
    ~analogInput();
    unsigned short read();
};
analogInput ArmatureCurrent(pinArmatureCurrent, ADC_0db);
analogInput InputVoltage(pinInputVoltage, ADC_0db);
//analogInput SwitchVoltage(pinSwitchVoltage, ADC_0db);
mediaMovel mediaRPM(dimMediaMovel);
mediaMovel mediaArmatureCurrent(dimMediaMovel*10);
mediaMovel mediaArmatureVoltage(dimMediaMovel*10);
//mediaMovel mediaFieldCurrent(dimMediaMovel);
#endif