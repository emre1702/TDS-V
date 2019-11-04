using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TDS_Server.Instance.Player;

namespace TDS_Server.Instance.Lobby
{
    partial class GangLobby
    {

        public async Task StartGangwar(TDSPlayer attacker, int gangwarAreaId)
        {
            var ownerGang = GetGangwarAreaOwner(gangwarAreaId);
            if (ownerGang == null)
                return;

            var lobby = new GangwarLobby(attacker, gangwarAreaId, attacker.Gang, ownerGang);
            await lobby.AddToDB();

            await lobby.AddPlayer(attacker, 1);

            lobby.StartPreparations();

        }
    }
}
