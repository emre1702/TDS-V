using TDS_Server.Data.Abstracts.Entities.GTA;

namespace TDS_Server.Data.Interfaces.PlayersSystem
{
    public interface IPlayerVoice
    {
        void Init(ITDSPlayer player);

        void ResetVoiceToAndFrom();

        void SetVoiceTo(ITDSPlayer target, bool on);
    }
}
