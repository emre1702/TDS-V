using System;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Data.Interfaces.ModAPI.Player;
using TDS_Server.Handler.Entities.Gamemodes;
using TDS_Server.Handler.Entities.LobbySystem;
using TDS_Server.Handler.Player;
using TDS_Shared.Data.Enums;
using TDS_Shared.Default;

namespace TDS_Server.Handler.Events
{
    public class LobbyEventsHandler
    {
        #region Private Fields

        private readonly LobbiesHandler _lobbiesHandler;
        private readonly ILoggingHandler _loggingHandler;
        private readonly IModAPI _modAPI;
        private readonly TDSPlayerHandler _tdsPlayerHandler;

        #endregion Private Fields

        #region Public Constructors

        public LobbyEventsHandler(IModAPI modAPI, ILoggingHandler loggingHandler, LobbiesHandler lobbiesHandler, TDSPlayerHandler tdsPlayerHandler)
        {
            _modAPI = modAPI;
            _loggingHandler = loggingHandler;
            _lobbiesHandler = lobbiesHandler;
            _tdsPlayerHandler = tdsPlayerHandler;

            modAPI.ClientEvent.Add<IPlayer, int>(ToServerEvent.ChooseTeam, this, OnChooseTeam);
            modAPI.ClientEvent.Add<IPlayer, int>(ToServerEvent.GotHit, this, OnGotHit);
            modAPI.ClientEvent.Add<IPlayer, int, long, int>(ToServerEvent.HitOtherPlayer, this, OnHitOtherPlayer);
            modAPI.ClientEvent.Add<IPlayer, bool>(ToServerEvent.SpectateNext, this, OnSpectateNext);
            modAPI.ClientEvent.Add(ToServerEvent.StartDefusing, this, OnStartDefusing);
            modAPI.ClientEvent.Add(ToServerEvent.StartPlanting, this, OnStartPlanting);
            modAPI.ClientEvent.Add(ToServerEvent.StopDefusing, this, OnStopDefusing);
            modAPI.ClientEvent.Add(ToServerEvent.StopPlanting, this, OnStopPlanting);
            modAPI.ClientEvent.Add(ToServerEvent.SuicideKill, this, OnSuicideKill);
            modAPI.ClientEvent.Add(ToServerEvent.SuicideShoot, this, OnSuicideShoot);
            modAPI.ClientEvent.Add(ToServerEvent.OutsideMapLimit, this, OnOutsideMapLimit);
            modAPI.ClientEvent.Add(ToServerEvent.MapsListRequest, this, OnMapsListRequest);
            modAPI.ClientEvent.Add<IPlayer, int>(ToServerEvent.MapCreatorSyncRemoveTeamObjects, this, OnMapCreatorSyncRemoveTeamObjects);
            modAPI.ClientEvent.Add<IPlayer, int>(ToServerEvent.MapCreatorSyncRemoveObject, this, OnMapCreatorSyncRemoveObject);
            modAPI.ClientEvent.Add(ToServerEvent.LeaveLobby, this, OnLeaveLobby);
            modAPI.ClientEvent.Add(ToServerEvent.MapCreatorStartNewMap, this, OnMapCreatorStartNewMap);
            modAPI.ClientEvent.Add<IPlayer, int>(ToServerEvent.MapCreatorSyncLastId, this, OnMapCreatorSyncLastId);
            modAPI.ClientEvent.Add<IPlayer, string>(ToServerEvent.MapCreatorSyncNewObject, this, OnMapCreatorSyncNewObject);
            modAPI.ClientEvent.Add<IPlayer, string>(ToServerEvent.MapCreatorSyncObjectPosition, this, OnMapCreatorSyncObjectPosition);
            modAPI.ClientEvent.Add<IPlayer, int>(ToServerEvent.SendTeamOrder, this, OnSendTeamOrder);
        }

        #endregion Public Constructors

        #region Public Methods

        public void OnChooseTeam(IPlayer modPlayer, int index)
        {
            var player = _tdsPlayerHandler.GetIfLoggedIn(modPlayer);
            if (player is null)
                return;
            if (player.Lobby is null || !(player.Lobby is Arena arena))
                return;
            arena.ChooseTeam(player, index);
        }

        public void OnGotHit(IPlayer modPlayer, int damage)
        {
            var player = _tdsPlayerHandler.GetIfLoggedIn(modPlayer);
            if (player is null)
                return;

            player.Damage(ref damage, out _);
        }

        public void OnHitOtherPlayer(IPlayer attackerModPlayer, int targetRemoteId, long weaponHashLong, int bodyPartValue)
        {
            var attacker = _tdsPlayerHandler.GetIfLoggedIn(attackerModPlayer);
            if (attacker is null)
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

        public async void OnLeaveLobby(IPlayer modPlayer)
        {
            var player = _tdsPlayerHandler.GetIfLoggedIn(modPlayer);
            if (player is null)
                return;
            if (player.Lobby is null)
                return;

            await player.Lobby.RemovePlayer(player);
            await _lobbiesHandler.MainMenu.AddPlayer(player, 0);
        }

        public void OnMapCreatorStartNewMap(IPlayer modPlayer)
        {
            var player = _tdsPlayerHandler.GetIfLoggedIn(modPlayer);
            if (player is null)
                return;
            if (!(player.Lobby is MapCreateLobby lobby))
                return;
            lobby.StartNewMap();
        }

        public void OnMapCreatorSyncLastId(IPlayer modPlayer, int id)
        {
            var player = _tdsPlayerHandler.GetIfLoggedIn(modPlayer);
            if (player is null)
                return;
            if (!(player.Lobby is MapCreateLobby lobby))
                return;
            lobby.SyncLastId(player, id);
        }

        public void OnMapCreatorSyncNewObject(IPlayer modPlayer, string json)
        {
            var player = _tdsPlayerHandler.GetIfLoggedIn(modPlayer);
            if (player is null)
                return;
            if (!(player.Lobby is MapCreateLobby lobby))
                return;
            lobby.SyncNewObject(player, json);
        }

        public void OnMapCreatorSyncObjectPosition(IPlayer modPlayer, string json)
        {
            var player = _tdsPlayerHandler.GetIfLoggedIn(modPlayer);
            if (player is null)
                return;
            if (!(player.Lobby is MapCreateLobby lobby))
                return;
            lobby.SyncObjectPosition(player, json);
        }

        public void OnMapCreatorSyncRemoveObject(IPlayer modPlayer, int id)
        {
            var player = _tdsPlayerHandler.GetIfLoggedIn(modPlayer);
            if (player is null)
                return;
            if (!(player.Lobby is MapCreateLobby lobby))
                return;
            lobby.SyncRemoveObject(player, id);
        }

        public void OnMapCreatorSyncRemoveTeamObjects(IPlayer modPlayer, int teamNumber)
        {
            var player = _tdsPlayerHandler.GetIfLoggedIn(modPlayer);
            if (player is null)
                return;
            if (!(player.Lobby is MapCreateLobby lobby))
                return;
            lobby.SyncRemoveTeamObjects(player, teamNumber);
        }

        public void OnMapsListRequest(IPlayer modPlayer)
        {
            var player = _tdsPlayerHandler.GetIfLoggedIn(modPlayer);
            if (player is null)
                return;
            if (!(player.Lobby is Arena arena))
                return;

            arena.SendMapsForVoting(player);
        }

        public void OnOutsideMapLimit(IPlayer modPlayer)
        {
            var player = _tdsPlayerHandler.GetIfLoggedIn(modPlayer);
            if (player is null)
                return;
            if (!(player.Lobby is Arena arena))
                return;
            if (arena.Entity.LobbyMapSettings.MapLimitType == MapLimitType.KillAfterTime)
                FightLobby.KillPlayer(player, player.Language.TOO_LONG_OUTSIDE_MAP);
        }

        public void OnSendTeamOrder(IPlayer modPlayer, int teamOrderInt)
        {
            var player = _tdsPlayerHandler.GetIfLoggedIn(modPlayer);
            if (player is null)
                return;
            if (!Enum.IsDefined(typeof(TeamOrder), teamOrderInt))
                return;
            player.Lobby?.SendTeamOrder(player, (TeamOrder)teamOrderInt);
        }

        public void OnSpectateNext(IPlayer modPlayer, bool forward)
        {
            var player = _tdsPlayerHandler.GetIfLoggedIn(modPlayer);
            if (player is null)
                return;
            if (!(player.Lobby is FightLobby lobby))
                return;
            lobby.SpectateNext(player, forward);
        }

        public void OnStartDefusing(IPlayer modPlayer)
        {
            var player = _tdsPlayerHandler.GetIfLoggedIn(modPlayer);
            if (player is null)
                return;
            if (!(player.Lobby is Arena arena))
                return;
            if (!(arena.CurrentGameMode is Bomb bombMode))
                return;
            if (!bombMode.StartBombDefusing(player))
                player.SendEvent(ToClientEvent.StopBombPlantDefuse);
        }

        public void OnStartPlanting(IPlayer modPlayer)
        {
            var player = _tdsPlayerHandler.GetIfLoggedIn(modPlayer);
            if (player is null)
                return;
            if (!(player.Lobby is Arena arena))
                return;
            if (!(arena.CurrentGameMode is Bomb bombMode))
                return;
            if (!bombMode.StartBombPlanting(player))
                player.SendEvent(ToClientEvent.StopBombPlantDefuse);
        }

        public void OnStopDefusing(IPlayer modPlayer)
        {
            var player = _tdsPlayerHandler.GetIfLoggedIn(modPlayer);
            if (player is null)
                return;
            if (!(player.Lobby is Arena arena))
                return;
            if (!(arena.CurrentGameMode is Bomb bombMode))
                return;
            bombMode.StopBombDefusing(player);
        }

        public void OnStopPlanting(IPlayer modPlayer)
        {
            var player = _tdsPlayerHandler.GetIfLoggedIn(modPlayer);
            if (player is null)
                return;
            if (!(player.Lobby is Arena arena))
                return;
            if (!(arena.CurrentGameMode is Bomb bombMode))
                return;
            bombMode.StopBombPlanting(player);
        }

        public void OnSuicideKill(IPlayer modPlayer)
        {
            var player = _tdsPlayerHandler.GetIfLoggedIn(modPlayer);
            if (player is null)
                return;
            if (player.Lifes == 0)
                return;
            if (!(player.Lobby is FightLobby))
                return;
            FightLobby.KillPlayer(player, player.Language.COMMITED_SUICIDE);
        }

        public void OnSuicideShoot(IPlayer modPlayer)
        {
            var player = _tdsPlayerHandler.GetIfLoggedIn(modPlayer);
            if (player is null)
                return;
            if (player.Lifes == 0)
                return;
            if (!(player.Lobby is FightLobby fightLobby))
                return;

            _modAPI.Native.Send(fightLobby, NativeHash.SET_PED_SHOOTS_AT_COORD, player.RemoteId, 0f, 0f, 0f, false);
        }

        #endregion Public Methods
    }
}
