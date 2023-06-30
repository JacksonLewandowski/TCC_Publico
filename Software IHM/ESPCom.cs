using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace IHM_ESP
{
    public class ESPCom
    {
        const int MAX_PWM = 1023;
        private readonly SerialPort serialPort;
        
        byte[] response;
        readonly int messageIntervalInTicks;
        public bool IsOpen
        { 
            get 
            {
                if (serialPort != null)
                    return serialPort.IsOpen;

                return false;
            } 
        }

        public ESPCom(string comPort, int baudRate)
        {
            serialPort = new SerialPort(comPort, baudRate);

            serialPort.Open();
            //messageIntervalInTicks = Convert.ToInt32(10 * 3.5 * Stopwatch.Frequency / baudRate); // 10 bits * 3.5 characters 
            messageIntervalInTicks = Convert.ToInt32(0.0015 * Stopwatch.Frequency); // 1.5 ms

        }     

        public double GetPWMDuty()
        {
            throw new NotImplementedException();
        }

        public int GetRPM()
        {
            throw new NotImplementedException();
        }

        public void SetPWM(double duty)
        {
            if (duty < 0 || duty > 1)
                throw new Exception("duty: " + duty.ToString() + " está fora do intervalo: 0 <= duty <= 1");

            int value = Convert.ToInt32(Math.Round(duty * MAX_PWM));
            byte[] setPwmMessage = Modbus.BuildSingleMessage(Modbus.Device.ESP_USB, 
                                                             Modbus.DataAccess.WRITE_SINGLE_REGISTER, 
                                                             (UInt16)Devices.ESP32_USB.HoldingRegisters.Pwm, 
                                                             (UInt16)value);

            SendMessage(setPwmMessage, setPwmMessage.Length);
        }

        public void SetPWM(int duty)
        {
            SetPWM(Convert.ToDouble(duty) / 100);
        }

        public void SetRPM(int rpm)
        {
            byte[] setRpmMessage = Modbus.BuildSingleMessage(Modbus.Device.ESP_USB,
                                                             Modbus.DataAccess.WRITE_SINGLE_REGISTER,
                                                             (UInt16)Devices.ESP32_USB.HoldingRegisters.SetPointRPM,
                                                             (UInt16)rpm);

            SendMessage(setRpmMessage, setRpmMessage.Length);
        }

        public void Start()
        {
            byte[] startMessage = Modbus.BuildSingleMessage(Modbus.Device.ESP_USB,
                                                            Modbus.DataAccess.WRITE_SINGLE_REGISTER,
                                                            (UInt16)Devices.ESP32_USB.HoldingRegisters.DeviceState,
                                                            (UInt16)Devices.ESP32_USB.DeviceState.Running);

            SendMessage(startMessage, startMessage.Length);
        }

        public void Stop()
        {
            byte[] stopMessage = Modbus.BuildSingleMessage(Modbus.Device.ESP_USB,
                                                           Modbus.DataAccess.WRITE_SINGLE_REGISTER,
                                                           (UInt16)Devices.ESP32_USB.HoldingRegisters.DeviceState,
                                                           (UInt16)Devices.ESP32_USB.DeviceState.Idle);

            SendMessage(stopMessage, stopMessage.Length);
        }

        protected bool SendMessage(byte[] message, int responseLength)
        {
            bool success = false;
            bool lockTaken = false;
            try
            {
                Monitor.TryEnter(serialPort, 150, ref lockTaken);
                if (serialPort.IsOpen)
                {
                    serialPort.Write(message, 0, message.Length);

                    if (!SpinWait.SpinUntil(() => serialPort.BytesToRead >= responseLength, 150))
                    {
                        response = Modbus.GetResponse(serialPort, serialPort.BytesToRead);
                        //serialPort.DiscardInBuffer();
                    }
                    else
                    {
                        response = Modbus.GetResponse(serialPort, responseLength);
                        success = Modbus.CheckResponse(response);
                    }
                }
            }
            finally
            {
                if (lockTaken)
                    Monitor.Exit(serialPort);
            }
            
            

            // Aguarda o intervalo mínimo entre mensagens (tempo de 3.5 caracteres)
            Stopwatch sw = Stopwatch.StartNew();
            while (sw.ElapsedTicks < messageIntervalInTicks) ;

            return success;
        }

        public byte[] RequestData()
        {   
            // Response:
            // Slave address    = 0x01          [1 byte ]
            // Function code    = 0x04          [1 byte ]
            // Bytes to follow  = 0x06          [1 byte ]
            // Content          = data          [6 bytes]
            // CRC              = crc           [2 bytes]
            const UInt16 responseLength = 11;
            const UInt16 numberOfRegistersRequested = 3;
            byte[] data = null;


            byte[] request = Modbus.BuildSingleMessage(Modbus.Device.ESP_USB,
                                                       Modbus.DataAccess.READ_INPUT_REGISTER,
                                                       (UInt16)Devices.ESP32_USB.InputRegisters.Voltage,
                                                       numberOfRegistersRequested);

            if (SendMessage(request, responseLength))
            {
                // Skip Slave address, fn code and bytes to follow
                data = new ArraySegment<byte>(response, 3, 6).ToArray();
            }

            return data;
        }

        public void SetMaxPwm(int duty)
        {
            byte[] maxPwmMessage = Modbus.BuildSingleMessage(Modbus.Device.ESP_USB,
                                                             Modbus.DataAccess.WRITE_SINGLE_REGISTER,
                                                             (UInt16)Devices.ESP32_USB.HoldingRegisters.PwmMaxValue,
                                                             (UInt16)duty);

            SendMessage(maxPwmMessage, maxPwmMessage.Length);
        }

        public void SetMaxPwm(double duty)
        {
            int value = Convert.ToInt32(Math.Round(duty * MAX_PWM));
            SetMaxPwm(value);
        }

        public void SetMinPwm(int duty)
        {
            byte[] minPwmMessage = Modbus.BuildSingleMessage(Modbus.Device.ESP_USB,
                                                             Modbus.DataAccess.WRITE_SINGLE_REGISTER,
                                                             (UInt16)Devices.ESP32_USB.HoldingRegisters.PwmMaxValue,
                                                             (UInt16)duty);

            SendMessage(minPwmMessage, minPwmMessage.Length);
        }

        public void SetMinPwm(double duty)
        {
            int value = Convert.ToInt32(Math.Round(duty * MAX_PWM));
            SetMinPwm(value);
        }


        public void SetP(int kp)
        {
            byte[] message = Modbus.BuildSingleMessage(Modbus.Device.ESP_USB,
                                                       Modbus.DataAccess.WRITE_SINGLE_REGISTER,
                                                       (UInt16)Devices.ESP32_USB.HoldingRegisters.Kp,
                                                       (UInt16)kp);

            SendMessage(message, message.Length);
        }

        public void SetI(int ki)
        {
            byte[] message = Modbus.BuildSingleMessage(Modbus.Device.ESP_USB,
                                                       Modbus.DataAccess.WRITE_SINGLE_REGISTER,
                                                       (UInt16)Devices.ESP32_USB.HoldingRegisters.Ki,
                                                       (UInt16)ki);

            SendMessage(message, message.Length);
        }

        public void SetD(int kd)
        {
            byte[] message = Modbus.BuildSingleMessage(Modbus.Device.ESP_USB,
                                                       Modbus.DataAccess.WRITE_SINGLE_REGISTER,
                                                       (UInt16)Devices.ESP32_USB.HoldingRegisters.Kd,
                                                       (UInt16)kd);

            SendMessage(message, message.Length);
        }

        public double GetCurrent()
        {
            int value = -1;

            byte[] message = Modbus.BuildSingleMessage(Modbus.Device.ESP_USB,
                                                       Modbus.DataAccess.READ_INPUT_REGISTER,
                                                       (UInt16)Devices.ESP32_USB.InputRegisters.Current,
                                                       1);

            if(SendMessage(message, message.Length - 2))
                value = BitConverter.ToInt32(new ArraySegment<byte>(response, 3, 2).ToArray(), 0);

            return value;
        }

        public double GetMaxPwm()
        {
            double pwm = -1.0;
            byte[] message = Modbus.BuildSingleMessage(Modbus.Device.ESP_USB,
                                                       Modbus.DataAccess.READ_HOLDING_REGISTERS,
                                                       (UInt16)Devices.ESP32_USB.HoldingRegisters.PwmMaxValue,
                                                       1);

            if(SendMessage(message, message.Length - 2))
                pwm = BitConverter.ToInt32(new ArraySegment<byte>(response, 3, 2).ToArray(), 0) / 1024;

            return pwm;
        }

        public double GetMinPwm()
        {
            double pwm = -1.0;
            byte[] message = Modbus.BuildSingleMessage(Modbus.Device.ESP_USB,
                                                       Modbus.DataAccess.READ_HOLDING_REGISTERS,
                                                       (UInt16)Devices.ESP32_USB.HoldingRegisters.PwmMinValue,
                                                       1);

            if (SendMessage(message, message.Length - 2))
                pwm = BitConverter.ToInt32(new ArraySegment<byte>(response, 3, 2).ToArray(), 0) / 1024;

            return pwm;
        }

        public double GetKp()
        {
            double kp = -1.0;
            byte[] message = Modbus.BuildSingleMessage(Modbus.Device.ESP_USB,
                                                       Modbus.DataAccess.READ_HOLDING_REGISTERS,
                                                       (UInt16)Devices.ESP32_USB.HoldingRegisters.Kp,
                                                       1);

            if (SendMessage(message, message.Length - 2))
                kp = BitConverter.ToInt32(new ArraySegment<byte>(response, 3, 2).ToArray(), 0);

            return kp;
        }

        public double GetKi()
        {
            double ki = -1.0;
            byte[] message = Modbus.BuildSingleMessage(Modbus.Device.ESP_USB,
                                                       Modbus.DataAccess.READ_HOLDING_REGISTERS,
                                                       (UInt16)Devices.ESP32_USB.HoldingRegisters.Ki,
                                                       1);

            if (SendMessage(message, message.Length - 2))
                ki = BitConverter.ToInt32(new ArraySegment<byte>(response, 3, 2).ToArray(), 0);

            return ki;
        }

        public double GetKd()
        {
            double kd = -1.0;
            byte[] message = Modbus.BuildSingleMessage(Modbus.Device.ESP_USB,
                                                       Modbus.DataAccess.READ_HOLDING_REGISTERS,
                                                       (UInt16)Devices.ESP32_USB.HoldingRegisters.Kd,
                                                       1);

            if (SendMessage(message, message.Length - 2))
                kd = BitConverter.ToInt32(new ArraySegment<byte>(response, 3, 2).ToArray(), 0);

            return kd;
        }

        public byte[] GetAllHoldingRegisters()
        {
            const int header_length = 3;
            const int crc_length = 2;
            const int response_length = 25;
            byte[] data = null;
            byte[] request = Modbus.BuildSingleMessage(Modbus.Device.ESP_USB,
                                                       Modbus.DataAccess.READ_HOLDING_REGISTERS,
                                                       (UInt16)Devices.ESP32_USB.HoldingRegisters.Kp,
                                                       (UInt16)Devices.ESP32_USB.HoldingRegisters.DeviceState);

            if(SendMessage(request, response_length))
            {
                data = new ArraySegment<byte>(response,
                                              header_length,
                                              response.Length - crc_length - header_length)
                                              .ToArray();
            }

            return data;
        }

        public bool SetAllHoldingRegisters(byte[] data)
        {   
            const int response_length = 8;
            byte[] request = Modbus.BuildWriteMultipleRegistersMessage(Modbus.Device.ESP_USB,
                                                                       Modbus.DataAccess.WRITE_MULTIPLE_REGISTERS,
                                                                       (UInt16)Devices.ESP32_USB.HoldingRegisters.Kp,
                                                                       (UInt16)(data.Length / 2),
                                                                       (byte)data.Length,
                                                                       data);

            return SendMessage(request, response_length);
        }

        public void Disconnect()
        {
            if (serialPort != null)
                serialPort.Close();
        }
    }
}