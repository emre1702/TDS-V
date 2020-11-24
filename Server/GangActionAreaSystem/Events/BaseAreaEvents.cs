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
        public event AttackerOwnerDelegate? Conquered;

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

        public void Remove()
        {
            if (CooldownStarted is { })
                foreach (var d in CooldownStarted.GetInvocationList())
                    CooldownStarted -= d as EmptyDelegate;

            if (CooldownEnded is { })
                foreach (var d in CooldownEnded.GetInvocationList())
                    CooldownEnded -= d as EmptyDelegate;

            if (AddedToLobby is { })
                foreach (var d in AddedToLobby.GetInvocationList())
                    AddedToLobby -= d as LobbyDelegate;

            if (Conquered is { })
                foreach (var d in Conquered.GetInvocationList())
                    Conquered -= d as AttackerOwnerDelegate;
        }
    }
}
