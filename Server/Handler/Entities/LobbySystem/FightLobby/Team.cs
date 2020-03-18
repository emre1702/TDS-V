using TDS_Server.Data.Interfaces;

namespace TDS_Server.Handler.Entities.LobbySystem
{
    partial class FightLobby
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="start"></param>
        /// <param name="index"></param>
        /// <returns>Return the next team which is not spectator - if there is none, returns the spectator team</returns>
        private ITeam GetNextNonSpectatorTeam(ITeam start)
        {
            return GetNextNonSpectatorTeam(start.Entity.Index);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="startindex"></param>
        /// <param name="index"></param>
        /// <returns>Return the next team which is not spectator - if there is none, returns the spectator team</returns>
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

        private ITeam? GetNextNonSpectatorTeamWithPlayers(short startindex)
        {
            short startindextoiterate = startindex;
            do
            {
                if (++startindextoiterate >= Teams.Count - 1)
                    startindextoiterate = 0;
            } while ((startindextoiterate == 0 || Teams[startindextoiterate - 1].SpectateablePlayers?.Count == 0) && startindextoiterate != startindex);
            if (startindextoiterate == 0)
                startindextoiterate = 1;
            ITeam team = Teams[startindextoiterate];
            if (team.SpectateablePlayers is null)
                return null;

            return team.SpectateablePlayers.Count == 0 ? null : team;
        }

        private ITeam? GetPreviousNonSpectatorTeamWithPlayers(ITeam? start)
        {
            return GetPreviousNonSpectatorTeamWithPlayers(start?.Entity.Index ?? 0);
        }

        private ITeam? GetPreviousNonSpectatorTeamWithPlayers(short startindex)
        {
            int startindextoiterate = (int)startindex;
            do
            {
                if (--startindextoiterate < 0)
                    startindextoiterate = Teams.Count - 1;
            } while ((startindextoiterate == 0 || Teams[startindextoiterate].SpectateablePlayers?.Count == 0) && startindextoiterate != startindex);
            if (startindextoiterate == 0)
                startindextoiterate = Teams.Count - 1;
            ITeam team = Teams[startindextoiterate];
            if (team.SpectateablePlayers is null)
                return null;

            return team.SpectateablePlayers.Count == 0 ? null : team;
        }
    }
}
