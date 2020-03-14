using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Server.Handler.Entities.Player;
using TDS_Server.Handler.Entities.Utility;

namespace TDS_Server.Handler.Entities.LobbySystem.Base
{
    partial class Lobby
    {
        public HashSet<TDSPlayer> Players { get; } = new HashSet<TDSPlayer>();

        public void SendAllPlayerEvent(string eventname, Team? team, params object[] args)
        {
            if (team is null)
                _modAPI.
                NAPI.ClientEvent.TriggerClientEventToPlayers(Players.Select(p => p.Player).ToArray(), eventname, args);
            else
                NAPI.ClientEvent.TriggerClientEventToPlayers(Players.Where(p => p.Team == team).Select(p => p.Player).ToArray(), eventname, args);
        }

        public void FuncIterateAllPlayers(Action<TDSPlayer, Team?> func)
        {
            foreach (var player in Players)
            {
                func(player, player.Team);
            }
        }

        public void SendAllPlayerLangMessage(Func<ILanguage, string> langgetter, Team? targetTeam = null)
        {
            Dictionary<ILanguage, string> texts = LangUtils.GetLangDictionary(langgetter);
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

        public void SendAllPlayerLangMessage(Dictionary<ILanguage, string> texts, Team? targetTeam = null)
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

        public void SendAllPlayerLangNotification(Func<ILanguage, string> langgetter, Team? targetTeam = null, bool flashing = false)
        {
            Dictionary<ILanguage, string> texts = LangUtils.GetLangDictionary(langgetter);
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

        public void SendAllPlayerChatMessage(string msg, Team? targetTeam = null)
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

        public void SendAllPlayerChatMessage(string msg, HashSet<int> blockingPlayerIds, Team? targetTeam = null)
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
            FuncIterateAllPlayers((player, team) =>
                NAPI.ClientEvent.TriggerClientEvent(player.Player, DToClientEvent.PlayCustomSound, soundName)
            );
        }
    }
}
