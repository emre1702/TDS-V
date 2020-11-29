using GTANetworkAPI;
using System;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS.Server.Data.RoundEndReasons;
using TDS.Server.GamemodesSystem.Specials;
using TDS.Server.GangGamemodesSystem.Gamemodes;
using TDS.Server.Handler;
using TDS.Shared.Core;

namespace TDS.Server.GangGamemodesSystem.Specials
{
    public class GangGangwarSpecials : GangwarSpecials, IGangGangwarGamemodeSpecials
    {
        protected new IGangActionLobby Lobby => (IGangActionLobby)base.Lobby;

        private TDSTimer? _checkTargetTimer;
        private int _noOneInTargetSeconds = 0;

        public GangGangwarSpecials(IGangActionLobby lobby, IGangGangwarGamemode gamemode, ISettingsHandler settingsHandler)
            : base(lobby, gamemode, settingsHandler)
        {
        }

        public override void AddEvents(IRoundFightLobbyEventsHandler events)
        {
            base.AddEvents(events);
            events.RoundEnd += RoundEnd;
        }

        public override void RemoveEvents(IRoundFightLobbyEventsHandler events)
        {
            base.RemoveEvents(events);
            if (events.RoundEnd is { })
                events.RoundEnd -= RoundEnd;
        }

        private async void CheckIsSomeoneInTarget()
        {
            try
            {
                var someoneInTarget = await GetSomeoneIsInTarget();

                if (someoneInTarget)
                    SomeoneIsInTarget();
                else
                    NoOneIsInTarget();
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance.LogError(ex);
            }
        }

        private void SomeoneIsInTarget()
        {
            _noOneInTargetSeconds = 0;
        }

        private void NoOneIsInTarget()
        {
            ++_noOneInTargetSeconds;
            var secsLeft = SettingsHandler.ServerSettings.GangActionTargetWithoutAttackerMaxSeconds - _noOneInTargetSeconds;
            switch ((secsLeft, _noOneInTargetSeconds))
            {
                case (_, 1):
                case ( > 0 and <= 5, _):
                case (10, _):
                case (15, _):
                    Gamemode.Teams.Attacker.Chat.Send(lang => string.Format(lang.GANGWAR_TARGET_EMPTY_SECS_LEFT, secsLeft));
                    break;
                case (0, _):
                    _checkTargetTimer?.Kill();
                    Lobby.Rounds.RoundStates.EndRound(new TargetEmptyRoundEndReason(Lobby.Teams.Owner));
                    break;
            }
        }

        private async Task<bool> GetSomeoneIsInTarget()
        {
            return await Gamemode.Teams.Attacker.Players.DoListInMain(players =>
            {
                foreach (var player in players)
                {
                    if (IsInTarget(player))
                        return true;
                }
                return false;
            });
        }

        private bool IsInTarget(ITDSPlayer player)
        {
            return !player.Dead
                && player.Lifes > 0
                && player.LoggedIn
                && player.Lobby == Lobby
                && player.Position.DistanceTo(Gamemode.MapHandler.TargetObject!.Position) <= SettingsHandler.ServerSettings.GangActionTargetRadius;
        }

        public override ValueTask InRound()
        {
            // We're replacing "TargetMan" behaviour by not calling base.InRound. 

            _checkTargetTimer = new TDSTimer(CheckIsSomeoneInTarget, 1000, 0);
            return default;
        }

        private ValueTask RoundEnd()
        {
            _checkTargetTimer?.Kill();
            _checkTargetTimer = null;

            return default;
        }
    }
}
