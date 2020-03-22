using System;
using System.Collections.Generic;
using TDS_Server.Data.Interfaces;
using TDS_Server.Handler.Entities.TeamSystem;
using TDS_Shared.Default;

namespace TDS_Server.Handler.Entities.LobbySystem
{
    partial class Lobby
    {
        public HashSet<ITDSPlayer> Players { get; } = new HashSet<ITDSPlayer>();

        public void FuncIterateAllPlayers(Action<ITDSPlayer, ITeam?> func)
        {
            foreach (var player in Players)
            {
                func(player, player.Team);
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
                FuncIterateAllPlayers((character, teamID) =>
                {
                    character.SendNotification(texts[character.Language], flashing);
                });
            }
            else
            {
                targetTeam.FuncIterate((character, teamID) =>
                {
                    character.SendNotification(texts[character.Language], flashing);
                });
            }
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

        public void PlaySound(string soundName)
        {
            ModAPI.Sync.SendEvent(this, ToClientEvent.PlayCustomSound, soundName);
        }
    }
}
