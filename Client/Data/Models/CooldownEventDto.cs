namespace TDS_Client.Data.Models
{
    public class CooldownEventDto
    {
        public int LastExecMs = 0;
        public uint CooldownMs;

        public CooldownEventDto(uint cooldownMs)
        {
            CooldownMs = cooldownMs;
        }
    }
}
