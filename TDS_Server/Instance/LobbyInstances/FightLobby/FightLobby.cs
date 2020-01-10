using TDS_Server_DB.Entity.LobbyEntities;

namespace TDS_Server.Instance.LobbyInstances
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
