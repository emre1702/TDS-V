using System;
using System.Collections.Generic;
using System.Text;

namespace TDS_Client.Dto
{
    class CooldownEventDto
    {
        public string EventName;
        public ulong LastExecMs = 0;
        public uint CooldownMs; 

        public CooldownEventDto(string eventName, uint cooldownMs)
        {
            EventName = eventName;
            CooldownMs = cooldownMs;
        }
    }
}
