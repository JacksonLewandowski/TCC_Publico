#ifndef UARTMODBUS_CPP
#define UARTMODBUS_CPP
#include "uartModbus.h"
void initialize_uart()
{
    uart_param_config(SLAVE_UART, &uart_config);
    uart_set_pin(SLAVE_UART, UART_PIN_NO_CHANGE, UART_PIN_NO_CHANGE,
                             UART_PIN_NO_CHANGE, UART_PIN_NO_CHANGE);
    uart_driver_install(SLAVE_UART, BUF_SIZE * 2, BUF_SIZE * 2, 10, &uart0_queue, 0);
    // uart_enable_pattern_det_baud_intr(SLAVE_UART, 0x0a, 1, 1000, 0, 0);
    xTaskCreatePinnedToCore(uart_event_task,"uart_event_task",4096,NULL,1,NULL,APP_CPU_NUM);
}
static void uart_event_task(void *pvParameters)
{
    uart_event_t event;
    while (1)
    {
        if (xQueueReceive(uart0_queue, (void * )&event, (portTickType)portMAX_DELAY))
            if (event.type == UART_DATA)
            {
                int len  = uart_read_bytes(UART_NUM_0, data_uart, BUF_SIZE, portTICK_RATE_MS);
                processMessage(data_uart,(unsigned char)len);
            }
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
void processMessage(const uint8_t *Data, unsigned char dimension)
{
    if (dimension < 8)//dimensão minima da mensagem
        return;
    if (Data[0] != VetRegisters[HoldingRegOffset + RegAddressID]->read())//é para mim?
        return;
    // if(ChecksCRC(Data, dimension) == 0)
    //     return;
    if (vectorFunc[Data[1]] == NULL)
    {
        /* função invalida */
        return;
    }
    vectorFunc[Data[1]]->func((unsigned char*)Data,dimension);
}
unsigned char ChecksCRC(const uint8_t *Data, unsigned char dimension){return 1;}  
void sendUart(unsigned char* data, unsigned short dimension)
{
    unsigned char reply[dimension+2];
    memcpy(reply, data, dimension);
    unsigned short _crc = returnCRC16(data, dimension);
    LittleEndianConvert(&reply[dimension], _crc);
    uart_write_bytes(SLAVE_UART, (const char*)reply, dimension+2);
}

#endif