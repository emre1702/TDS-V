using GTANetworkAPI;
using System.Collections.Generic;
using System.Linq;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Defaults;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Data.Models.CustomLobby;
using TDS.Server.Handler.Events;
using TDS.Server.Handler.Extensions;
using TDS.Shared.Core;
using TDS.Shared.Data.Enums;
using TDS.Shared.Default;

namespace TDS.Server.Handler.Sync
{
    public class CustomLobbyMenuSyncHandler
    {
        private readonly LobbiesHandler _lobbiesHandler;
        private readonly List<ITDSPlayer> _playersInCustomLobbyMenu = new List<ITDSPlayer>();

        public CustomLobbyMenuSyncHandler(EventsHandler eventsHandler, LobbiesHandler lobbiesHandler)
        {
            _lobbiesHandler = lobbiesHandler;

            eventsHandler.PlayerLoggedOut += RemovePlayer;
            eventsHandler.PlayerJoinedCustomMenuLobby += AddPlayer;
            eventsHandler.PlayerLeftCustomMenuLobby += RemovePlayer;
            eventsHandler.LobbyCreated += SyncLobbyAdded;
            eventsHandler.LobbyRemoved += SyncLobbyRemoved;
        }

        public void AddPlayer(ITDSPlayer player)
        {
            _playersInCustomLobbyMenu.Add(player);
            var lobbyDatas = _lobbiesHandler.Lobbies.Where(l => !l.IsOfficial && l.Entity.Type != LobbyType.MapCreateLobby)
                                                        .Select(l => GetCustomLobbyData(l))
                                                        .ToList();
            var lobbyDatasJson = Serializer.ToBrowser(lobbyDatas);
            NAPI.Task.RunSafe(() =>
                player.TriggerEvent(ToClientEvent.ToBrowserEvent, ToBrowserEvent.SyncAllCustomLobbies, lobbyDatasJson));
        }

        public bool IsPlayerInCustomLobbyMenu(ITDSPlayer player)
        {
            return _playersInCustomLobbyMenu.Contains(player);
        }

        public void RemovePlayer(ITDSPlayer player)
        {
            _playersInCustomLobbyMenu.Remove(player);
        }

        public void SyncLobbyAdded(IBaseLobby lobby)
        {
            if (!IsLobbyToSync(lobby))
                return;

            var json = Serializer.ToBrowser(GetCustomLobbyData(lobby));
            NAPI.Task.RunSafe(() =>
            {
                for (int i = _playersInCustomLobbyMenu.Count - 1; i >= 0; --i)
                {
                    ITDSPlayer player = _playersInCustomLobbyMenu[i];
                    if (!player.LoggedIn)
                    {
                        _playersInCustomLobbyMenu.RemoveAt(i);
                        continue;
                    }
                    player.TriggerEvent(ToClientEvent.ToBrowserEvent, ToBrowserEvent.AddCustomLobby, json);
                }
            });
        }

        public void SyncLobbyRemoved(IBaseLobby lobby)
        {
            if (!IsLobbyToSync(lobby))
                return;

            NAPI.Task.RunSafe(() =>
            {
                for (int i = _playersInCustomLobbyMenu.Count - 1; i >= 0; --i)
                {
                    var player = _playersInCustomLobbyMenu[i];
                    if (!player.LoggedIn)
                    {
                        _playersInCustomLobbyMenu.RemoveAt(i);
                        continue;
                    }
                    player.TriggerEvent(ToClientEvent.ToBrowserEvent, ToBrowserEvent.RemoveCustomLobby, lobby.Entity.Id);
                }
            });
        }

        private CustomLobbyData GetCustomLobbyData(IBaseLobby lobby)
        {
            return new CustomLobbyData
            {
                AmountLifes = lobby.Entity.FightSettings?.AmountLifes ?? 1,

                LobbyId = lobby.Entity.Id,
                Name = lobby.Entity.Name,
                OwnerName = lobby.Entity.Owner?.Name ?? "?",
                Password = lobby.Entity.Password,
                ShowRanking = lobby.Entity.LobbyRoundSettings.ShowRanking,
                SpawnAgainAfterDeathMs = lobby.Entity.FightSettings?.SpawnAgainAfterDeathMs ?? 400,
                StartArmor = lobby.Entity.FightSettings?.StartArmor ?? 100,
                StartHealth = lobby.Entity.FightSettings?.StartHealth ?? 100,

                RoundTime = lobby.Entity.LobbyRoundSettings.RoundTime,
                MixTeamsAfterRound = lobby.Entity.LobbyRoundSettings.MixTeamsAfterRound,
                CountdownTime = lobby.Entity.LobbyRoundSettings.CountdownTime,
                BombPlantTimeMs = lobby.Entity.LobbyRoundSettings.BombPlantTimeMs,
                BombDetonateTimeMs = lobby.Entity.LobbyRoundSettings.BombDetonateTimeMs,
                BombDefuseTimeMs = lobby.Entity.LobbyRoundSettings.BombDefuseTimeMs,

                MapLimitTime = lobby.Entity.LobbyMapSettings.MapLimitTime,
                MapLimitType = lobby.Entity.LobbyMapSettings.MapLimitType,

                Teams = lobby.Entity.Teams.Select(t => new CustomLobbyTeamData
                {
                    Name = t.Name,
                    Color = $"rgb({t.ColorR},{t.ColorG},{t.ColorB})",
                    BlipColor = t.BlipColor,
                    SkinHash = t.SkinHash
                }).ToList(),

                Maps = lobby.Entity.LobbyMaps.Select(m => m.MapId).ToList(),
                Weapons = lobby.Entity.LobbyWeapons.Select(w => new CustomLobbyWeaponData
                {
                    WeaponHash = w.Hash,
                    Ammo = w.Ammo,
                    Damage = w.Damage,
                    HeadshotMultiplicator = w.HeadMultiplicator
                }).ToList()
            };
        }

        private bool IsLobbyToSync(IBaseLobby lobby)
            => !(lobby.IsOfficial
                || lobby.Entity.Type == LobbyType.MapCreateLobby
                || lobby.Entity.Type == LobbyType.CharCreateLobby
                || lobby.Entity.Type == LobbyType.GangActionLobby
                || lobby.Entity.Type == LobbyType.DamageTestLobby);
    }
}
