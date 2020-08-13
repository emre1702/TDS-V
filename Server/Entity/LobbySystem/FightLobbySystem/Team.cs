using System.Linq;
using TDS_Server.Data.Interfaces;

namespace TDS_Server.Entity.LobbySystem.FightLobbySystem
{
    partial class FightLobby
    {
        #region Private Methods

        /// <summary>
        /// </summary>
        /// <param name="start"></param>
        /// <param name="index"></param>
        /// <returns>
        /// Return the next team which is not spectator - if there is none, returns the spectator team
        /// </returns>
        private ITeam GetNextNonSpectatorTeam(ITeam start)
        {
            return GetNextNonSpectatorTeam(start.Entity.Index);
        }

        /// <summary>
        /// </summary>
        /// <param name="startindex"></param>
        /// <param name="index"></param>
        /// <returns>
        /// Return the next team which is not spectator - if there is none, returns the spectator team
        /// </returns>
        private ITeam GetNextNonSpectatorTeam(short startindex)
        {
            short startindextoiterate = startindex;
            do
            {
                if (++startindextoiterate == Teams.Count - 1)
                    startindextoiterate = 0;
            } while (Teams[startindex].IsSpectator && startindextoiterate != startindex);

            return Teams[startindextoiterate];
        }

        private ITeam? GetNextNonSpectatorTeamWithPlayers(ITeam? start)
        {
            return GetNextNonSpectatorTeamWithPlayers(start?.Entity.Index ?? 0);
        }

        private ITeam? GetNextNonSpectatorTeamWithPlayers(short startIndex)
        {
            if (Teams.Count <= 1)
                return null;

            int index = startIndex;
            do
            {
                if (++index >= Teams.Count - 1)
                    index = 0;
            } while (Teams[index].SpectateablePlayers?.Any() != true && index != startIndex);

            ITeam team = Teams[index];
            if (team.SpectateablePlayers?.Any() != true)
                return null;

            return team;
        }

        private ITeam? GetPreviousNonSpectatorTeamWithPlayers(ITeam? start)
        {
            return GetPreviousNonSpectatorTeamWithPlayers(start?.Entity.Index ?? 0);
        }

        private ITeam? GetPreviousNonSpectatorTeamWithPlayers(short startIndex)
        {
            if (Teams.Count <= 1)
                return null;

            int index = startIndex;
            do
            {
                if (--index < 0)
                    index = Teams.Count - 1;
            } while (Teams[index].SpectateablePlayers?.Any() != true && index != startIndex);

            ITeam team = Teams[index];
            if (team.SpectateablePlayers?.Any() != true)
                return null;

            return team;
        }

        #endregion Private Methods
    }
}
