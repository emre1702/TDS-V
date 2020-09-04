using GTANetworkAPI;
using System;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Extensions;
using TDS_Server.Data.Interfaces;
using TDS_Server.Handler.Entities.Gamemodes;
using TDS_Server.Handler.Entities.LobbySystem;
using TDS_Shared.Data.Enums;
using TDS_Shared.Default;

namespace TDS_Server.Handler.Events
{
    public class LobbyEventsHandler
    {
        private readonly LobbiesHandler _lobbiesHandler;
        private readonly ILoggingHandler _loggingHandler;
        private readonly ITDSPlayerHandler _tdsPlayerHandler;

        #region Public Constructors

        public LobbyEventsHandler(ILoggingHandler loggingHandler, LobbiesHandler lobbiesHandler, ITDSPlayerHandler tdsPlayerHandler)
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

        #endregion Public Constructors

        #region Public Methods

        public void OnChooseTeam(ITDSPlayer player, int index)
        {
            if (!player.LoggedIn)
                return;
            if (player.Lobby is null || !(player.Lobby is Arena arena))
                return;
            arena.ChooseTeam(player, index);
        }

        public void OnGotHit(ITDSPlayer player, int damage)
        {
            if (!player.LoggedIn)
                return;

            player.Damage(ref damage, out _);
        }

        public void OnHitOtherPlayer(ITDSPlayer attacker, int targetRemoteId, long weaponHashLong, int bodyPartValue)
        {
            if (!attacker.LoggedIn)
                return;
            var target = _tdsPlayerHandler.GetIfLoggedIn((ushort)targetRemoteId);
            if (target is null)
                return;
            WeaponHash weaponHash = (WeaponHash)weaponHashLong;
            PedBodyPart bodyPart = (PedBodyPart)bodyPartValue;

            if (!(target.Lobby is FightLobby fightLobby))
            {
                _loggingHandler.LogError(
                    string.Format("Attacker {0} dealt damage on body part {1} to {2} - but this player isn't in fightlobby.",
                        attacker.DisplayName, bodyPart, target.DisplayName),
                    Environment.StackTrace, null, attacker);
                return;
            }

            fightLobby.DamagedPlayer(target, attacker, weaponHash, bodyPart);
        }

        public async void OnLeaveLobby(ITDSPlayer player)
        {
            if (!player.LoggedIn)
                return;
            if (player.Lobby is null)
                return;

            await player.Lobby.RemovePlayer(player);
            await _lobbiesHandler.MainMenu.AddPlayer(player, 0);
        }

        public void OnMapCreatorStartNewMap(ITDSPlayer player)
        {
            if (!player.LoggedIn)
                return;
            if (!(player.Lobby is MapCreateLobby lobby))
                return;
            lobby.StartNewMap();
        }

        public void OnMapCreatorSyncLastId(ITDSPlayer player, int id)
        {
            if (!player.LoggedIn)
                return;
            if (!(player.Lobby is MapCreateLobby lobby))
                return;
            lobby.SyncLastId(player, id);
        }

        public void OnMapCreatorSyncNewObject(ITDSPlayer player, string json)
        {
            if (!player.LoggedIn)
                return;
            if (!(player.Lobby is MapCreateLobby lobby))
                return;
            lobby.SyncNewObject(player, json);
        }

        public void OnMapCreatorSyncObjectPosition(ITDSPlayer player, string json)
        {
            if (!player.LoggedIn)
                return;
            if (!(player.Lobby is MapCreateLobby lobby))
                return;
            lobby.SyncObjectPosition(player, json);
        }

        public void OnMapCreatorSyncRemoveObject(ITDSPlayer player, int id)
        {
            if (!player.LoggedIn)
                return;
            if (!(player.Lobby is MapCreateLobby lobby))
                return;
            lobby.SyncRemoveObject(player, id);
        }

        public void OnMapCreatorSyncRemoveTeamObjects(ITDSPlayer player, int teamNumber)
        {
            if (!player.LoggedIn)
                return;
            if (!(player.Lobby is MapCreateLobby lobby))
                return;
            lobby.SyncRemoveTeamObjects(player, teamNumber);
        }

        public void OnMapsListRequest(ITDSPlayer player)
        {
            if (!player.LoggedIn)
                return;
            if (!(player.Lobby is Arena arena))
                return;

            arena.SendMapsForVoting(player);
        }

        public void OnOutsideMapLimit(ITDSPlayer player)
        {
            if (!player.LoggedIn)
                return;
            if (!(player.Lobby is Arena arena))
                return;
            if (arena.Entity.LobbyMapSettings.MapLimitType == MapLimitType.KillAfterTime)
                FightLobby.KillPlayer(player, player.Language.TOO_LONG_OUTSIDE_MAP);
        }

        public void OnSendTeamOrder(ITDSPlayer player, int teamOrderInt)
        {
            if (!player.LoggedIn)
                return;
            if (!Enum.IsDefined(typeof(TeamOrder), teamOrderInt))
                return;
            player.Lobby?.SendTeamOrder(player, (TeamOrder)teamOrderInt);
        }

        public void OnSpectateNext(ITDSPlayer player, bool forward)
        {
            if (!player.LoggedIn)
                return;
            if (!(player.Lobby is FightLobby lobby))
                return;
            lobby.SpectateNext(player, forward);
        }

        public void OnStartDefusing(ITDSPlayer player)
        {
            if (!player.LoggedIn)
                return;
            if (!(player.Lobby is Arena arena))
                return;
            if (!(arena.CurrentGameMode is Bomb bombMode))
                return;
            if (!bombMode.StartBombDefusing(player))
                player.TriggerEvent(ToClientEvent.StopBombPlantDefuse);
        }

        public void OnStartPlanting(ITDSPlayer player)
        {
            if (!player.LoggedIn)
                return;
            if (!(player.Lobby is Arena arena))
                return;
            if (!(arena.CurrentGameMode is Bomb bombMode))
                return;
            if (!bombMode.StartBombPlanting(player))
                player.TriggerEvent(ToClientEvent.StopBombPlantDefuse);
        }

        public void OnStopDefusing(ITDSPlayer player)
        {
            if (!player.LoggedIn)
                return;
            if (!(player.Lobby is Arena arena))
                return;
            if (!(arena.CurrentGameMode is Bomb bombMode))
                return;
            bombMode.StopBombDefusing(player);
        }

        public void OnStopPlanting(ITDSPlayer player)
        {
            if (!player.LoggedIn)
                return;
            if (!(player.Lobby is Arena arena))
                return;
            if (!(arena.CurrentGameMode is Bomb bombMode))
                return;
            bombMode.StopBombPlanting(player);
        }

        public void OnSuicideKill(ITDSPlayer player)
        {
            if (!player.LoggedIn)
                return;
            if (player.Lifes == 0)
                return;
            if (!(player.Lobby is FightLobby))
                return;
            FightLobby.KillPlayer(player, player.Language.COMMITED_SUICIDE);
        }

        public void OnSuicideShoot(ITDSPlayer player)
        {
            if (!player.LoggedIn)
                return;
            if (player.Lifes == 0)
                return;
            if (!(player.Lobby is FightLobby fightLobby))
                return;

            fightLobby.SendNative(NativeHash.SET_PED_SHOOTS_AT_COORD, player.RemoteId, 0f, 0f, 0f, false);
        }

        #endregion Public Methods
    }
}
