using System;
using System.Collections.Generic;
using System.Text;
using TDS.Entity;

namespace TDS.Instance.Lobby
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
                if (++startindextoiterate == teams.Length - 1)
                    startindextoiterate = 0;
            } while (teams[startindex].IsSpectatorTeam && startindextoiterate != startindex);

            return teams[startindextoiterate];
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
                if (++startindextoiterate == teams.Length - 1)
                    startindextoiterate = 0;
            } while ((teams[startindex].IsSpectatorTeam || AliveOrNotDisappearedPlayers[startindex].Count == 0) && startindextoiterate != startindex);
            Teams team = teams[startindextoiterate];

            return AliveOrNotDisappearedPlayers[team.Index].Count == 0 ? null : team;
        }
    }
}
