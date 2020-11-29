using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Enums;
using TDS.Server.Data.Interfaces.GangActionAreaSystem.Areas;
using TDS.Server.Data.Interfaces.LobbySystem.Actions;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS.Server.Handler;
using TDS.Server.Handler.GangSystem;
using TDS.Shared.Data.Enums;

namespace TDS.Server.LobbySystem.Actions
{
    public class GangLobbyActions : IGangLobbyActions
    {
        private readonly GangActionAreasHandler _gangActionAreasHandler;

        public GangLobbyActions(IGangLobby gangLobby, GangActionAreasHandler gangActionAreasHandler)
        {
            _gangActionAreasHandler = gangActionAreasHandler;
        } 

        public async Task TryStartAction(ITDSPlayer attacker, int gangActionAreaId)
        {
            if (!TryGetArea(attacker, gangActionAreaId, out var area))
                return;
            if (!CheckAllRequirements(attacker, area))
                return;

            await area.Action.Attack(attacker);
        }

        private bool TryGetArea(ITDSPlayer attacker, int gangActionAreaId, [NotNullWhen(true)] out IBaseGangActionArea? area)
        {
            area = _gangActionAreasHandler.GetById(gangActionAreaId);
            if (area is null)
            {
                attacker.SendNotification(attacker.Language.GANG_ACTION_AREA_DOES_NOT_EXIST);
                return false;
            }
            return true;
        }

        private bool CheckAllRequirements(ITDSPlayer attacker, IBaseGangActionArea area)
        {
            if (!attacker.Gang.PermissionsHandler.CheckIsAllowedTo(attacker, GangCommand.StartGangAction))
                return false;
            if (!attacker.Gang.Action.CheckCanAttack(attacker))
                return false;
            if (!area.StartRequirements.CheckIsAttackable(attacker))
                return false;
            if (attacker.Lobby?.Type != LobbyType.GangLobby)
            {
                LoggingHandler.Instance.LogError("Tried to start an action, but is not in GangLobby", Environment.StackTrace, null, attacker);
                return false;
            }

            return true;
        }
        

        /*
         *  public async Task StartGangwar(ITDSPlayer attacker, int gangwarAreaId)
        {
            if (gangwarArea.Owner is null)
            {
                var task = gangwarArea.SetConqueredWithoutAttack(attacker.Gang);
                if (task is { })
                    await task.ConfigureAwait(false);
                return;
            }

            if (!CheckCanStartAction(attacker, gangwarArea))
                return;
            if (!CheckCanStartGangwar(attacker, gangwarArea))
                return;

            gangwarArea.SetInPreparation(attacker.Gang);

            var lobby = ActivatorUtilities.CreateInstance<Arena>(_serviceProvider, CreateEntity(gangwarArea), gangwarArea, true);

            await lobby.AddToDB().ConfigureAwait(false);
            await NAPI.Task.RunWait(() =>
            {
                EventsHandler.OnLobbyCreated(lobby);
                lobby.SetMapList(new List<MapDto> { gangwarArea.Map });

                lobby.SetRoundStatus(RoundStatus.NewMapChoose);
                lobby.Start();
            }).ConfigureAwait(false);

            await lobby.AddPlayer(attacker, 1).ConfigureAwait(false);
        }

        }*/


    }
}
