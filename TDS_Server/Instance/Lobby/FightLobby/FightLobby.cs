using System.Collections.Generic;
using TDS_Server.Entity;
using TDS_Server.Instance.Player;

namespace TDS_Server.Instance.Lobby
{
    partial class FightLobby : Lobby
    {
        protected readonly Damagesys DmgSys;

        public FightLobby(Lobbies entity) : base(entity)
        {
            DmgSys = new Damagesys(entity.LobbyWeapons);
            SpectateablePlayers = new List<TDSPlayer>[entity.Teams.Count-1];
            AlivePlayers = new List<TDSPlayer>[entity.Teams.Count-1];
            foreach (Teams team in entity.Teams)
            {
                if (team.Index == 0)
                    continue;
                SpectateablePlayers[team.Index-1] = new List<TDSPlayer>();
                AlivePlayers[team.Index-1] = new List<TDSPlayer>();
            }
        }
    }
}
