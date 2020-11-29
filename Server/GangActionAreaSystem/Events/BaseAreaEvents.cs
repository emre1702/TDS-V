using System;
using TDS.Server.Data.Interfaces.GangActionAreaSystem.Events;
using TDS.Server.Data.Interfaces.GangsSystem;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS.Server.Handler;
using static TDS.Server.Data.Interfaces.GangActionAreaSystem.Events.IBaseGangActionAreaEvents;

namespace TDS.Server.GangActionAreaSystem.Events
{
    internal class BaseAreaEvents : IBaseGangActionAreaEvents
    {
        public event EmptyDelegate? CooldownStarted;
        public event EmptyDelegate? CooldownEnded;
        public event LobbyDelegate? AddedToLobby;
        public event LobbyDelegate? RemovedFromLobby;
        public event AttackerMaybeOwnerDelegate? Conquered;
        public event AttackerOwnerDelegate? Defended;

        public void TriggerCooldownStarted()
        {
            try
            {
                CooldownStarted?.Invoke();
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance.LogError(ex);
            }
        }

        public void TriggerCooldownEnded()
        {
            try
            {
                CooldownEnded?.Invoke();
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance.LogError(ex);
            }
        }

        public void TriggerAddedToLobby(IGangActionLobby lobby)
        {
            try
            {
                AddedToLobby?.Invoke(lobby);
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance.LogError(ex);
            }
        }

        public void TriggerRemovedFromLobby(IGangActionLobby lobby)
        {
            try
            {
                RemovedFromLobby?.Invoke(lobby);
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance.LogError(ex);
            }
        }

        public void TriggerConquered(IGang newOwner, IGang? previousOwner)
        {
            try
            {
                Conquered?.Invoke(newOwner, previousOwner);
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance.LogError(ex);
            }
        }

        public void TriggerDefended(IGang attacker, IGang owner)
        {
            try
            {
                Defended?.Invoke(attacker, owner);
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance.LogError(ex);
            }
        }
    }
}
