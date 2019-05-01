namespace TDS_Client.Dto
{
    internal class CooldownEventDto
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