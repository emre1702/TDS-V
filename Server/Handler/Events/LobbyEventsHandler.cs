using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using System;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Entities;
using TDS_Server.Data.Interfaces.Entities.Gamemodes;
using TDS_Server.Data.Interfaces.Entities.LobbySystem;
using TDS_Server.Data.Interfaces.Handlers;
using TDS_Shared.Data.Enums;
using TDS_Shared.Default;

namespace TDS_Server.Handler.Events
{
    public class LobbyEventsHandler
    {
        #region Private Fields

        private readonly LobbiesHandler _lobbiesHandler;
        private readonly ILoggingHandler _loggingHandler;
        private readonly ITDSPlayerHandler _tdsPlayerHandler;

        #endregion Private Fields

        #region Public Constructors

        public LobbyEventsHandler(ILoggingHandler loggingHandler, LobbiesHandler lobbiesHandler, ITDSPlayerHandler tdsPlayerHandler)
        {
            _loggingHandler = loggingHandler;
            _lobbiesHandler = lobbiesHandler;
            _tdsPlayerHandler = tdsPlayerHandler;
 
            Alt.OnClient<ITDSPlayer, int>(ToServerEvent.ChooseTeam, OnChooseTeam);
            Alt.OnClient<ITDSPlayer, int>(ToServerEvent.GotHit, OnGotHit);
            Alt.OnClient<ITDSPlayer, bool>(ToServerEvent.SpectateNext, OnSpectateNext);
            Alt.OnClient<ITDSPlayer>(ToServerEvent.StartDefusing, OnStartDefusing);
            Alt.OnClient<ITDSPlayer>(ToServerEvent.StartPlanting, OnStartPlanting);
            Alt.OnClient<ITDSPlayer>(ToServerEvent.StopDefusing, OnStopDefusing);
            Alt.OnClient<ITDSPlayer>(ToServerEvent.StopPlanting, OnStopPlanting);
            Alt.OnClient<ITDSPlayer>(ToServerEvent.SuicideKill, OnSuicideKill);
            Alt.OnClient<ITDSPlayer>(ToServerEvent.SuicideShoot, OnSuicideShoot);
            Alt.OnClient<ITDSPlayer>(ToServerEvent.OutsideMapLimit, OnOutsideMapLimit);
            Alt.OnClient<ITDSPlayer>(ToServerEvent.MapsListRequest, OnMapsListRequest);
            Alt.OnClient<ITDSPlayer, int>(ToServerEvent.MapCreatorSyncRemoveTeamObjects, OnMapCreatorSyncRemoveTeamObjects);
            Alt.OnClient<ITDSPlayer, int>(ToServerEvent.MapCreatorSyncRemoveObject, OnMapCreatorSyncRemoveObject);
            AltAsync.OnClient<ITDSPlayer>(ToServerEvent.LeaveLobby, OnLeaveLobby);
            Alt.OnClient<ITDSPlayer>(ToServerEvent.MapCreatorStartNewMap, OnMapCreatorStartNewMap);
            Alt.OnClient<ITDSPlayer, int>(ToServerEvent.MapCreatorSyncLastId, OnMapCreatorSyncLastId);
            Alt.OnClient<ITDSPlayer, string>(ToServerEvent.MapCreatorSyncNewObject, OnMapCreatorSyncNewObject);
            Alt.OnClient<ITDSPlayer, string>(ToServerEvent.MapCreatorSyncObjectPosition, OnMapCreatorSyncObjectPosition);
            Alt.OnClient<ITDSPlayer, int>(ToServerEvent.SendTeamOrder, OnSendTeamOrder);

            Alt.OnWeaponDamage += OnWeaponDamage;
        }

        #endregion Public Constructors

        #region Public Methods

        public void OnChooseTeam(ITDSPlayer player, int index)
        {
            if (!player.LoggedIn)
                return;
            if (player.Lobby is null || !(player.Lobby is IArena arena))
                return;
            arena.ChooseTeam(player, index);
        }

        public void OnGotHit(ITDSPlayer player, int damage)
        {
            if (!player.LoggedIn)
                return;

            player.Damage(ref damage, out _);
        }

        private bool OnWeaponDamage(IPlayer modPlayer, IEntity targetEntity, uint weapon, ushort damage, Position shotOffset, BodyPart bodyPart)
        {
            var player = (ITDSPlayer)modPlayer;
            var weaponHash = (WeaponHash)weapon;

            if (!(targetEntity is ITDSPlayer target))
                return false;

            if (!(target.Lobby is IFightLobby fightLobby))
            {
                _loggingHandler.LogError(
                    string.Format("Attacker {0} dealt damage on body part {1} to {2} - but this player isn't in fightlobby.",
                        player.DisplayName, bodyPart, target.DisplayName),
                    Environment.StackTrace, null, player);
                return false;
            }

            fightLobby.DamagedPlayer(target, player, weaponHash, bodyPart);
            return true;
        }

        public async void OnLeaveLobby(ITDSPlayer player)
        {
            if (player.Lobby is null)
                return;

            await player.Lobby.RemovePlayer(player);
            await _lobbiesHandler.MainMenu.AddPlayer(player, 0);
        }

        public void OnMapCreatorStartNewMap(ITDSPlayer player)
        {
            if (!(player.Lobby is IMapCreateLobby lobby))
                return;
            lobby.StartNewMap();
        }

        public void OnMapCreatorSyncLastId(ITDSPlayer player, int id)
        {
            if (!(player.Lobby is IMapCreateLobby lobby))
                return;
            lobby.SyncLastId(player, id);
        }

        public void OnMapCreatorSyncNewObject(ITDSPlayer player, string json)
        {
            if (!(player.Lobby is IMapCreateLobby lobby))
                return;
            lobby.SyncNewObject(player, json);
        }

        public void OnMapCreatorSyncObjectPosition(ITDSPlayer player, string json)
        {
            if (!(player.Lobby is IMapCreateLobby lobby))
                return;
            lobby.SyncObjectPosition(player, json);
        }

        public void OnMapCreatorSyncRemoveObject(ITDSPlayer player, int id)
        {
            if (!(player.Lobby is IMapCreateLobby lobby))
                return;
            lobby.SyncRemoveObject(player, id);
        }

        public void OnMapCreatorSyncRemoveTeamObjects(ITDSPlayer player, int teamNumber)
        {
            if (!(player.Lobby is IMapCreateLobby lobby))
                return;
            lobby.SyncRemoveTeamObjects(player, teamNumber);
        }

        public void OnMapsListRequest(ITDSPlayer player)
        {
            if (!(player.Lobby is IArena arena))
                return;

            arena.SendMapsForVoting(player);
        }

        public void OnOutsideMapLimit(ITDSPlayer player)
        {
            if (!(player.Lobby is IArena arena))
                return;
            if (arena.Entity.LobbyMapSettings.MapLimitType == MapLimitType.KillAfterTime)
                player.Kill(player.Language.TOO_LONG_OUTSIDE_MAP);
        }

        public void OnSendTeamOrder(ITDSPlayer player, int teamOrderInt)
        {
            if (!Enum.IsDefined(typeof(TeamOrder), teamOrderInt))
                return;
            player.Lobby?.SendTeamOrder(player, (TeamOrder)teamOrderInt);
        }

        public void OnSpectateNext(ITDSPlayer player, bool forward)
        {
            if (!(player.Lobby is IFightLobby lobby))
                return;
            lobby.SpectateNext(player, forward);
        }

        public void OnStartDefusing(ITDSPlayer player)
        {
            if (!(player.Lobby is IArena arena))
                return;
            if (!(arena.CurrentGameMode is IBomb bombMode))
                return;
            if (!bombMode.StartBombDefusing(player))
                player.SendEvent(ToClientEvent.StopBombPlantDefuse);
        }

        public void OnStartPlanting(ITDSPlayer player)
        {
            if (!(player.Lobby is IArena arena))
                return;
            if (!(arena.CurrentGameMode is IBomb bombMode))
                return;
            if (!bombMode.StartBombPlanting(player))
                player.SendEvent(ToClientEvent.StopBombPlantDefuse);
        }

        public void OnStopDefusing(ITDSPlayer player)
        {
            if (!(player.Lobby is IArena arena))
                return;
            if (!(arena.CurrentGameMode is IBomb bombMode))
                return;
            bombMode.StopBombDefusing(player);
        }

        public void OnStopPlanting(ITDSPlayer player)
        {
            if (!(player.Lobby is IArena arena))
                return;
            if (!(arena.CurrentGameMode is IBomb bombMode))
                return;
            bombMode.StopBombPlanting(player);
        }

        public void OnSuicideKill(ITDSPlayer player)
        {
            if (player.Lifes == 0)
                return;
            if (!(player.Lobby is IFightLobby))
                return;
            player.Kill(player.Language.COMMITED_SUICIDE);
        }

        public void OnSuicideShoot(ITDSPlayer player)
        {
            if (player.Lifes == 0)
                return;
            if (!(player.Lobby is IFightLobby fightLobby))
                return;

            fightLobby.SendNative(NativeHash.SET_PED_SHOOTS_AT_COORD, player, 0f, 0f, 0f, false);
        }

        #endregion Public Methods
    }
}
