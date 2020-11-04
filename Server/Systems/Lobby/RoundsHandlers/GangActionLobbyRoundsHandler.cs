using System;
using System.Threading.Tasks;
using TDS_Server.Data.Interfaces.GamemodesSystem;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS_Server.Data.Interfaces.TeamsSystem;
using TDS_Server.Data.RoundEndReasons;

namespace TDS_Server.LobbySystem.RoundsHandlers
{
    internal class GangActionLobbyRoundsHandler : RoundFightLobbyRoundsHandler
    {
        protected new IGangActionLobby Lobby => (IGangActionLobby)base.Lobby;

        public GangActionLobbyRoundsHandler(IGangActionLobby lobby, IRoundFightLobbyEventsHandler events, IGamemodesProvider gamemodesProvider)
            : base(lobby, events, gamemodesProvider)
        {
            lobby.Events.RoundClear += RoundClear;
        }

        public virtual async ValueTask RoundClear()
        {
            await Lobby.Remove().ConfigureAwait(false);
        }

        public override Task CheckForEnoughAlive()
        {
            // if owner is alive only the target should be able to end the round
            // if attacker is alive we need to check if owner is alive or could still join

            if (Lobby.Teams.Owner.Players.HasAnyAlive)
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
