using TDS_Server.Instance.Utility;

namespace TDS_Server.Instance.Lobby
{
    partial class FightLobby
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="start"></param>
        /// <param name="index"></param>
        /// <returns>Return the next team which is not spectator - if there is none, returns the spectator team</returns>
        private Team GetNextNonSpectatorTeam(Team start)
        {
            return GetNextNonSpectatorTeam(start.Entity.Index);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="startindex"></param>
        /// <param name="index"></param>
        /// <returns>Return the next team which is not spectator - if there is none, returns the spectator team</returns>
        private Team GetNextNonSpectatorTeam(short startindex)
        {
            short startindextoiterate = startindex;
            do
            {
                if (++startindextoiterate == Teams.Length - 1)
                    startindextoiterate = 0;
            } while (Teams[startindex].IsSpectator && startindextoiterate != startindex);

            return Teams[startindextoiterate];
        }

        private Team? GetNextNonSpectatorTeamWithPlayers(Team? start)
        {
            return GetNextNonSpectatorTeamWithPlayers(start?.Entity.Index ?? 0);
        }

        private Team? GetNextNonSpectatorTeamWithPlayers(short startindex)
        {
            short startindextoiterate = startindex;
            do
            {
                if (++startindextoiterate >= Teams.Length - 1)
                    startindextoiterate = 0;
            } while ((startindextoiterate == 0 || SpectateablePlayers[startindextoiterate - 1].Count == 0) && startindextoiterate != startindex);
            if (startindextoiterate == 0)
                startindextoiterate = 1;
            Team team = Teams[startindextoiterate];
            if (team.SpectateablePlayers == null)
                return null;

            return team.SpectateablePlayers.Count == 0 ? null : team;
        }

        private Team? GetPreviousNonSpectatorTeamWithPlayers(Team? start)
        {
            return GetPreviousNonSpectatorTeamWithPlayers(start?.Entity.Index ?? 0);
        }

        private Team? GetPreviousNonSpectatorTeamWithPlayers(short startindex)
        {
            int startindextoiterate = (int)startindex;
            do
            {
                if (--startindextoiterate < 0)
                    startindextoiterate = Teams.Length - 1;
            } while ((startindextoiterate == 0 || SpectateablePlayers[startindextoiterate - 1].Count == 0) && startindextoiterate != startindex);
            if (startindextoiterate == 0)
                startindextoiterate = Teams.Length - 1;
            Team team = Teams[startindextoiterate];
            if (team.SpectateablePlayers == null)
                return null;

            return team.SpectateablePlayers.Count == 0 ? null : team;
        }
    }
}