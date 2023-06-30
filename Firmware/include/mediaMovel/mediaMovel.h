#ifndef MEDIAMOVEL_H
#define MEDIAMOVEL_H
#include <Arduino.h>
class mediaMovel
{
private:
    unsigned char _counter = 0;
    unsigned int *_ptrValues = NULL;
    unsigned char _dimension;
public:
    mediaMovel(unsigned char dimension);
    ~mediaMovel();
    unsigned int newValue(unsigned int value);
};

#endif