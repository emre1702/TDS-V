using System.Collections.Generic;
using TDS.Entity;
using TDS.Instance.Lobby.Interfaces;
using TDS.Instance.Player;

namespace TDS.Instance.Lobby
{
    partial class FightLobby : Lobby, IFight
    {
        protected readonly Damagesys DmgSys;

        public FightLobby(Lobbies entity) : base(entity)
        {
            DmgSys = new Damagesys(entity.LobbyWeapons);
            SpectateablePlayers = new List<TDSPlayer>[entity.Teams.Count];
            AlivePlayers = new List<TDSPlayer>[entity.Teams.Count];
            foreach (Teams team in entity.Teams)
            {
                if (team.IsSpectatorTeam)
                    continue;
                SpectateablePlayers[team.Index] = new List<TDSPlayer>();
                AlivePlayers[team.Index] = new List<TDSPlayer>();
            }
        }
    }
}
