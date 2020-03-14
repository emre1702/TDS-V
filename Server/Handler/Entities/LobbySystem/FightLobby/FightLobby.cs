using TDS_Server.Database.Entity.LobbyEntities;

namespace TDS_Server.Handler.Entities.LobbySystem.FightLobby
{
    partial class FightLobby : Lobby
    {
        public readonly Damagesys DmgSys;

        public FightLobby(Lobbies entity, bool isGangActionLobby = false) : base(entity, isGangActionLobby)
        {
            DmgSys = new Damagesys(entity.LobbyWeapons, entity.LobbyKillingspreeRewards);
        }
    }
}
