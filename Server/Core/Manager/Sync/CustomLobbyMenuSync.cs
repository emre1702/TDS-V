﻿using System.Collections.Generic;
using System.Linq;
using TDS_Common.Default;
using TDS_Shared.Data.Enums;
using TDS_Common.Manager.Utility;
using TDS_Server.Dto.CustomLobby;
using TDS_Server.Instance.LobbyInstances;
using TDS_Server.Instance.PlayerInstance;
using TDS_Server.Manager.EventManager;
using TDS_Server.Manager.Utility;

namespace TDS_Server.Core.Manager.Sync
{
    static class CustomLobbyMenuSync
    {
        private static readonly List<TDSPlayer> _playerInCustomLobbyMenu = new List<TDSPlayer>();

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
                string json = Serializer.ToBrowser(GetCustomLobbyData(lobby));
                for (int i = _playerInCustomLobbyMenu.Count - 1; i >= 0; --i)
                {
                    TDSPlayer player = _playerInCustomLobbyMenu[i];
                    if (!player.LoggedIn)
                    {
                        _playerInCustomLobbyMenu.RemoveAt(i);
                        continue;
                    }
                    player.Player?.TriggerEvent(DToClientEvent.SyncNewCustomLobby, json);
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
                    player.Player?.TriggerEvent(DToClientEvent.RemoveCustomLobby, lobby.Id);
                }
            }
        }

        public static void AddPlayer(TDSPlayer player)
        {
            _playerInCustomLobbyMenu.Add(player);
            List<CustomLobbyData> lobbyDatas = LobbyManager.Lobbies.Where(l => !l.IsOfficial && l.LobbyEntity.Type != ELobbyType.MapCreateLobby)
                                                        .Select(l => GetCustomLobbyData(l))
                                                        .ToList();

            player.Player?.TriggerEvent(DToClientEvent.SyncAllCustomLobbies, Serializer.ToBrowser(lobbyDatas));
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
                AmountLifes = lobby.LobbyEntity.FightSettings?.AmountLifes ?? 1,

                LobbyId = lobby.LobbyEntity.Id,
                Name = lobby.LobbyEntity.Name,
                OwnerName = lobby.OwnerName,
                Password = lobby.LobbyEntity.Password,
                ShowRanking = lobby.LobbyEntity.LobbyRoundSettings.ShowRanking,
                SpawnAgainAfterDeathMs = lobby.LobbyEntity.FightSettings?.SpawnAgainAfterDeathMs ?? 400,
                StartArmor = lobby.LobbyEntity.FightSettings?.StartArmor ?? 100,
                StartHealth = lobby.LobbyEntity.FightSettings?.StartHealth ?? 100,

                RoundTime = lobby.LobbyEntity.LobbyRoundSettings.RoundTime,
                MixTeamsAfterRound = lobby.LobbyEntity.LobbyRoundSettings.MixTeamsAfterRound,
                CountdownTime = lobby.LobbyEntity.LobbyRoundSettings.CountdownTime,
                BombPlantTimeMs = lobby.LobbyEntity.LobbyRoundSettings.BombPlantTimeMs,
                BombDetonateTimeMs = lobby.LobbyEntity.LobbyRoundSettings.BombDetonateTimeMs,
                BombDefuseTimeMs = lobby.LobbyEntity.LobbyRoundSettings.BombDefuseTimeMs,

                MapLimitTime = lobby.LobbyEntity.LobbyMapSettings.MapLimitTime,
                MapLimitType = lobby.LobbyEntity.LobbyMapSettings.MapLimitType,

                Teams = lobby.LobbyEntity.Teams.Select(t => new CustomLobbyTeamData
                {
                    Name = t.Name,
                    Color = $"rgb({t.ColorR},{t.ColorG},{t.ColorB})",
                    BlipColor = t.BlipColor, 
                    SkinHash = t.SkinHash
                }).ToList(),

                Maps = lobby.LobbyEntity.LobbyMaps.Select(m => m.MapId).ToList(),
                Weapons = lobby.LobbyEntity.LobbyWeapons.Select(w => new CustomLobbyWeaponData 
                { 
                    WeaponHash = w.Hash,
                    Ammo = w.Ammo,
                    Damage = w.Damage,
                    HeadshotMultiplicator = w.HeadMultiplicator
                }).ToList()
                
            };
        }

    }
}
