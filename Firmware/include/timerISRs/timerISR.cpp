#ifndef TIMERISR_CPP
#define TIMERISR_CPP
#include "timerISR.h"
#include <inputRead/inputRead.cpp>
void initTimers()
{
    TimerAnalogRead = timerBegin(0, TimerDiv, true);
    timerAttachInterrupt(TimerAnalogRead, &ISR_analogRead, true);
    timerAlarmWrite(TimerAnalogRead, Timer_1ms, true);
    timerAlarmEnable(TimerAnalogRead);
    initInputs();
    Timer0 = TimerAnalogRead;
}
IRAM_ATTR void ISR_analogRead()
{
    counterTimer++;
#ifdef DELTA_T_MS
    if (counterTimer > DELTA_T_MS)
    {
        ISR_PIDFlag = 1;
        counterTimer = 0;
        //VetRegisters[HoldingInputOffset + RegAddressRPMread]->write(mediaRPM.newValue(150*RPMcounter));//150
        RPMcounter = 0;
    }
#endif // DELTA_T_MS
    
    /*if (timer10ms == 0)
    {
        armatureCurrentRead();
        armatureVoltageRead();
        //fieldCurrentRead();
    }
    timer10ms++;
    timer10ms%=10;*/
}
void armatureCurrentRead()
{
    unsigned short _value = 0;
    _value = ArmatureCurrent.read();
    if (ZeroCurrent > _value)
        _value = 0;
        else
            _value -= ZeroCurrent;
    _value = mediaArmatureCurrent.newValue(_value);
    VetRegisters[RegAddressArmatureCurrent + HoldingInputOffset]->write(_value);
}
void armatureVoltageRead()
{
    unsigned short _InputVolage = InputVoltage.read();
    //unsigned short _SwitchVoltage = SwitchVoltage.read();
    /*if (_SwitchVoltage > _InputVolage)
    {
        VetRegisters[RegAddressArmatureVoltage + HoldingInputOffset]->write(mediaArmatureVoltage.newValue(0));
        return;
    }*/
    VetRegisters[RegAddressArmatureVoltage + HoldingInputOffset]->write(mediaArmatureVoltage.newValue(_InputVolage));
}
void fieldCurrentRead()
{
    /*unsigned short _value = 0;
    _value = FieldCurrent.read();
    _value = mediaFieldCurrent.newValue(_value);*/
}

#endif