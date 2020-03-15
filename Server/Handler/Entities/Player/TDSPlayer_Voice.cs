using TDS_Server.Data.Interfaces;

namespace TDS_Server.Handler.Entities.Player
{
    partial class TDSPlayer
    {
        public void SetVoiceTo(ITDSPlayer target, bool on)
        {
            ModPlayer?.SetVoiceTo(target, on);
        }
    }
}
