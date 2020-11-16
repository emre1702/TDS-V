using TDS.Server.Data.Abstracts.Entities.GTA;

namespace TDS.Server.Data.Interfaces.PlayersSystem
{
    public interface IPlayerVoice
    {
        void Init(ITDSPlayer player);

        void ResetVoiceToAndFrom();

        void SetVoiceTo(ITDSPlayer target, bool toggleOn);
    }
}
