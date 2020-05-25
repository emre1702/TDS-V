namespace TDS_Client.Data.Interfaces.ModAPI.Voice
{
    public interface IVoiceAPI
    {
        #region Public Properties

        bool Allowed { get; }
        bool Muted { get; set; }

        #endregion Public Properties
    }
}
