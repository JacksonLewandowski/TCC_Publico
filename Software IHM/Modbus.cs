using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IHM_ESP
{   
    public static class Modbus
    {       
        public enum Device : byte
        {
            ESP_USB = 0x01
        }
        public enum DataAccess : byte
        {
            READ_DISCRETE_INPUT             = 0x02,
            READ_COILS                      = 0x01,
            WRITE_SINGLE_COIL               = 0x05,
            WRITE_MULTIPLE_COILS            = 0x0F,
            READ_INPUT_REGISTER             = 0x04,
            READ_HOLDING_REGISTERS          = 0x03,
            WRITE_SINGLE_REGISTER           = 0x06,
            WRITE_MULTIPLE_REGISTERS        = 0x10,
            READ_WRITE_MULTIPLE_REGISTERS   = 0x17,
            MASK_WRITE_REGISTER             = 0x16,
            READ_FIFO_QUEUE                 = 0x18
        }

        /// <summary>
        /// Calcula e retorna o CRC da mensagem.
        /// </summary>
        /// <param name="message">Mensagem que será usada para gerar o CRC</param>
        /// <returns>CRC de 2 bytes (WORD)</returns>
        private static byte[] GetCRC(byte[] message)
        {
            byte[] CRC = new byte[2];
            ushort CRCFull = 0xFFFF;
            byte CRCHigh = 0xFF, CRCLow = 0xFF;
            char CRCLSB;

            for (int i = 0; i < (message.Length) - 2; i++)
            {
                CRCFull = (ushort)(CRCFull ^ message[i]);

                for (int j = 0; j < 8; j++)
                {
                    CRCLSB = (char)(CRCFull & 0x0001);
                    CRCFull = (ushort)((CRCFull >> 1) & 0x7FFF);

                    if (CRCLSB == 1)
                        CRCFull = (ushort)(CRCFull ^ 0xA001);
                }
            }
            CRC[1] = CRCHigh = (byte)((CRCFull >> 8) & 0xFF);
            CRC[0] = CRCLow = (byte)(CRCFull & 0xFF);

            return CRC;
        }

        /// <summary>
        /// Formata uma mensagem de leitura ou escrita simples.
        /// </summary>
        /// <param name="device">Dispositivo para o qual a mensagem é endereçada.</param>
        /// <param name="fnCode">Tipo de mensagem.</param>
        /// <param name="startingAddress">Endereço inicial a ser lido/escrito.</param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] BuildSingleMessage(Device device, DataAccess fnCode, UInt16 startingAddress, UInt16 value)
        {
            byte[] message = new byte[8];

            message[0] = (byte)device;
            message[1] = (byte)fnCode;
            message[2] = (byte)(startingAddress >> 8);
            message[3] = (byte)startingAddress;
            message[4] = (byte)(value >> 8);
            message[5] = (byte)value;
            
            byte[] crc = GetCRC(message);
            message[6] = crc[0];
            message[7] = crc[1];

            return message;
        }

        public static byte[] BuildWriteMultipleRegistersMessage(Device device, DataAccess fnCode, UInt16 startingAddress, 
                                                                UInt16 numberOfRegisters, byte numberOfDataBytes, byte[] data)
        {
            const int offset = 7;
            byte[] message = new byte[numberOfDataBytes + 9];

            message[0] = (byte)device;
            message[1] = (byte)fnCode;
            message[2] = (byte)(startingAddress >> 8);
            message[3] = (byte)startingAddress;
            message[4] = (byte)(numberOfRegisters >> 8);
            message[5] = (byte)numberOfRegisters;
            message[6] = numberOfDataBytes;

            for (int i = 0; i < numberOfDataBytes; i++)
                message[i + offset] = data[i];

            byte[] crc = GetCRC(message);
            message[offset + numberOfDataBytes] = crc[0];
            message[offset + numberOfDataBytes + 1] = crc[1];

            return message;
        }

        public static byte[] GetResponse(SerialPort stream, int length)
        {
            byte[] response = new byte[length];
            for (int i = 0; i < length; i++)
            {
                response[i] = (byte)stream.ReadByte();
            }

            return response;
        }

        /// <summary>
        /// Calcula o CRC da resposta e verifica se a mensagem está corrompida.
        /// </summary>
        /// <param name="response">Mensagem para verificar</param>
        /// <returns>true: se a resposta é válida, false: caso o CRC calculado esteja diferente do recebido.</returns>
        public static bool CheckResponse(byte[] response)
        {   
            byte[] CRC = GetCRC(response);

            return CRC[0] == response[response.Length - 2] && CRC[1] == response[response.Length - 1];
        }

    }
}
