namespace TDS_Client.Data.Models
{
    public class CooldownEventDto
    {
        #region Public Fields

        public uint CooldownMs;
        public int LastExecMs = 0;

        #endregion Public Fields

        #region Public Constructors

        public CooldownEventDto(uint cooldownMs)
        {
            CooldownMs = cooldownMs;
        }

        #endregion Public Constructors
    }
}
