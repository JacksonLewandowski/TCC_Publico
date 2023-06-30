#ifndef FUNCTIONS_H
#define FUNCTIONS_H
#include "function.h"
#include "Registradores/Registradores.cpp"
#include <BitManipulation/bitManipulation.cpp>

void (*ptrFuncReply)(unsigned char*, unsigned short);
void initFunctions(void ptrFunc(unsigned char*, unsigned short));
class readDigOutput : public function
{
public:
    virtual void func(unsigned char *data, unsigned short dimendion);
};
class readDigInput : public function
{
public:
    virtual void func(unsigned char *data, unsigned short dimendion);
};
class readHoldingRegisters : public function
{
public:
    virtual void func(unsigned char *data, unsigned short dimendion);
};
class readInputRegister : public function
{
public:
    virtual void func(unsigned char *data, unsigned short dimendion);
};
class writeDigOutput : public function
{
public:
    virtual void func(unsigned char *data, unsigned short dimendion);
};
class writeSingleRegister: public function
{
public:
    virtual void func(unsigned char *data, unsigned short dimendion);
};
class writeMultipleDigOutput: public function
{
public:
    virtual void func(unsigned char *data, unsigned short dimendion);
};
class writeMultipleRegisters: public function
{
public:
    virtual void func(unsigned char *data, unsigned short dimendion);
};
class IllegalFunction: public function
{
public:
    virtual void func(unsigned char *data, unsigned short dimendion);
};
class llegalAddress: public function
{
public:
    virtual void func(unsigned char *data, unsigned short dimendion);
};
class llegalValue: public function
{
public:
    virtual void func(unsigned char *data, unsigned short dimendion);
};
#endif