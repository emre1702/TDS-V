using GTANetworkAPI;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Shared.Data.Enums;

namespace TDS.Server.Data.Interfaces
{
#nullable enable

    public interface IWorkaroundsHandler
    {
        void AttachEntityToEntity(Entity entity, Entity entityTarget, PedBone bone, Vector3 positionOffset, Vector3 rotationOffset, IBaseLobby? lobby = null);

        void DetachEntity(Entity entity);

        void FreezeEntity(Entity entity, bool freeze, IBaseLobby lobby);

        void FreezePlayer(Player player, bool freeze);

        void SetEntityCollisionless(Entity entity, bool collisionless, IBaseLobby? lobby = null);

        void SetEntityInvincible(IBaseLobby atLobby, Entity entity, bool invincible);

        void SetEntityInvincible(Player invincibleAtClient, Entity entity, bool invincible);

        void SetPlayerInvincible(Player player, bool invincible);

        void SetPlayerTeam(Player player, int team);
    }
}