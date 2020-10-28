using GTANetworkAPI;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Data.Interfaces.LobbySystem.RoundsHandlers.Datas.RoundStates;
using TDS_Server.GamemodesSystem.Gamemodes;
using TDS_Server.Handler.Extensions;
using TDS_Shared.Core;
using TDS_Shared.Default;

namespace TDS_Server.GamemodesSystem.Rounds
{
    public class BombRounds : BaseGamemodeRounds
    {
        private readonly IRoundFightLobby _lobby;
        private readonly BombGamemode _gamemode;

        public BombRounds(IRoundFightLobby lobby, BombGamemode gamemode)
            => (_lobby, _gamemode) = (lobby, gamemode);

        internal override void AddEvents(IRoundFightLobbyEventsHandler events)
        {
            base.AddEvents(events);
            events.Countdown += Countdown;
            events.InRound += InRound;
            events.PlayerJoinedAfter += PlayerJoinedAfter;
        }

        internal override void RemoveEvents(IRoundFightLobbyEventsHandler events)
        {
            base.RemoveEvents(events);
            events.Countdown -= Countdown;
            if (events.InRound is { })
                events.InRound -= InRound;
            if (events.PlayerJoinedAfter is { })
                events.PlayerJoinedAfter -= PlayerJoinedAfter;
        }

        private void Countdown()
        {
            _gamemode.Specials.GiveBombToRandomTerrorist();
        }

        private async ValueTask InRound()
        {
            await _lobby.Players.DoInMain(player =>
             {
                 if (player.Team is null || player.Team.IsSpectator)
                     player.SendChatMessage(player.Language.ROUND_MISSION_BOMG_SPECTATOR);
                 else if (player.Team == _gamemode.Teams.Terrorists)
                     player.SendChatMessage(player.Language.ROUND_MISSION_BOMB_BAD);
                 else
                     player.SendChatMessage(player.Language.ROUND_MISSION_BOMB_GOOD);
             }).ConfigureAwait(false);

            await NAPI.Task.WaitForMainThread().ConfigureAwait(false);
            if (_gamemode.Players.BombAtPlayer is null)
                _gamemode.Specials.GiveBombToRandomTerrorist();
        }

        private ValueTask PlayerJoinedAfter((ITDSPlayer Player, int TeamIndex) data)
        {
            if (ShouldSendRoundInfo())
            {
                var bombPosJson = Serializer.ToClient(_gamemode.Specials.Bomb!.Position);
                var bombDetonateTimer = _gamemode.Specials.BombDetonateTimer!;
                var startAtMs = bombDetonateTimer.ExecuteAfterMs - bombDetonateTimer.RemainingMsToExecute;

                NAPI.Task.RunSafe(() =>
                   data.Player.TriggerEvent(ToClientEvent.BombPlanted, bombPosJson, false, startAtMs));
            }
            return default;
        }

        private bool ShouldSendRoundInfo()
        {
            if (!(_lobby.Rounds.RoundStates.CurrentState is IInRoundState))
                return false;

            if (_gamemode.Specials.BombDetonateTimer is null)
                return false;

            if (_gamemode.Specials.Bomb is null)
                return false;

            return true;
        }
    }
}
