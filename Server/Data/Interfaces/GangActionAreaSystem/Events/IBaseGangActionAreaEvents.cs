using TDS.Server.Data.Interfaces.GangsSystem;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies;

namespace TDS.Server.Data.Interfaces.GangActionAreaSystem.Events
{
#nullable enable
    public interface IBaseGangActionAreaEvents
    {
        delegate void EmptyDelegate();
        delegate void LobbyDelegate(IGangActionLobby lobby);
        delegate void AttackerMaybeOwnerDelegate(IGang attacker, IGang? owner);
        delegate void AttackerOwnerDelegate(IGang attacker, IGang owner);

        event EmptyDelegate? CooldownEnded;
        event EmptyDelegate? CooldownStarted;
        event LobbyDelegate? AddedToLobby;
        event LobbyDelegate? RemovedFromLobby;
        event AttackerMaybeOwnerDelegate? Conquered;
        event AttackerOwnerDelegate? Defended;

        void TriggerCooldownEnded();
        void TriggerCooldownStarted();
        void TriggerAddedToLobby(IGangActionLobby lobby);
        void TriggerRemovedFromLobby(IGangActionLobby lobby);
        void TriggerConquered(IGang newOwner, IGang? previousOwner);
        void TriggerDefended(IGang attacker, IGang owner);
    }
}