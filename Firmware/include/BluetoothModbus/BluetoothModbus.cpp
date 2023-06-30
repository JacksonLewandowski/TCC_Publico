#ifndef BLUETOOTH_MODBUS_CPP
#define BLUETOOTH_MODBUS_CPP
#include "BluetoothModbus.h"
#include "UartSlaveModbus/uartModbus.h"

void initialize_bluetooth()
{
    BluetoothSerial SerialBT;
    SerialBT.begin();

    xTaskCreatePinnedToCore(bluetooth_task,"bluetooth_task",4096,NULL,1,NULL,PRO_CPU_NUM);
}
static void bluetooth_task(void *pvParameters)
{
    while (1)
    {
        uint8_t count = 0;
        uint8_t message_buffer[0xff] = {0};
        while(SerialBT.available())
            message_buffer[count++] = SerialBT.read();
        
        if(count)
            processMessage(message_buffer, count);

        vTaskDelay(pdMS_TO_TICKS(3));
    }
    vTaskDelete(NULL);
}
unsigned short returnCRC16(unsigned char *message, unsigned short dimension)
{
    unsigned char nTemp;
    unsigned short wCRCWord = 0xFFFF;
    while (dimension--)
    {
        nTemp = *message++ ^ wCRCWord;
        wCRCWord >>= 8;
        wCRCWord ^= wCRCTable[nTemp];
    }
    return wCRCWord;
}
void processMessage(unsigned char* Data, unsigned char dimension)
{
    if (dimension < 8)//dimensão minima da mensagem
        return;
    if (Data[0] != VetRegisters[HoldingRegOffset + RegAddressID]->read())//é para mim?
        return;
    if(ChecksCRC(Data,dimension) == 0)
        return;
    if (vectorFunc[Data[1]] == NULL)
    {
        /* função invalida */
        return;
    }
    vectorFunc[Data[1]]->func(Data,dimension);
}
unsigned char ChecksCRC(unsigned char* Data, unsigned char dimension){return 1;}  
void bluetooth_write(unsigned char* data, unsigned short dimension)
{
    unsigned char reply[dimension+2];
    memcpy(reply, data, dimension);
    unsigned short _crc = returnCRC16(data, dimension);
    LittleEndianConvert(&reply[dimension], _crc);
    SerialBT.write(reply, dimension + sizeof(_crc));
}

#endif