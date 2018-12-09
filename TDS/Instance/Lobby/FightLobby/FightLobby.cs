using System.Collections.Generic;
using TDS.Entity;
using TDS.Instance.Lobby.Interfaces;
using TDS.Instance.Player;

namespace TDS.Instance.Lobby
{
    partial class FightLobby : Lobby, IFight
    {
        private readonly Damagesys damagesys;

        public FightLobby(Lobbies entity) : base(entity)
        {
            damagesys = new Damagesys(entity.LobbyWeapons);
            AliveOrNotDisappearedPlayers = new List<Character>[entity.Teams.Count];
            foreach (Teams team in entity.Teams)
            {
                AliveOrNotDisappearedPlayers[team.Index] = new List<Character>();
            }
        }
    }
}
