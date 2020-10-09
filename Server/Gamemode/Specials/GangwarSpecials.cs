using GTANetworkAPI;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.GamemodesSystem.Gamemodes;
using TDS_Server.Data.Interfaces.GamemodesSystem.Specials;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Data.Interfaces.LobbySystem.RoundsHandlers.Datas.RoundStates;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Utility;
using TDS_Shared.Default;

namespace TDS_Server.GamemodesSystem.Specials
{
    public class GangwarSpecials : BaseGamemodeSpecials, IGangwarGamemodeSpecials
    {
        private ITDSPlayer? _playerForcedAtTarget;

        private readonly IGangwarGamemode _gamemode;
        private readonly ISettingsHandler _settingsHandler;

        public GangwarSpecials(IRoundFightLobby lobby, IGangwarGamemode gamemode, ISettingsHandler settingsHandler) : base(lobby)
        {
            _gamemode = gamemode;
            _settingsHandler = settingsHandler;
        }

        internal override void AddEvents(IRoundFightLobbyEventsHandler events)
        {
            base.AddEvents(events);

            events.PlayerDied += PlayerDied;
            events.PlayerLeftAfter += PlayerLeftAfter;
        }

        internal override void RemoveEvents(IRoundFightLobbyEventsHandler events)
        {
            base.RemoveEvents(events);

            events.PlayerDied -= PlayerDied;
            if (events.PlayerLeftAfter is { })
                events.PlayerLeftAfter -= PlayerLeftAfter;
        }

        public void PlayerEnteredTargetColShape(ITDSPlayer player)
        {
        }

        public void PlayerExitedTargetColShape(ITDSPlayer player)
        {
        }

        private void PlayerDied(ITDSPlayer player, ITDSPlayer killer, uint weapon, int hadLifes)
        {
            ReplaceTargetManIfIsPlayer(player);
        }

        private ValueTask PlayerLeftAfter((ITDSPlayer Player, int HadLifes) data)
        {
            ReplaceTargetManIfIsPlayer(data.Player);
            return default;
        }

        private void ReplaceTargetManIfIsPlayer(ITDSPlayer player)
        {
            if (player == _playerForcedAtTarget)
            {
                var nextTargetMan = GetNextTargetMan();
                SetTargetMan(nextTargetMan);
            }
        }

        private ITDSPlayer? GetNextTargetMan()
        {
            if (Lobby.Rounds.RoundStates.CurrentState is IInRoundState)
                return SharedUtils.GetRandom(_gamemode.Teams.Attacker.Players);

            if (_gamemode.MapHandler.TargetObject is null)
                return null;

            return _gamemode.Teams.Attacker.GetNearestPlayer(_gamemode.MapHandler.TargetObject.Position);
        }

        private void SetTargetMan(ITDSPlayer? player)
        {
            RemoveTargetMan();
            if (player is null)
                return;

            _playerForcedAtTarget = player;
            _gamemode.Teams.Attacker.FuncIterate(player =>
                player.SendNotification(string.Format(player.Language.TARGET_PLAYER_DEFEND_INFO, _playerForcedAtTarget.DisplayName)));

            var targetObjectPositionJson = Serializer.ToClient(_gamemode.MapHandler.TargetObject!.Position);

            NAPI.Task.Run(() =>
                _playerForcedAtTarget.TriggerEvent(ToClientEvent.SetForceStayAtPosition,
                    targetObjectPositionJson,
                    _settingsHandler.ServerSettings.GangwarTargetRadius,
                    MapLimitType.KillAfterTime,
                    _settingsHandler.ServerSettings.GangwarTargetWithoutAttackerMaxSeconds));
        }

        private void RemoveTargetMan()
        {
            if (_playerForcedAtTarget is null)
                return;

            NAPI.Task.Run(() =>
                _playerForcedAtTarget.TriggerEvent(ToClientEvent.RemoveForceStayAtPosition));

            _playerForcedAtTarget = null;
        }
    }
}
