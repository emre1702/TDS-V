using TDS_Common.Instance.Utility;
using TDS_Server.Enum;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Utility;

namespace TDS_Server.Instance.Lobby
{ 
    partial class GangwarLobby
    {
        public override bool StartPreparations(TDSPlayer player)
        {
            if (!base.StartPreparations(player))
                return false;

            _actionChangeTimer = new TDSTimer(StopPreparations, SettingsManager.ServerSettings.GangwarPreparationTimeMs, 1);
            return true;
        }

        public override void StopPreparations()
        {
            base.StopPreparations();
            StartAction();
        }

        public override bool StartAction()
        {
            base.StartAction();

            _actionChangeTimer = new TDSTimer(StopAction, SettingsManager.ServerSettings.GangwarActionTimeMs, 1);
            return true;
        }

        public override void StopAction()
        {
            base.StopAction();
            StartEnd();
        }
    }
}
