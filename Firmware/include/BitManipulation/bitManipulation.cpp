#ifndef BITMANIPULATION_CPP
#define BITMANIPULATION_CPP
#include "bitManipulation.h"
void LittleEndianConvert(unsigned char *vet, unsigned short value)
{
    for (unsigned char i = 0; i < sizeof(short); i++)
    {
        vet[i] = (unsigned char)value;
        value = value >> 8;
    }
}
void LittleEndianConvert(unsigned char *vet, unsigned int value)
{
    for (unsigned char i = sizeof(int); i > 0; i--)
    {
        vet[i-1] = (unsigned char)value;
        value = value >> 8;
    }
}
void LittleEndianConvert(unsigned char *vet, unsigned short *value, unsigned short dim)
{
    for (unsigned char i = dim; i > 0; i--)
        BigEndianConvert(&vet[(i-1)*sizeof(short)], value[i-1]);
}
void LittleEndianConvert(unsigned char *vet, unsigned int *value, unsigned short dim)
{
    for (unsigned char i = dim; i > 0; i--)
        BigEndianConvert(&vet[(i-1)*sizeof(int)], value[i-1]);
}
void LittleEndianConvert(unsigned short *vet, unsigned int value)
{
     for (unsigned char i = (sizeof(int)/sizeof(short)); i > 0; i--)
    {
        vet[i-1] = (unsigned short)value;
        value = value >> (sizeof(short)*8);
    }
}
void LittleEndianConvert(unsigned short *vet, unsigned int *value, unsigned short dim)
{
    for (unsigned char i = dim; i > 0; i--)
        BigEndianConvert(&vet[(i-1)*sizeof(int)], value[i-1]);
}
void LittleEndianConvert(unsigned short *value, unsigned char *vet)
{
    for (unsigned char i = sizeof(short); i > 0; i--)
    {
        *value = (*value<<8);
        *value += vet[i];
    }
}
void LittleEndianConvert(unsigned int *value, unsigned char *vet)
{
    for (unsigned char i = sizeof(int); i > 0; i--)
    {
        *value = (*value<<8);
        *value += vet[i];
    }
}
void LittleEndianConvert(unsigned int *value, unsigned short *vet)
{
    for (unsigned char i = (sizeof(int)/sizeof(short)); i > 0; i--)
    {
        *value = (*value<<(sizeof(short) * 8));
        *value += vet[i];
    }
}

void BigEndianConvert(unsigned char *vet, unsigned short value)
{
    for (unsigned char i = sizeof(short); i > 0; i--)
    {
        vet[i-1] = (unsigned char)value;
        value = value >> 8;
    }
}
void BigEndianConvert(unsigned char *vet, unsigned int value)
{
    for (unsigned char i = 0; i < sizeof(int); i++)
    {
        vet[i] = (unsigned char)value;
        value = value >> 8;
    }
}
void BigEndianConvert(unsigned char *vet, unsigned short *value, unsigned short dim)
{
    for (unsigned char i = 0; i < dim; i++)
        BigEndianConvert(&vet[i*sizeof(short)], value[i]);
}
void BigEndianConvert(unsigned char *vet, unsigned int *value, unsigned short dim)
{
    for (unsigned char i = 0; i < dim; i++)
        BigEndianConvert(&vet[i*sizeof(int)], value[i]);
}
void BigEndianConvert(unsigned short *vet, unsigned int value)
{
    for (unsigned char i = 0; i < (sizeof(int) / sizeof(short)); i++)
    {
        vet[i] = (unsigned char)value;
        value = value >> (sizeof(short) * 8);
    }
}
void BigEndianConvert(unsigned short *vet, unsigned int *value, unsigned short dim)
{
    for (unsigned char i = 0; i < dim; i++)
        BigEndianConvert(&vet[i*(sizeof(int)/sizeof(short))], value[i]);
}
void BigEndianConvert(unsigned short *value, unsigned char *vet)
{
    for (unsigned char i = 0; i < sizeof(short); i++)
    {
        *value = (*value)<<8;
        *value += vet[i];
    }
}
void BigEndianConvert(unsigned int *value, unsigned char *vet)
{
    for (unsigned char i = 0; i < sizeof(int); i++)
    {
        *value = (*value<<8);
        *value += vet[i];
    }
}
void BigEndianConvert(unsigned int *value, unsigned short *vet)
{
    for (unsigned char i = 0; i < (sizeof(int) / sizeof(short)); i++)
    {
        *value = (*value << (8 * sizeof(short)));
        *value += vet[i];
    }
}
#endif