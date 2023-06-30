#ifndef FUNCTIONS_CPP
#define FUNCTIONS_CPP
#include "functions.h"
void initFunctions(void ptrFunc(unsigned char*, unsigned short))
{
    ptrFuncReply = ptrFunc;
    vectorFunc[codeReadDigOutput] = new readDigOutput;
    vectorFunc[codeReadDigInput] = new readDigInput;
    vectorFunc[codeReadHoldingRegisters] = new readHoldingRegisters;
    vectorFunc[codeReadInputRegister] = new readInputRegister;
    vectorFunc[codeWriteDigOutput] = new writeDigOutput;
    vectorFunc[codeWriteSingleRegister] = new writeSingleRegister;
    vectorFunc[codeWriteMultipleDigOutput] = new writeMultipleDigOutput;
    vectorFunc[codeWriteMultipleRegisters] = new writeMultipleRegisters;
}
void readDigOutput::func(unsigned char *data, unsigned short dimendion)
{
    unsigned short address = 0;
    unsigned short total = 0;
    BigEndianConvert(&address, &data[2]);
    BigEndianConvert(&total, &data[4]);
    /*read*/
    return;
    /*reply*/
}
void readDigInput::func(unsigned char *data, unsigned short dimendion)
{ 
    unsigned short address = 0;
    unsigned short total = 0;
    BigEndianConvert(&address, &data[2]);
    BigEndianConvert(&total, &data[4]);
    /*read*/
    return;
    /*reply*/
}
void readHoldingRegisters::func(unsigned char *data, unsigned short dimendion)
{
    unsigned short address = 0;
    unsigned short total = 0;
    BigEndianConvert(&address, &data[2]);
    BigEndianConvert(&total, &data[4]);
    unsigned short replyDim = (total * 2) + 3;
    unsigned char reply[replyDim];
    reply[0] = data[0];
    reply[1] = data[1];
    reply[2] = total * 2;
    for (unsigned short i = 0; i < total; i++)
    {
        if (VetRegisters[address + i + HoldingRegOffset] == NULL)
            return;
        BigEndianConvert(&reply[i*2 + 3], VetRegisters[address + i + HoldingRegOffset]->read());
    }
    (*ptrFuncReply)(reply,replyDim);
}
void readInputRegister::func(unsigned char *data, unsigned short dimendion)
{ 
    unsigned short address = 0;
    unsigned short total = 0;
    BigEndianConvert(&address, &data[2]);
    BigEndianConvert(&total, &data[4]);
    unsigned short replyDim = (total * 2) + 3;
    unsigned char reply[replyDim];
    reply[0] = data[0];
    reply[1] = data[1];
    reply[2] = total * 2;
    for (unsigned short i = 0; i < total; i++)
    {
        if (VetRegisters[address + i + HoldingInputOffset] == NULL)
            return;
        BigEndianConvert(&reply[i*2 + 3], VetRegisters[address + i + HoldingInputOffset]->read());
    }
    (*ptrFuncReply)(reply,replyDim);
}
void writeDigOutput::func(unsigned char *data, unsigned short dimendion)
{ 
    unsigned short address = 0;
    unsigned short total = 0;
    BigEndianConvert(&address, &data[2]);
    BigEndianConvert(&total, &data[4]);
    /*read*/
    return;
    /*reply*/
}
void writeSingleRegister::func(unsigned char *data, unsigned short dimendion)
{   
    unsigned short address = 0;
    unsigned short value = 0;
    BigEndianConvert(&address, &data[2]);
    BigEndianConvert(&value, &data[4]);
    if (VetRegisters[address + HoldingRegOffset] == NULL)
        return;
    VetRegisters[address + HoldingRegOffset]->write(value);
    unsigned short replyDim = 6;
    unsigned char reply[replyDim];
    memcpy(reply,data,replyDim);
    (*ptrFuncReply)(reply,replyDim);
}
void writeMultipleDigOutput::func(unsigned char *data, unsigned short dimendion)
{
    unsigned short address = 0;
    unsigned short total = 0;
    BigEndianConvert(&address, &data[2]);
    BigEndianConvert(&total, &data[4]);
    /*read*/
    return;
    /*reply*/
}
void writeMultipleRegisters::func(unsigned char *data, unsigned short dimendion)
{
    unsigned short address = 0;
    unsigned short total = 0;
    BigEndianConvert(&address, &data[2]);
    BigEndianConvert(&total, &data[4]);
    // unsigned char bytesNum = data[6];
    unsigned short value = 0;
    for (unsigned short i = 0; i < total; i++)
    {
        if (VetRegisters[address + HoldingRegOffset + i] == NULL)
            return;
        value = 0;
        BigEndianConvert(&value, &data[7 + i*2]);
        VetRegisters[address + HoldingRegOffset + i]->write(value);
    }
    unsigned short replyDim = 6;
    unsigned char reply[replyDim];
    memcpy(reply,data,replyDim);
    (*ptrFuncReply)(reply,replyDim);
}
void IllegalFunction::func(unsigned char *data, unsigned short dimendion)
{
    unsigned short address = 0;
    unsigned short total = 0;
    BigEndianConvert(&address, &data[2]);
    BigEndianConvert(&total, &data[4]);
    /*read*/
    return;
    /*reply*/
}
void llegalAddress::func(unsigned char *data, unsigned short dimendion)
{
    unsigned short address = 0;
    unsigned short total = 0;
    BigEndianConvert(&address, &data[2]);
    BigEndianConvert(&total, &data[4]);
    /*read*/
    return;
    /*reply*/
}
void llegalValue::func(unsigned char *data, unsigned short dimendion)
{
    unsigned short address = 0;
    unsigned short total = 0;
    BigEndianConvert(&address, &data[2]);
    BigEndianConvert(&total, &data[4]);
    /*read*/
    return;
    /*reply*/
}

#endif