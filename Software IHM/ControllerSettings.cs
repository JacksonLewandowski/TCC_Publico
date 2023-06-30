using IHM_ESP.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IHM_ESP
{
    public class ControllerSettings
    {
        const int register_index_offset = 1;

        [ESP32Register("Ganho proporcional x 1k", ESP32_USB.HoldingRegisters.Kp, MinValue = 0, MaxValue = UInt16.MaxValue)]
        public UInt16 Kp { get; set; }

        [ESP32Register("Ganho integral x 1k", ESP32_USB.HoldingRegisters.Ki, MinValue = 0, MaxValue = UInt16.MaxValue)] 
        public UInt16 Ki { get; set; }

        [ESP32Register("Ganho derivativo x 1k", ESP32_USB.HoldingRegisters.Kd, MinValue = 0, MaxValue = UInt16.MaxValue)]
        public UInt16 Kd { get; set; }
        
        [ESP32Register("Valor atual do PWM", ESP32_USB.HoldingRegisters.Pwm, MinValue = 0, MaxValue = 1023)] 
        public UInt16 Pwm { get; set; }

        [ESP32Register("Valor inicial do PWM", ESP32_USB.HoldingRegisters.PwmInitialValue, MinValue = 0, MaxValue = 1023)]
        public UInt16 PwmInitial { get; set; }

        [ESP32Register("Valor mínimo de PWM", ESP32_USB.HoldingRegisters.PwmMinValue, MinValue = 0, MaxValue = 1023)]
        public UInt16 PwmMin { get; set; }

        [ESP32Register("Valor máximo de PWM", ESP32_USB.HoldingRegisters.PwmMaxValue, MinValue = 0, MaxValue = 1023)]
        public UInt16 PwmMax { get; set; }

        [ESP32Register("Valor do PWM de campo", ESP32_USB.HoldingRegisters.FieldPwm, MinValue = 0, MaxValue = 1023)]
        public UInt16 FieldPwm { get; set; }

        [ESP32Register("Velocidade alvo(RPM)", ESP32_USB.HoldingRegisters.SetPointRPM, MinValue = 200, MaxValue = 2000)]
        public UInt16 SetPointRPM { get; set; }

        [ESP32Register("Estado do controlador", ESP32_USB.HoldingRegisters.DeviceState, MinValue = 1, MaxValue = 3)]
        public UInt16 DeviceState { get; set; }

        public void FillFromBytes(byte[] data)
        {
            foreach(PropertyInfo info in GetType().GetProperties())
            {
                ESP32RegisterAttribute attr = info.GetCustomAttribute<ESP32RegisterAttribute>();
                
                int i = ((int)attr.Register - register_index_offset) * sizeof(UInt16);
                UInt16 value = (UInt16)((data[i] << 8) + data[i + 1]);
                info.SetValue(this, value);
            }
        }

        public byte[] ConvertToBytes()
        {
            List<byte> data = new List<byte>();
            foreach (PropertyInfo info in GetType().GetProperties())
            {
                ESP32RegisterAttribute attr = info.GetCustomAttribute<ESP32RegisterAttribute>();

                int i = ((int)attr.Register - register_index_offset) * sizeof(UInt16);
                UInt16 value = (UInt16)info.GetValue(this);
                data.Insert(i, (byte)(value >> 8));
                data.Insert(i+1, (byte)value);
            }

            return data.ToArray();
        }
    }
}
