using TDS.Server.Data.Abstracts.Entities.GTA;

namespace TDS.Server.Data.Interfaces.PlayersSystem
{
    public interface IPlayerMuteHandler
    {
        bool IsMuted { get; }
        bool IsPermamuted { get; }
        bool IsVoiceMuted { get; }
        int? MuteTime { get; set; }
        int? VoiceMuteTime { get; set; }

        void ChangeMuteTime(ITDSPlayer admin, int minutes, string reason);

        void ChangeVoiceMuteTime(ITDSPlayer admin, int minutes, string reason);

        void Init(ITDSPlayer player);
    }
}
