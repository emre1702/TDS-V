using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TDS_Server.Instance.GangTeam;
using TDS_Server.Instance.Player;

namespace TDS_Server.Instance.Lobby
{
    partial class GangLobby
    {

        public async Task StartGangwar(TDSPlayer attacker, int gangwarAreaId)
        {
            if (!CanStartGangwar(attacker))
                return;

            var ownerGang = GetGangwarAreaOwner(gangwarAreaId);
            if (ownerGang == null)
            {
                //Todo give the gang to the attacker gang
                return;
            }
                

            var lobby = new GangwarLobby(attacker, gangwarAreaId, attacker.Gang, ownerGang);
            await lobby.AddToDB();

            await lobby.AddPlayer(attacker, 1);

            lobby.StartPreparations(attacker);

        }

        private bool CanStartGangwar(TDSPlayer attacker)
        {
            if (attacker.Gang == Gang.None)
            {
                //todo You are not in a gang.
                return false;
            }

           

            return true;
        }
    }
}
