#ifndef OUTPUTS_CPP
#define OUTPUTS_CPP
#include "Outputs.h"

PWMout::PWMout(unsigned char pin, unsigned char channel, unsigned short frequency, unsigned char typePWM)
{
    _typePWM = typePWM;
    _channel = channel;
    ledcSetup(_channel, frequency, 10);
    ledcAttachPin(pin,_channel);
    write(0);
}
PWMout::~PWMout(){}
unsigned char PWMout::write(unsigned short value)
{
    unsigned short current_value = ledcRead(_channel);
    if (current_value == value)
        return 0;
    if (value > PWM_MAX)
        return 0;
    if (_typePWM)
        value = 1023 - value;
    ledcWrite(_channel, value);
    return 1;
}
unsigned char PWMout::set()
{
    if (_typePWM)
        ledcWrite(_channel, 0);
        else
            ledcWrite(_channel, 1023);
    return 1;
}
unsigned char PWMout::reset()
{
    if (_typePWM)
        ledcWrite(_channel, 1023);
        else
            ledcWrite(_channel, 0);
    return 1;
}

#endif