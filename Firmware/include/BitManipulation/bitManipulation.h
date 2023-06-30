#ifndef BITMANIPULATION_H
#define BITMANIPULATION_H
#pragma once

void LittleEndianConvert(unsigned char *vet, unsigned short value);
void LittleEndianConvert(unsigned char *vet, unsigned int value);
void LittleEndianConvert(unsigned char *vet, unsigned short *value, unsigned short dim);
void LittleEndianConvert(unsigned char *vet, unsigned int *value, unsigned short dim);
void LittleEndianConvert(unsigned short *vet, unsigned int value);
void LittleEndianConvert(unsigned short *vet, unsigned int *value, unsigned short dim);
void LittleEndianConvert(unsigned short *value, unsigned char *vet);
void LittleEndianConvert(unsigned int *value, unsigned char *vet);
void LittleEndianConvert(unsigned int *value, unsigned short *vet);
//**********************************************************************
void BigEndianConvert(unsigned char *vet, unsigned short value);
void BigEndianConvert(unsigned char *vet, unsigned int value);
void BigEndianConvert(unsigned char *vet, unsigned short *value, unsigned short dim);
void BigEndianConvert(unsigned char *vet, unsigned int *value, unsigned short dim);
void BigEndianConvert(unsigned short *vet, unsigned int value);
void BigEndianConvert(unsigned short *vet, unsigned int *value, unsigned short dim);
void BigEndianConvert(unsigned short *value, unsigned char *vet);
void BigEndianConvert(unsigned int *value, unsigned char *vet);
void BigEndianConvert(unsigned int *value, unsigned short *vet);


#endif