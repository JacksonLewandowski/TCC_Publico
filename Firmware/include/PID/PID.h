#ifndef PID_H
#define PID_H
#include <Arduino.h>
class PID
{
private:
    double _Kp = 0;
    double _Ki = 0;
    double _Kd = 0;
    double _saturation = 0;
    double _integral = 0;
    double _preError = 0;
    double _dt = 0;
    double last_time = 0;
    unsigned short _previous_value = 0;
public:
    PID(unsigned short Kp, unsigned short Ki, unsigned short Kd);
    ~PID();
    double calculatePID(unsigned short setPoint, unsigned short value);
    void setPID(unsigned short Kp, unsigned short Ki, unsigned short Kd);
    void setDT(unsigned int dt);
    void restart();
};
PID *armaturePID = NULL;


#endif