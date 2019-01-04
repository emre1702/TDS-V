using System;
using System.Collections.Generic;
using System.Text;
using TDS_Server.Entity;

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
        private Teams GetNextNonSpectatorTeam(Teams start)
        {
            return GetNextNonSpectatorTeam(start.Index);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startindex"></param>
        /// <param name="index"></param>
        /// <returns>Return the next team which is not spectator - if there is none, returns the spectator team</returns>
        private Teams GetNextNonSpectatorTeam(uint startindex)
        {
            uint startindextoiterate = startindex;
            do
            {
                if (++startindextoiterate == Teams.Length - 1)
                    startindextoiterate = 0;
            } while (Teams[startindex].Index == 0 && startindextoiterate != startindex);

            return Teams[startindextoiterate];
        }

        private Teams GetNextNonSpectatorTeamWithPlayers(Teams start)
        {
            return GetNextNonSpectatorTeamWithPlayers(start.Index);
        }

        private Teams GetNextNonSpectatorTeamWithPlayers(uint startindex)
        {
            uint startindextoiterate = startindex;
            do
            {
                if (++startindextoiterate >= Teams.Length - 1)
                    startindextoiterate = 0;
            } while ((startindextoiterate == 0 || SpectateablePlayers[startindextoiterate - 1].Count == 0) && startindextoiterate != startindex);
            if (startindextoiterate == 0)
                startindextoiterate = 1;
            Teams team = Teams[startindextoiterate];

            return SpectateablePlayers[team.Index-1].Count == 0 ? null : team;
        }

        private Teams GetPreviousNonSpectatorTeamWithPlayers(Teams start)
        {
            return GetPreviousNonSpectatorTeamWithPlayers(start.Index);
        }

        private Teams GetPreviousNonSpectatorTeamWithPlayers(uint startindex)
        {
            int startindextoiterate = (int) startindex;
            do
            {
                if (--startindextoiterate < 0)
                    startindextoiterate = Teams.Length - 1;
            } while ((startindextoiterate == 0 || SpectateablePlayers[startindextoiterate - 1].Count == 0) && startindextoiterate != startindex);
            if (startindextoiterate == 0)
                startindextoiterate = Teams.Length - 1;
            Teams team = Teams[startindextoiterate];

            return SpectateablePlayers[team.Index-1].Count == 0 ? null : team;
        }
    }
}
