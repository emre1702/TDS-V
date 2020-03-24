using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Models.CustomLobby;
using TDS_Server.Handler.Entities.LobbySystem;
using TDS_Server.Handler.Entities.Player;
using TDS_Server.Handler.Events;
using TDS_Shared.Data.Enums;
using TDS_Shared.Default;
using TDS_Shared.Manager.Utility;

namespace TDS_Server.Handler.Sync
{
    public class CustomLobbyMenuSyncHandler
    {
        private readonly List<ITDSPlayer> _playersInCustomLobbyMenu = new List<ITDSPlayer>();

        private readonly Serializer _serializer;
        private readonly LobbiesHandler _lobbiesHandler;

        public CustomLobbyMenuSyncHandler(EventsHandler eventsHandler, Serializer serializer, LobbiesHandler lobbiesHandler)
        {
            _serializer = serializer;
            _lobbiesHandler = lobbiesHandler;

            eventsHandler.PlayerLoggedOut += EventsHandler_PlayerLoggedOut;
        }

        private void EventsHandler_PlayerLoggedOut(ITDSPlayer player)
        {
            RemovePlayer(player);
        }

        public void SyncLobbyAdded(Lobby lobby)
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
                player.SendEvent(ToClientEvent.SyncNewCustomLobby, json);
            }
        }

        public void SyncLobbyRemoved(Lobby lobby)
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
                    player.SendEvent(ToClientEvent.RemoveCustomLobby, lobby.Id);
                }
            }
        }

        public void AddPlayer(ITDSPlayer player)
        {
            _playersInCustomLobbyMenu.Add(player);
            List<CustomLobbyData> lobbyDatas = _lobbiesHandler.Lobbies.Where(l => !l.IsOfficial && l.Entity.Type != LobbyType.MapCreateLobby)
                                                        .Select(l => GetCustomLobbyData(l))
                                                        .ToList();

            player.SendEvent(ToClientEvent.SyncAllCustomLobbies, _serializer.ToBrowser(lobbyDatas));
        }

        public void RemovePlayer(ITDSPlayer player)
        {
            _playersInCustomLobbyMenu.Remove(player);
        }

        public bool IsPlayerInCustomLobbyMenu(TDSPlayer player)
        {
            return _playersInCustomLobbyMenu.Contains(player);
        }

        private CustomLobbyData GetCustomLobbyData(Lobby lobby)
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
