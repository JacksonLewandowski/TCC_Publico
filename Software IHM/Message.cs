using System;
using System.Collections.Generic;
using System.Linq;

namespace IHM_ESP
{
    public abstract class Message
    {   
        const int CHECKSUM_SIZE = 1; //Em bytes
        public byte Length { get; protected set; }
        public abstract byte Code { get; }
        public byte[] Data { get; protected set; }
        byte checkSum;

        public Message() { }
        public Message(byte[] data)
        {
            this.Data = data;
        }

        public byte[] Encode()
        {
            List<byte> buffer = new List<byte>
            {
                Code
            };

            if (Data != null)
                foreach (byte d in Data)
                    buffer.Add(d);

            Length = Convert.ToByte(buffer.Count + CHECKSUM_SIZE);
            buffer.Insert(0, Length);

            checkSum = Convert.ToByte(buffer.Sum(b => b) % 256);
            buffer.Add(checkSum);

            return buffer.ToArray();
        }

        #region EVENTOS
        public delegate void MessageEventHandler(Message message);
        public event MessageEventHandler OnMessageReceived;

        public virtual void MessageReceived(Message message)
        {
            OnMessageReceived?.Invoke(message);
        }
        #endregion
    }
}
