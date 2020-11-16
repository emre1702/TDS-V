namespace TDS.Client.Data.Models
{
    public class CooldownEventDto
    {
        public uint CooldownMs;
        public int LastExecMs = 0;

        public CooldownEventDto(uint cooldownMs) => CooldownMs = cooldownMs;
    }
}
