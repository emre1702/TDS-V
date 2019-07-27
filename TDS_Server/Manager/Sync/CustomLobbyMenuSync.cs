﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Common.Default;
using TDS_Common.Enum;
using TDS_Server.Dto;
using TDS_Server.Instance.Lobby;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Utility;

namespace TDS_Server.Manager.Sync
{
    static class CustomLobbyMenuSync
    {
        private static List<TDSPlayer> _playerInCustomLobbyMenu = new List<TDSPlayer>();

        static CustomLobbyMenuSync()
        {
            CustomEventManager.OnPlayerLoggedOut += (player) =>
            {
                RemovePlayer(player);
            };
        }

        public static void SyncLobbyAdded(Lobby lobby)
        {
            if (!lobby.IsOfficial && lobby.LobbyEntity.Type != ELobbyType.MapCreateLobby)
            {
                string json = JsonConvert.SerializeObject(GetCustomLobbyData(lobby));
                for (int i = _playerInCustomLobbyMenu.Count - 1; i >= 0; --i)
                {
                    TDSPlayer player = _playerInCustomLobbyMenu[i];
                    if (!player.LoggedIn)
                    {
                        _playerInCustomLobbyMenu.RemoveAt(i);
                        continue;
                    }
                    player.Client.TriggerEvent(DToClientEvent.SyncNewCustomLobby, json);
                }
            }
        }

        public static void SyncLobbyRemoved(Lobby lobby)
        {
            if (!lobby.IsOfficial && lobby.LobbyEntity.Type != ELobbyType.MapCreateLobby)
            {
                for (int i = _playerInCustomLobbyMenu.Count - 1; i >= 0; --i)
                {
                    TDSPlayer player = _playerInCustomLobbyMenu[i];
                    if (!player.LoggedIn)
                    {
                        _playerInCustomLobbyMenu.RemoveAt(i);
                        continue;
                    }
                    player.Client.TriggerEvent(DToClientEvent.RemoveCustomLobby, lobby.Id);
                }
            }
        }

        public static void AddPlayer(TDSPlayer player)
        {
            _playerInCustomLobbyMenu.Add(player);
            List<CustomLobbyData> lobbyDatas = LobbyManager.Lobbies.Where(l => !l.IsOfficial && l.LobbyEntity.Type != ELobbyType.MapCreateLobby)
                                                        .Select(l => GetCustomLobbyData(l))
                                                        .ToList();

            player.Client.TriggerEvent(DToClientEvent.SyncAllCustomLobbies, JsonConvert.SerializeObject(lobbyDatas));
        }

        public static void RemovePlayer(TDSPlayer player)
        {
            _playerInCustomLobbyMenu.Remove(player);
        }

        public static bool IsPlayerInCustomLobbyMenu(TDSPlayer player)
        {
            return _playerInCustomLobbyMenu.Contains(player);
        }

        private static CustomLobbyData GetCustomLobbyData(Lobby lobby)
        {
            return new CustomLobbyData
            {
                AmountLifes = lobby.LobbyEntity.AmountLifes ?? 1,

                LobbyId = lobby.LobbyEntity.Id,
                Name = lobby.LobbyEntity.Name,
                OwnerName = lobby.OwnerName,
                Password = lobby.LobbyEntity.Password,
                SpawnAgainAfterDeathMs = lobby.LobbyEntity.SpawnAgainAfterDeathMs,
                StartArmor = lobby.LobbyEntity.StartArmor,
                StartHealth = lobby.LobbyEntity.StartHealth,

                RoundTime = lobby.LobbyEntity.LobbyRoundSettings.RoundTime,
                MixTeamsAfterRound = lobby.LobbyEntity.LobbyRoundSettings.MixTeamsAfterRound,
                CountdownTime = lobby.LobbyEntity.LobbyRoundSettings.CountdownTime,
                BombPlantTimeMs = lobby.LobbyEntity.LobbyRoundSettings.BombPlantTimeMs,
                BombDetonateTimeMs = lobby.LobbyEntity.LobbyRoundSettings.BombDetonateTimeMs,
                BombDefuseTimeMs = lobby.LobbyEntity.LobbyRoundSettings.BombDefuseTimeMs,

                MapLimitTime = lobby.LobbyEntity.LobbyMapSettings.MapLimitTime,
                MapLimitType = lobby.LobbyEntity.LobbyMapSettings.MapLimitType
            };
        }

    }
}