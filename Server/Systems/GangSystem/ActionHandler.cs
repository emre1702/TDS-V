using GTANetworkAPI;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.GangsSystem;

namespace TDS.Server.GangsSystem
{
    public class ActionHandler : IGangActionHandler
    {
        public bool InAction { get; private set; }
        public int AttackCount { get; private set; }

        private readonly IGang _gang;
        private readonly ISettingsHandler _settingsHandler;

        public ActionHandler(IGang gang, ISettingsHandler settingsHandler)
        {
            _gang = gang;
            _settingsHandler = settingsHandler;
        }

        public void SetInAction(bool asAttacker)
        {
            InAction = true;
            ++AttackCount;
        }

        public void SetActionEnded()
        {
            InAction = false;
        }

        public bool CheckCanAttack(ITDSPlayer outputTo)
        {
            if (AttackCount >= _settingsHandler.ServerSettings.GangMaxGangActionAttacksPerDay)
            {
                outputTo.SendNotification(string.Format(outputTo.Language.YOUR_GANG_ALREADY_REACHED_MAX_ATTACK_COUNT, _settingsHandler.ServerSettings.GangMaxGangActionAttacksPerDay));
                return false;
            }

            if (InAction)
            {
                outputTo.SendNotification(outputTo.Language.YOUR_GANG_ALREADY_IN_ACTION);
                return false;
            }

            if (_gang.Players.CountOnline < _settingsHandler.ServerSettings.MinPlayersOnlineForGangAction)
            {
                outputTo.SendNotification(
                    string.Format(outputTo.Language.NOT_ENOUGH_PLAYERS_ONLINE_IN_YOUR_GANG, _settingsHandler.ServerSettings.MinPlayersOnlineForGangAction));
                return false;
            }

            return true;
        }
    }
}
