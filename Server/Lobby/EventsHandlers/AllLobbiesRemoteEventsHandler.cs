using GTANetworkAPI;
using System;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Extensions;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.GamemodesSystem.Gamemodes;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Handler;
using TDS_Server.LobbySystem.Players;
using TDS_Shared.Data.Enums;
using TDS_Shared.Default;

namespace TDS_Server.LobbySystem.EventsHandlers
{
    public class AllLobbiesRemoteEventsHandler
    {
        private readonly LobbiesHandler _lobbiesHandler;
        private readonly ILoggingHandler _loggingHandler;
        private readonly ITDSPlayerHandler _tdsPlayerHandler;

        public AllLobbiesRemoteEventsHandler(ILoggingHandler loggingHandler, LobbiesHandler lobbiesHandler, ITDSPlayerHandler tdsPlayerHandler)
        {
            _loggingHandler = loggingHandler;
            _lobbiesHandler = lobbiesHandler;
            _tdsPlayerHandler = tdsPlayerHandler;

            NAPI.ClientEvent.Register<ITDSPlayer, int>(ToServerEvent.ChooseTeam, this, OnChooseTeam);
            NAPI.ClientEvent.Register<ITDSPlayer, int>(ToServerEvent.GotHit, this, OnGotHit);
            NAPI.ClientEvent.Register<ITDSPlayer, int, long, int>(ToServerEvent.HitOtherPlayer, this, OnHitOtherPlayer);
            NAPI.ClientEvent.Register<ITDSPlayer, bool>(ToServerEvent.SpectateNext, this, OnSpectateNext);
            NAPI.ClientEvent.Register<ITDSPlayer>(ToServerEvent.StartDefusing, this, OnStartDefusing);
            NAPI.ClientEvent.Register<ITDSPlayer>(ToServerEvent.StartPlanting, this, OnStartPlanting);
            NAPI.ClientEvent.Register<ITDSPlayer>(ToServerEvent.StopDefusing, this, OnStopDefusing);
            NAPI.ClientEvent.Register<ITDSPlayer>(ToServerEvent.StopPlanting, this, OnStopPlanting);
            NAPI.ClientEvent.Register<ITDSPlayer>(ToServerEvent.SuicideKill, this, OnSuicideKill);
            NAPI.ClientEvent.Register<ITDSPlayer>(ToServerEvent.SuicideShoot, this, OnSuicideShoot);
            NAPI.ClientEvent.Register<ITDSPlayer>(ToServerEvent.OutsideMapLimit, this, OnOutsideMapLimit);
            NAPI.ClientEvent.Register<ITDSPlayer>(ToServerEvent.MapsListRequest, this, OnMapsListRequest);
            NAPI.ClientEvent.Register<ITDSPlayer, int>(ToServerEvent.MapCreatorSyncRemoveTeamObjects, this, OnMapCreatorSyncRemoveTeamObjects);
            NAPI.ClientEvent.Register<ITDSPlayer, int>(ToServerEvent.MapCreatorSyncRemoveObject, this, OnMapCreatorSyncRemoveObject);
            NAPI.ClientEvent.Register<ITDSPlayer>(ToServerEvent.LeaveLobby, this, OnLeaveLobby);
            NAPI.ClientEvent.Register<ITDSPlayer>(ToServerEvent.MapCreatorStartNewMap, this, OnMapCreatorStartNewMap);
            NAPI.ClientEvent.Register<ITDSPlayer, int>(ToServerEvent.MapCreatorSyncLastId, this, OnMapCreatorSyncLastId);
            NAPI.ClientEvent.Register<ITDSPlayer, string>(ToServerEvent.MapCreatorSyncNewObject, this, OnMapCreatorSyncNewObject);
            NAPI.ClientEvent.Register<ITDSPlayer, string>(ToServerEvent.MapCreatorSyncObjectPosition, this, OnMapCreatorSyncObjectPosition);
            NAPI.ClientEvent.Register<ITDSPlayer, int>(ToServerEvent.SendTeamOrder, this, OnSendTeamOrder);
        }

        public async void OnChooseTeam(ITDSPlayer player, int index)
        {
            if (!player.LoggedIn)
                return;
            if (!(player.Lobby?.Players is ArenaPlayers players))
                return;
            await players.ChooseTeam(player, index).ConfigureAwait(false);
        }

        public void OnGotHit(ITDSPlayer player, int damage)
        {
            if (!player.LoggedIn)
                return;

            player.HealthAndArmor.Remove(damage, out _, out _);
        }

        public void OnHitOtherPlayer(ITDSPlayer attacker, int targetRemoteId, long weaponHashLong, int bodyPartValue)
        {
            if (!attacker.LoggedIn)
                return;
            var target = _tdsPlayerHandler.GetIfLoggedIn((ushort)targetRemoteId);
            if (target is null)
                return;
            var weaponHash = (WeaponHash)weaponHashLong;
            var bodyPart = (PedBodyPart)bodyPartValue;

            if (!(target.Lobby is IFightLobby fightLobby))
            {
                _loggingHandler.LogError(
                    string.Format("Attacker {0} dealt damage on body part {1} to {2} - but this player isn't in fightlobby.",
                        attacker.DisplayName, bodyPart, target.DisplayName),
                    Environment.StackTrace, null, attacker);
                return;
            }

            fightLobby.Deathmatch.Damage.DamagePlayer(target, weaponHash, bodyPart, attacker);
        }

        public async void OnLeaveLobby(ITDSPlayer player)
        {
            if (!player.LoggedIn)
                return;
            if (player.Lobby is null)
                return;

            await player.Lobby.Players.RemovePlayer(player).ConfigureAwait(false);
            await _lobbiesHandler.MainMenu.Players.AddPlayer(player, 0).ConfigureAwait(false);
        }

        public void OnMapCreatorStartNewMap(ITDSPlayer player)
        {
            if (!player.LoggedIn)
                return;
            if (!(player.Lobby is IMapCreatorLobby lobby))
                return;
            lobby.Sync.StartNewMap();
        }

        public void OnMapCreatorSyncLastId(ITDSPlayer player, int id)
        {
            if (!player.LoggedIn)
                return;
            if (!(player.Lobby is IMapCreatorLobby lobby))
                return;
            lobby.Sync.SyncLastId(player, id);
        }

        public void OnMapCreatorSyncNewObject(ITDSPlayer player, string json)
        {
            if (!player.LoggedIn)
                return;
            if (!(player.Lobby is IMapCreatorLobby lobby))
                return;
            lobby.Sync.SyncNewObject(player, json);
        }

        public void OnMapCreatorSyncObjectPosition(ITDSPlayer player, string json)
        {
            if (!player.LoggedIn)
                return;
            if (!(player.Lobby is IMapCreatorLobby lobby))
                return;
            lobby.Sync.SyncObjectPosition(player, json);
        }

        public void OnMapCreatorSyncRemoveObject(ITDSPlayer player, int id)
        {
            if (!player.LoggedIn)
                return;
            if (!(player.Lobby is IMapCreatorLobby lobby))
                return;
            lobby.Sync.SyncRemoveObject(player, id);
        }

        public void OnMapCreatorSyncRemoveTeamObjects(ITDSPlayer player, int teamNumber)
        {
            if (!player.LoggedIn)
                return;
            if (!(player.Lobby is IMapCreatorLobby lobby))
                return;
            lobby.Sync.SyncRemoveTeamObjects(player, teamNumber);
        }

        public void OnMapsListRequest(ITDSPlayer player)
        {
            if (!player.LoggedIn)
                return;
            if (!(player.Lobby is IArena arena))
                return;

            var mapsJson = arena.MapHandler.GetMapsJson();
            NAPI.Task.Run(() => player.TriggerEvent(ToClientEvent.MapsListRequest, mapsJson));
        }

        public void OnOutsideMapLimit(ITDSPlayer player)
        {
            if (!player.LoggedIn)
                return;
            if (!(player.Lobby is IArena arena))
                return;
            if (arena.Entity.LobbyMapSettings.MapLimitType == MapLimitType.KillAfterTime)
                arena.Deathmatch.Kill(player, player.Language.TOO_LONG_OUTSIDE_MAP);
        }

        public void OnSendTeamOrder(ITDSPlayer player, int teamOrderInt)
        {
            if (!player.LoggedIn)
                return;
            if (!(player.Lobby is IFightLobby fightLobby))
                return;
            if (!Enum.IsDefined(typeof(TeamOrder), teamOrderInt))
                return;
            fightLobby.Teams.SendTeamOrder(player, (TeamOrder)teamOrderInt);
        }

        public void OnSpectateNext(ITDSPlayer player, bool forward)
        {
            if (!player.LoggedIn)
                return;
            if (!(player.Lobby is IFightLobby lobby))
                return;
            lobby.Spectator.SpectateNext(player, forward);
        }

        public void OnStartDefusing(ITDSPlayer player)
        {
            if (!player.LoggedIn)
                return;
            if (!(player.Lobby is IRoundFightLobby roundFightLobby))
                return;
            if (!(roundFightLobby.Rounds.CurrentGamemode is IBombGamemode bombMode))
                return;
            if (!bombMode.Specials.StartBombDefusing(player))
                player.TriggerEvent(ToClientEvent.StopBombPlantDefuse);
        }

        public void OnStartPlanting(ITDSPlayer player)
        {
            if (!player.LoggedIn)
                return;
            if (!(player.Lobby is IRoundFightLobby roundFightLobby))
                return;
            if (!(roundFightLobby.Rounds.CurrentGamemode is IBombGamemode bombMode))
                return;
            if (!bombMode.Specials.StartBombPlanting(player))
                player.TriggerEvent(ToClientEvent.StopBombPlantDefuse);
        }

        public void OnStopDefusing(ITDSPlayer player)
        {
            if (!player.LoggedIn)
                return;
            if (!(player.Lobby is IRoundFightLobby roundFightLobby))
                return;
            if (!(roundFightLobby.Rounds.CurrentGamemode is IBombGamemode bombMode))
                return;
            bombMode.Specials.StopBombDefusing(player);
        }

        public void OnStopPlanting(ITDSPlayer player)
        {
            if (!player.LoggedIn)
                return;
            if (!(player.Lobby is IRoundFightLobby roundFightLobby))
                return;
            if (!(roundFightLobby.Rounds.CurrentGamemode is IBombGamemode bombMode))
                return;
            bombMode.Specials.StopBombPlanting(player);
        }

        public void OnSuicideKill(ITDSPlayer player)
        {
            if (!player.LoggedIn)
                return;
            if (player.Lifes == 0)
                return;
            if (!(player.Lobby is IFightLobby lobby))
                return;
            lobby.Deathmatch.Kill(player, player.Language.COMMITED_SUICIDE);
        }

        public void OnSuicideShoot(ITDSPlayer player)
        {
            if (!player.LoggedIn)
                return;
            if (player.Lifes == 0)
                return;
            if (!(player.Lobby is IFightLobby fightLobby))
                return;

            fightLobby.Natives.Send(NativeHash.SET_PED_SHOOTS_AT_COORD, player.RemoteId, 0f, 0f, 0f, false);
        }
    }
}
