#ifndef FUNCTION_H
#define FUNCTION_H
#include <Arduino.h>
#include "constants.h"
class function
{
public:
    virtual void func(unsigned char *data, unsigned short dimendion) = 0;
};
function *vectorFunc[17] = {NULL};

#endif