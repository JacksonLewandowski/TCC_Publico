#ifndef OUTPUTS_H
#define OUTPUTS_H
#include <Arduino.h>
#define PWM_LOW 1
#define PWM_HIGH 0
class PWMout
{
private:
    unsigned char _channel = 0;
    unsigned char _typePWM = 0;
public:
    PWMout(unsigned char pin, unsigned char channel, unsigned short frequency, unsigned char typePWM);
    ~PWMout();
    unsigned char write(unsigned short value);
    unsigned char set();
    unsigned char reset();

    const static uint16_t PWM_MAX = 1000;
};
PWMout pwmArmature(pinArmaturePWM, 0, DefaultPWMFrequency, PWM_HIGH);
PWMout pwmField(pinFieldPWM, 1, DefaultPWMFrequency, PWM_HIGH);
#endif