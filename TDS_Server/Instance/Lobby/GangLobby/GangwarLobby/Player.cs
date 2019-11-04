using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TDS_Server.Instance.Player;

namespace TDS_Server.Instance.Lobby
{
    partial class GangwarLobby
    {
        public override Task<bool> AddPlayer(TDSPlayer player, uint? teamindex)
        {
            return base.AddPlayer(player, teamindex);
        }

        public override void RemovePlayer(TDSPlayer character)
        {
            base.RemovePlayer(character);

            
        }
    }
}
