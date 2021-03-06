﻿using GTANetworkAPI;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.GamemodesSystem.Gamemodes;
using TDS.Server.Data.Interfaces.GamemodesSystem.Specials;
using TDS.Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Data.Interfaces.LobbySystem.RoundsHandlers.Datas.RoundStates;
using TDS.Server.Handler.Extensions;
using TDS.Shared.Core;
using TDS.Shared.Data.Enums;
using TDS.Shared.Data.Utility;
using TDS.Shared.Default;

namespace TDS.Server.GamemodesSystem.Specials
{
    public class GangwarSpecials : BaseGamemodeSpecials, IGangwarGamemodeSpecials
    {
        private ITDSPlayer? _playerForcedAtTarget;

        protected IGangwarGamemode Gamemode { get; }
        protected ISettingsHandler SettingsHandler { get; }

        public GangwarSpecials(IRoundFightLobby lobby, IGangwarGamemode gamemode, ISettingsHandler settingsHandler) : base(lobby)
        {
            Gamemode = gamemode;
            SettingsHandler = settingsHandler;
        }

        public override void AddEvents(IRoundFightLobbyEventsHandler events)
        {
            base.AddEvents(events);

            events.PlayerDied += PlayerDied;
            events.PlayerLeftAfter += PlayerLeftAfter;
            events.InRound += InRound;
            events.RoundClear += RoundClear;
        }

        public override void RemoveEvents(IRoundFightLobbyEventsHandler events)
        {
            base.RemoveEvents(events);

            events.PlayerDied -= PlayerDied;
            if (events.PlayerLeftAfter is { })
                events.PlayerLeftAfter -= PlayerLeftAfter;
            if (events.InRound is { })
                events.InRound -= InRound;
            if (events.RoundClear is { })
                events.RoundClear -= RoundClear;
        }

        private async void PlayerDied(ITDSPlayer player, ITDSPlayer killer, uint weapon, int hadLifes)
        {
            await ReplaceTargetManIfIsPlayer(player);
        }

        private ValueTask PlayerLeftAfter((ITDSPlayer Player, int HadLifes) data)
        {
            return ReplaceTargetManIfIsPlayer(data.Player);
        }

        public virtual async ValueTask InRound()
        {
            if (Gamemode.MapHandler.TargetObject is null)
                return;

            await ReplaceTargetMan();
            if (_playerForcedAtTarget is { })
                _playerForcedAtTarget.Position = Gamemode.MapHandler.TargetObject.Position;
        }

        public virtual ValueTask RoundClear()
        {
            RemoveTargetMan();
            return default;
        }

        private async ValueTask ReplaceTargetManIfIsPlayer(ITDSPlayer player)
        {
            if (player == _playerForcedAtTarget)
                await ReplaceTargetMan();
        }

        private async Task ReplaceTargetMan()
        {
            var nextTargetMan = await GetNextTargetMan();
            SetTargetMan(nextTargetMan);
        }

        private async ValueTask<ITDSPlayer?> GetNextTargetMan()
        {
            if (Lobby.Rounds.RoundStates.CurrentState is IInRoundState)
                return Gamemode.Teams.Attacker.Players.GetRandom();

            if (Gamemode.MapHandler.TargetObject is null)
                return null;

            var player = await NAPI.Task.RunWait(() =>
            {
                return Gamemode.Teams.Attacker.Players.GetNearestPlayer(Gamemode.MapHandler.TargetObject.Position);
            });
            return player;
        }

        private void SetTargetMan(ITDSPlayer? player)
        {
            RemoveTargetMan();
            if (player is null)
                return;

            _playerForcedAtTarget = player;
            Gamemode.Teams.Attacker.Players.DoInMain(player =>
                player.SendNotification(string.Format(player.Language.TARGET_PLAYER_DEFEND_INFO, _playerForcedAtTarget.DisplayName)));

            var targetObjectPositionJson = Serializer.ToClient(Gamemode.MapHandler.TargetObject!.Position);

            NAPI.Task.RunSafe(() =>
                _playerForcedAtTarget.TriggerEvent(ToClientEvent.SetForceStayAtPosition,
                    targetObjectPositionJson,
                    SettingsHandler.ServerSettings.GangActionTargetRadius,
                    MapLimitType.KillAfterTime,
                    SettingsHandler.ServerSettings.GangActionTargetWithoutAttackerMaxSeconds));
        }

        private void RemoveTargetMan()
        {
            if (_playerForcedAtTarget is null)
                return;

            NAPI.Task.RunSafe(() =>
                _playerForcedAtTarget.TriggerEvent(ToClientEvent.RemoveForceStayAtPosition));

            _playerForcedAtTarget = null;
        }
    }
}