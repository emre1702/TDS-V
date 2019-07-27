namespace TDS_Client.Dto
{
    internal class CooldownEventDto
    {
        public ulong LastExecMs = 0;
        public uint CooldownMs;

        public CooldownEventDto(uint cooldownMs)
        {
            CooldownMs = cooldownMs;
        }
    }
}