#ifndef MEDIAMOVEL_CPP
#define MEDIAMOVEL_CPP
#include "mediaMovel.h"

mediaMovel::mediaMovel(unsigned char dimension)
{
    _dimension = dimension;
    _ptrValues = new unsigned int[_dimension];
}
mediaMovel::~mediaMovel(){delete _ptrValues;}
unsigned int mediaMovel::newValue(unsigned int value)
{
    unsigned int _value = 0;
    _ptrValues[_counter] = value;
    for (unsigned char i = 0; i < _dimension; i++)
        _value += _ptrValues[i];
    _counter++;
    _counter %= _dimension;
    return (_value / _dimension);
}


#endif