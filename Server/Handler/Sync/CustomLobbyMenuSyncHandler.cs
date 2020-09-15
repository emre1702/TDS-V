using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Defaults;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Models.CustomLobby;
using TDS_Server.Handler.Events;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums;
using TDS_Shared.Default;

namespace TDS_Server.Handler.Sync
{
    public class CustomLobbyMenuSyncHandler
    {
        private readonly LobbiesHandler _lobbiesHandler;
        private readonly List<ITDSPlayer> _playersInCustomLobbyMenu = new List<ITDSPlayer>();

        private readonly Serializer _serializer;

        public CustomLobbyMenuSyncHandler(EventsHandler eventsHandler, Serializer serializer, LobbiesHandler lobbiesHandler)
        {
            _serializer = serializer;
            _lobbiesHandler = lobbiesHandler;

            eventsHandler.PlayerLoggedOut += RemovePlayer;
            eventsHandler.PlayerJoinedCustomMenuLobby += AddPlayer;
            eventsHandler.PlayerLeftCustomMenuLobby += RemovePlayer;
            eventsHandler.CustomLobbyCreated += SyncLobbyAdded;
            eventsHandler.CustomLobbyRemoved += SyncLobbyRemoved;
        }

        public void AddPlayer(ITDSPlayer player)
        {
            _playersInCustomLobbyMenu.Add(player);
            List<CustomLobbyData> lobbyDatas = _lobbiesHandler.Lobbies.Where(l => !l.IsOfficial && l.Entity.Type != LobbyType.MapCreateLobby)
                                                        .Select(l => GetCustomLobbyData(l))
                                                        .ToList();

            player.TriggerEvent(ToClientEvent.ToBrowserEvent, ToBrowserEvent.SyncAllCustomLobbies, _serializer.ToBrowser(lobbyDatas));
        }

        public bool IsPlayerInCustomLobbyMenu(ITDSPlayer player)
        {
            return _playersInCustomLobbyMenu.Contains(player);
        }

        public void RemovePlayer(ITDSPlayer player)
        {
            _playersInCustomLobbyMenu.Remove(player);
        }

        public void SyncLobbyAdded(ILobby lobby)
        {
            if (lobby.IsOfficial || lobby.Entity.Type == LobbyType.MapCreateLobby)
                return;

            string json = _serializer.ToBrowser(GetCustomLobbyData(lobby));
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
        }

        public void SyncLobbyRemoved(ILobby lobby)
        {
            if (!lobby.IsOfficial && lobby.Entity.Type != LobbyType.MapCreateLobby)
            {
                for (int i = _playersInCustomLobbyMenu.Count - 1; i >= 0; --i)
                {
                    ITDSPlayer player = _playersInCustomLobbyMenu[i];
                    if (!player.LoggedIn)
                    {
                        _playersInCustomLobbyMenu.RemoveAt(i);
                        continue;
                    }
                    player.TriggerEvent(ToClientEvent.ToBrowserEvent, ToBrowserEvent.RemoveCustomLobby, lobby.Id);
                }
            }
        }

        private CustomLobbyData GetCustomLobbyData(ILobby lobby)
        {
            return new CustomLobbyData
            {
                AmountLifes = lobby.Entity.FightSettings?.AmountLifes ?? 1,

                LobbyId = lobby.Entity.Id,
                Name = lobby.Entity.Name,
                OwnerName = lobby.OwnerName,
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
    }
}
