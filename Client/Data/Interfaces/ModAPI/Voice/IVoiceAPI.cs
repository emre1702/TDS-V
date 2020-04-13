namespace TDS_Client.Data.Interfaces.ModAPI.Voice
{
    public interface IVoiceAPI
    {
        bool Muted { get; set; }
        bool Allowed { get; }
    }
}
