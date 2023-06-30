using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IHM_ESP.Devices
{
    class ESP32_USB
    {
        /// <summary>
        /// Estado atual do dispositivo
        /// </summary>
        public enum DeviceState : UInt16
        {
            Idle = 1,
            Running = 2,
            Failure = 3
        }

        /// <summary>
        /// Registradores de entrada (apenas leitura)
        /// </summary>
        public enum InputRegisters : UInt16
        {
            Voltage = 1,
            Current = 2,
            RPM = 3
        }

        /// <summary>
        /// Registradores de uso geral (leitura e escrita)
        /// </summary>
        public enum HoldingRegisters : UInt16
        {
            Kp = 1,
            Ki = 2,
            Kd = 3,
            Pwm = 4,
            PwmInitialValue = 5,
            PwmMinValue = 6,
            PwmMaxValue = 7,
            FieldPwm = 8,
            SetPointRPM = 9,
            DeviceState = 10
        }
    }

    class ESP32RegisterAttribute :Attribute
    {
        public ESP32_USB.HoldingRegisters Register { get; protected set; }
        public string Description { get; protected set; }
        public UInt16 MinValue { get; set; }
        public UInt16 MaxValue { get; set; }
        public string UnitOfMeasurement { get; set; }
        public ESP32RegisterAttribute(string description, ESP32_USB.HoldingRegisters holdingRegister)
        {
            Register = holdingRegister;
            Description = description;
        }
    }
}
