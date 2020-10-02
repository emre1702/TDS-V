using System;
using System.Threading.Tasks;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS_Server.Data.RoundEndReasons;

namespace TDS_Server.LobbySystem.RoundsHandlers
{
    internal class GangActionLobbyRoundsHandler : RoundFightLobbyRoundsHandler
    {
        protected new IGangActionLobby Lobby => (IGangActionLobby)base.Lobby;

        public GangActionLobbyRoundsHandler(IGangActionLobby lobby, IServiceProvider serviceProvider, IRoundFightLobbyEventsHandler events)
            : base(lobby, serviceProvider, events)
        {
            lobby.Events.RoundClear += RoundClear;
        }

        public virtual async ValueTask RoundClear()
        {
            await Lobby.Remove();
        }

        public override Task CheckForEnoughAlive()
        {
            // if owner is alive only the target should be able to end the round
            // if attacker is alive we need to check if owner is alive or could still join

            if (Lobby.Teams.Owner.AlivePlayers!.Count > 0)
                return Task.CompletedTask;

            if (!OwnerCanJoin())
                Lobby.Rounds.RoundStates.EndRound(new DeathRoundEndReason(Lobby.Teams.Attacker));

            return Task.CompletedTask;
        }

        public override ValueTask<ITeam?> GetTimesUpWinnerTeam()
            => new ValueTask<ITeam?>(Lobby.Teams.Attacker);

        private bool OwnerCanJoin()
        {
            if (Lobby.Deathmatch.Damage.DamageDealtThisRound)
                return false;
            if (Lobby.Teams.HasTeamFreePlace(false))
                return false;
            return true;
        }
    }
}
