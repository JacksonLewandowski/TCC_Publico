namespace IHM_ESP
{
    class MotorStateMessage : Message
    {
        public override byte Code => 99;
    }

    class StartMessage : Message
    {
        public override byte Code => 19;
    }

    class StopMessage : Message
    {
        public override byte Code => 20;
    }
    class DataMessage : Message
    {
        public override byte Code => 09;
    }

    #region Setters  // Ok

    class SetPwmMessage : Message // Ok
    {
        public SetPwmMessage() { }
        public SetPwmMessage(byte[] data) : base(data) {}

        public override byte Code => 01;
    }
    class SetRpmMessage : Message // Ok
    {
        public SetRpmMessage() { }
        public SetRpmMessage(int rpm)
        {
            Data = System.BitConverter.GetBytes(rpm);
            System.Array.Reverse(Data);
        }

        public override byte Code => 03;
    }
    class SetMaxPwmMessage : Message // Ok
    {
        public SetMaxPwmMessage() { }

        public SetMaxPwmMessage(int duty)
        {
            Data = System.BitConverter.GetBytes(duty);
            System.Array.Reverse(Data);
        }

        public override byte Code => 08;
    }
    class SetMinPwmMessage : Message // Ok
    {
        public SetMinPwmMessage() { }
        public SetMinPwmMessage(int duty)
        {
            Data = System.BitConverter.GetBytes(duty);
            System.Array.Reverse(Data);
        }

        public override byte Code => 11;
    }
    class SetMaxCurrentMessage : Message // Ok
    {

        public override byte Code => 05;
    }
    class SetPMessage : Message // Ok
    {
        public override byte Code => 16;
    }
    class SetIMessage : Message // Ok
    {
        public override byte Code => 17;
    }
    class SetDMessage : Message // Ok
    {
        public override byte Code => 18;
    }
    #endregion

    #region Getters // Ok
    class GetPwmMessage : Message // Ok
    {
        public override byte Code => 02;
    }
    class GetRpmMessage : Message // Ok
    {
        public override byte Code => 04;
    }
    class GetMaxCurrentMessage : Message // Ok
    {
        public override byte Code => 06;
    }
    class GetCurrentMessage : Message // Ok
    {
        public override byte Code => 07;
    }
    class GetMaxPwmMessage : Message // Ok
    {
        public override byte Code => 10;
    }
    class GetMinPwmMessage : Message // Ok
    {
        public override byte Code => 12;
    }
    class GetPMessage : Message // Ok
    {
        public override byte Code => 13;
    }
    class GetIMessage : Message // Ok
    {
        public override byte Code => 14;
    }
    class GetDMessage : Message // Ok
    {
        public override byte Code => 15;
    }

    #endregion
}
