using GTANetworkAPI;
using TDS_Common.Instance.Utility;
using TDS_Server.Manager.Utility;

namespace TDS_Server.Instance.Lobby
{ 
    partial class GangwarLobby
    {
        public override bool StartPreparations()
        {
            if (!base.StartPreparations())
                return false;

            _actionChangeTimer = new TDSTimer(StopPreparations, SettingsManager.ServerSettings.GangwarPreparationTimeMs, 1);

            SendAllPlayerLangNotification(lang => lang.GANGWAR_PREPARATION_INFO);
            SendLangMessageToAttacker(lang => string.Format(lang.GANGWAR_ATTACKER_PREPARATION_INFO, _gangwarArea.Entity.Map.Name, OwnerTeam.Entity.Name));
            _gangwarArea.SetInPreparation(this);
            

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

            SendAllPlayerLangNotification(lang => string.Format(lang.GANGWAR_STARTED_INFO, AttackerTeam.Entity.Name, _gangwarArea.Entity.Map.Name, OwnerTeam.Entity.Name));
            SendLangMessageToAttacker(lang => string.Format(lang.GANGWAR_ATTACKER_STARTED_INFO, _gangwarArea.Entity.Map.Name, OwnerTeam.Entity.Name));
            _gangwarArea.SetInAttack(this);

            return true;
        }

        public override void StopAction()
        {
            base.StopAction();
            StartEnd();
        }
    }
}
