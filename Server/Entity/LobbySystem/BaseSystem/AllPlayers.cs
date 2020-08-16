using AltV.Net;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Entities;
using TDS_Shared.Data.Enums;
using TDS_Shared.Default;

namespace TDS_Server.Entity.LobbySystem.BaseSystem
{
    partial class Lobby
    {
        #region Public Properties

        public ConcurrentDictionary<int, ITDSPlayer> Players { get; } = new ConcurrentDictionary<int, ITDSPlayer>();

        #endregion Public Properties

        #region Public Methods

        public void FuncIterateAllPlayers(Action<ITDSPlayer, ITeam?> func)
        {
            foreach (var player in Players.Values)
            {
                func(player, player.Team);
            }
        }

        public void PlaySound(string soundName)
        {
            SendEvent(ToClientEvent.PlayCustomSound, soundName);
        }

        public void SendAllPlayerChatMessage(string msg, ITeam? targetTeam = null)
        {
            if (targetTeam is null)
            {
                FuncIterateAllPlayers((character, teamID) =>
                {
                    character.SendMessage(msg);
                });
            }
            else
            {
                targetTeam.FuncIterate((character, teamID) =>
                {
                    character.SendMessage(msg);
                });
            }
        }

        public void SendAllPlayerChatMessage(string msg, HashSet<int> blockingPlayerIds, ITeam? targetTeam = null)
        {
            if (targetTeam is null)
            {
                FuncIterateAllPlayers((character, teamID) =>
                {
                    if (blockingPlayerIds.Contains(character.Entity?.Id ?? 0))
                        return;
                    character.SendMessage(msg);
                });
            }
            else
            {
                targetTeam.FuncIterate((character, teamID) =>
                {
                    if (blockingPlayerIds.Contains(character.Entity?.Id ?? 0))
                        return;
                    character.SendMessage(msg);
                });
            }
        }

        public void SendAllPlayerLangMessage(Func<ILanguage, string> langgetter, ITeam? targetTeam = null)
        {
            Dictionary<ILanguage, string> texts = LangHelper.GetLangDictionary(langgetter);
            if (targetTeam is null)
                FuncIterateAllPlayers((player, team) =>
                {
                    player.SendMessage(texts[player.Language]);
                });
            else
                targetTeam.FuncIterate((player, team) =>
                {
                    player.SendMessage(texts[player.Language]);
                });
        }

        public void SendAllPlayerLangMessage(Dictionary<ILanguage, string> texts, ITeam? targetTeam = null)
        {
            if (targetTeam is null)
                FuncIterateAllPlayers((player, team) =>
                {
                    player.SendMessage(texts[player.Language]);
                });
            else
                targetTeam.FuncIterate((player, team) =>
                {
                    player.SendMessage(texts[player.Language]);
                });
        }

        public void SendAllPlayerLangNotification(Func<ILanguage, string> langgetter, ITeam? targetTeam = null, bool flashing = false)
        {
            Dictionary<ILanguage, string> texts = LangHelper.GetLangDictionary(langgetter);
            if (targetTeam is null)
            {
                FuncIterateAllPlayers((player, teamID) =>
                {
                    player.SendNotification(texts[player.Language], flashing);
                });
            }
            else
            {
                targetTeam.FuncIterate((player, teamID) =>
                {
                    player.SendNotification(texts[player.Language], flashing);
                });
            }
        }

        public void SendEvent(string eventName, params object[] args) 
        {
            FuncIterateAllPlayers((player, teamID) =>
            {
                player.Emit(eventName, args);
            });
        }

        public void SendNative(NativeHash native, params object[] args)
        {
            //Todo: Implement - and convert player at clientside to scriptId
        }

        #endregion Public Methods
    }
}
