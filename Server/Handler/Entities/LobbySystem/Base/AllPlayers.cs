using GTANetworkAPI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;
using TDS_Shared.Data.Enums;
using TDS_Shared.Default;

namespace TDS_Server.Handler.Entities.LobbySystem
{
    partial class Lobby
    {
        public ConcurrentDictionary<int, ITDSPlayer> Players { get; } = new ConcurrentDictionary<int, ITDSPlayer>();

        public void FuncIterateAllPlayers(Action<ITDSPlayer, ITeam?> func)
        {
            foreach (var player in Players.Values)
            {
                func(player, player.Team);
            }
        }

        public void PlaySound(string soundName)
        {
            TriggerEvent(ToClientEvent.PlayCustomSound, soundName);
        }

        public void SendMessage(string msg, ITeam? targetTeam = null)
        {
            if (targetTeam is null)
            {
                FuncIterateAllPlayers((character, teamID) =>
                {
                    character.SendChatMessage(msg);
                });
            }
            else
            {
                targetTeam.FuncIterate((character, teamID) =>
                {
                    character.SendChatMessage(msg);
                });
            }
        }

        public void SendMessage(string msg, HashSet<int> blockingPlayerIds, ITeam? targetTeam = null)
        {
            if (targetTeam is null)
            {
                FuncIterateAllPlayers((character, teamID) =>
                {
                    if (blockingPlayerIds.Contains(character.Entity?.Id ?? 0))
                        return;
                    character.SendChatMessage(msg);
                });
            }
            else
            {
                targetTeam.FuncIterate((character, teamID) =>
                {
                    if (blockingPlayerIds.Contains(character.Entity?.Id ?? 0))
                        return;
                    character.SendChatMessage(msg);
                });
            }
        }

        public void SendMessage(Func<ILanguage, string> langgetter, ITeam? targetTeam = null)
        {
            Dictionary<ILanguage, string> texts = LangHelper.GetLangDictionary(langgetter);
            if (targetTeam is null)
                FuncIterateAllPlayers((player, team) =>
                {
                    player.SendChatMessage(texts[player.Language]);
                });
            else
                targetTeam.FuncIterate((player, team) =>
                {
                    player.SendChatMessage(texts[player.Language]);
                });
        }

        public void SendMessage(Dictionary<ILanguage, string> texts, ITeam? targetTeam = null)
        {
            if (targetTeam is null)
                FuncIterateAllPlayers((player, team) =>
                {
                    player.SendChatMessage(texts[player.Language]);
                });
            else
                targetTeam.FuncIterate((player, team) =>
                {
                    player.SendChatMessage(texts[player.Language]);
                });
        }

        public void SendNotification(Func<ILanguage, string> langgetter, ITeam? targetTeam = null, bool flashing = false)
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

        public void TriggerEvent(string eventName, params object[] args)
        {
            NAPI.ClientEvent.TriggerClientEventToPlayers(Players.Values.Cast<Player>().ToArray(), eventName, args);
        }

        public void SendNative(NativeHash nativeHash, params object[] args)
        {
            NAPI.Native.SendNativeToPlayersInDimension(Dimension, (Hash)nativeHash, args);
        }
    }
}
