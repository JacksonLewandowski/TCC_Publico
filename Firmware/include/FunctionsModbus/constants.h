#ifndef CONSTANTS_H
#define CONSTANTS_H
#pragma once
//Codes
const unsigned char codeReadDigOutput = 1;
const unsigned char codeReadDigInput = 2;
const unsigned char codeReadHoldingRegisters = 3;
const unsigned char codeReadInputRegister = 4;
const unsigned char codeWriteDigOutput  = 5;
const unsigned char codeWriteSingleRegister = 6;
const unsigned char codeWriteMultipleDigOutput = 15;
const unsigned char codeWriteMultipleRegisters = 16;
//exceptions
const unsigned char codeOffsetException = 0b10000000;
const unsigned char codeExceptionIllegalFunction = 0x01;
const unsigned char codeExceptionIllegalAddress = 0x02;
const unsigned char codeExceptionIllegalValue = 0x03;
const unsigned char codeExceptionSlaveError = 0x04;
const unsigned char codeExceptionAcknowledge = 0x05;
const unsigned char codeExceptionSlaveBusy  = 0x06;
const unsigned char codeExceptionNegativeAcknowledge = 0x07;
const unsigned char codeExceptionMemoryParity = 0x08;
const unsigned char codeExceptionGatewayPathUnavailable = 0x0A;
const unsigned char codeExceptionFailToRespond = 0x0B;

#endif