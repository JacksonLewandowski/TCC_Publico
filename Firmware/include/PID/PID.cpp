#ifndef PID_CPP
#define PID_CPP
#include "PID.h"

PID::PID(unsigned short Kp, unsigned short Ki, unsigned short Kd)
{
    _Kp = (double)Kp / 1000;
    _Ki = (double)Ki / 1000;
    _Kd = (double)Kd / 1000;
}
PID::~PID(){}
double PID::calculatePID(unsigned short setPoint, unsigned short value)
{
    unsigned long time = micros();
    //Dividindo por 1M para converter de microsegundos para segundos
    double dt = (time - last_time) / 1000000;
    last_time = time;

    if(dt > 0.25)
        return 0;

    double error = (double)setPoint - (double)value;
    double Proportional = error * _Kp;

    _integral += (_preError + error) * dt / 2;
    
    double Integral = _integral * _Ki;
    // double Derivative = _Kd * ((error - _preError) / _dt);
    double Derivative = 0;//_Kd * ((_preError - error) / dt);
    // Derivative = (_previous_value - value) * _Kd / dt;
    _previous_value = value;
    double output = Proportional + Integral + Derivative;
    
    _preError = error;

    return output;
}
void PID::setPID(unsigned short Kp, unsigned short Ki, unsigned short Kd)
{
    _Kp = (double)Kp / 1000;
    _Ki = (double)Ki / 1000;
    _Kd = (double)Kd / 1000;
}

void PID::setDT(unsigned int dt)
{
    _dt = (double)dt / 1000000;
}

void PID::restart()
{
    _integral = 0;
    _preError = 0;
    _previous_value = 0;
    last_time = micros();
}

#endif