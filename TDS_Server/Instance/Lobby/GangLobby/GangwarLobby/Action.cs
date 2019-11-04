using TDS_Common.Instance.Utility;
using TDS_Server.Enum;
using TDS_Server.Manager.Utility;

namespace TDS_Server.Instance.Lobby
{ 
    partial class GangwarLobby
    {
        public override void StartPreparations()
        {
            base.StartPreparations();

            _actionChangeTimer = new TDSTimer(StopPreparations, SettingsManager.ServerSettings.GangwarPreparationTimeMs, 1);
        }

        public override void StopPreparations()
        {
            base.StopPreparations();
            StartAction();
        }

        public override void StartAction()
        {
            base.StartAction();

            _actionChangeTimer = new TDSTimer(StopAction, SettingsManager.ServerSettings.GangwarActionTimeMs, 1);
        }

        public override void StopAction()
        {
            base.StopAction();
            StartEnd();
        }
    }
}
