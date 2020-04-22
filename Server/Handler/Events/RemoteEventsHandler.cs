using Microsoft.Extensions.DependencyInjection;
using System;
using TDS_Server.Core.Manager.PlayerManager;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Handler.Account;
using TDS_Server.Handler.Commands;
using TDS_Server.Handler.Entities.GameModes.Bomb;
using TDS_Server.Handler.Entities.LobbySystem;
using TDS_Server.Handler.Maps;
using TDS_Server.Handler.Sync;
using TDS_Server.Handler.Userpanel;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Enums.Userpanel;
using TDS_Shared.Default;

namespace TDS_Server.Handler.Events
{
    public class RemoteEventsHandler
    {
        private readonly ChatHandler _chatHandler;
        private readonly LoginHandler _loginHandler;
        private readonly CommandsHandler _commandsHandler;
        private readonly RegisterHandler _registerHandler;
        private readonly ScoreboardHandler _scoreboardHandler;
        private readonly LobbiesHandler _lobbiesHandler;
        private readonly ILoggingHandler _loggingHandler;
        private readonly IModAPI _modAPI;
        private readonly DataSyncHandler _dataSyncHandler;
        private readonly MapsRatingsHandler _mapsRatingsHandler;
        private readonly MapCreatorHandler _mapCreatorHandler;
        private readonly UserpanelHandler _userpanelHandler;

        public RemoteEventsHandler(ChatHandler chatHandler, LoginHandler loginHandler, CommandsHandler commandsHandler,
            RegisterHandler registerHandler, ScoreboardHandler scoreboardHandler, LobbiesHandler lobbiesHandler,
            ILoggingHandler loggingHandler, IModAPI modAPI, DataSyncHandler dataSyncHandler,
            MapsRatingsHandler mapsRatingsHandler, MapCreatorHandler mapCreatorHandler, UserpanelHandler userpanelHandler)
            => (_chatHandler, _loginHandler, _commandsHandler, _registerHandler, _scoreboardHandler, _lobbiesHandler,
            _loggingHandler, _modAPI, _dataSyncHandler, _mapsRatingsHandler, _mapCreatorHandler, _userpanelHandler) 
            = (chatHandler, loginHandler, commandsHandler, registerHandler, scoreboardHandler, lobbiesHandler,
            loggingHandler, modAPI, dataSyncHandler, mapsRatingsHandler, mapCreatorHandler, userpanelHandler);

        public void LobbyChatMessage(ITDSPlayer player, string message, int chatTypeNumber)
        {
            _chatHandler.SendLobbyMessage(player, message, chatTypeNumber);
        }

        public void TryLogin(ITDSPlayer player, string username, string password)
        {
            if (player.TryingToLoginRegister)
                return;
            _loginHandler.TryLogin(player, username, password);
        }

        public void TryRegister(ITDSPlayer player, string username, string password, string email)
        {
            if (player.TryingToLoginRegister)
                return;
            _registerHandler.TryRegister(player, username, password, email);
        }

        public void UseCommand(ITDSPlayer tdsPlayer, string msg)
        {
            _commandsHandler.UseCommand(tdsPlayer, msg);
        }

        public void OnLanguageChange(ITDSPlayer player, Language language)
        {
            player.LanguageEnum = language;
        }

        public void OnRequestPlayersForScoreboard(ITDSPlayer tdsPlayer)
        {
            _scoreboardHandler.OnRequestPlayersForScoreboard(tdsPlayer);
        }

        #region Lobby

        public void OnChooseTeam(ITDSPlayer player, int index)
        {
            if (player.Lobby is null || !(player.Lobby is Arena arena))
                return;
            arena.ChooseTeam(player, index);
        }

        public async void OnLeaveLobby(ITDSPlayer player)
        {
            if (player.Lobby is null)
                return;

            await player.Lobby.RemovePlayer(player);
            await _lobbiesHandler.MainMenu.AddPlayer(player, 0);
        }
        #endregion Lobby

        #region Damagesys

        public void OnGotHit(ITDSPlayer player, ITDSPlayer attacker, WeaponHash weaponHash, ulong boneIdx)
        {
            if (!(player.Lobby is FightLobby fightLobby))
            {
                _loggingHandler.LogError(string.Format("Attacker {0} dealt damage on bone {1} to {2} - but this player isn't in fightlobby.", attacker.DisplayName, boneIdx, player.DisplayName), 
                    Environment.StackTrace, attacker);
                return;
            }

            fightLobby.DamagedPlayer(player, attacker, weaponHash, boneIdx);
        }

        #endregion Damagesys

        public void OnOutsideMapLimit(ITDSPlayer player)
        {
            if (!(player.Lobby is Arena arena))
                return;
            if (arena.Entity.LobbyMapSettings.MapLimitType == MapLimitType.KillAfterTime)
                FightLobby.KillPlayer(player, player.Language.TOO_LONG_OUTSIDE_MAP);
        }

        public void OnSendTeamOrder(ITDSPlayer player, TeamOrder teamOrder)
        {
            player.Lobby?.SendTeamOrder(player, teamOrder);
        }

        public void OnSuicideKill(ITDSPlayer player)
        {
            if (player.Lifes == 0)
                return;
            if (!(player.Lobby is FightLobby))
                return;
            FightLobby.KillPlayer(player, player.Language.COMMITED_SUICIDE);
        }

        public void OnSuicideShoot(ITDSPlayer player)
        {
            if (player.Lifes == 0)
                return;
            if (!(player.Lobby is FightLobby fightLobby))
                return;

            _modAPI.Native.Send(fightLobby, NativeHash.SET_PED_SHOOTS_AT_COORD, player.RemoteId, 0f, 0f, 0f, false);
        }

        public void OnToggleCrouch(ITDSPlayer player)
        {
            player.IsCrouched = !player.IsCrouched;
            _dataSyncHandler.SetData(player, PlayerDataKey.Crouched, PlayerDataSyncMode.Lobby, player.IsCrouched);
        }

        #region Bomb

        public void OnStartPlanting(ITDSPlayer player)
        {
            if (!(player.Lobby is Arena arena))
                return;
            if (!(arena.CurrentGameMode is Bomb bombMode))
                return;
            if (!bombMode.StartBombPlanting(player))
                player.SendEvent(ToClientEvent.StopBombPlantDefuse);
        }

        public void OnStopPlanting(ITDSPlayer player)
        {
            if (!(player.Lobby is Arena arena))
                return;
            if (!(arena.CurrentGameMode is Bomb bombMode))
                return;
            bombMode.StopBombPlanting(player);
        }

        public void OnStartDefusing(ITDSPlayer player)
        {
            if (!(player.Lobby is Arena arena))
                return;
            if (!(arena.CurrentGameMode is Bomb bombMode))
                return;
            if (!bombMode.StartBombDefusing(player))
                player.SendEvent(ToClientEvent.StopBombPlantDefuse);
        }

        public void OnStopDefusing(ITDSPlayer player)
        {
            if (!(player.Lobby is Arena arena))
                return;
            if (!(arena.CurrentGameMode is Bomb bombMode))
                return;
            bombMode.StopBombDefusing(player);
        }

        #endregion Bomb

        #region Spectate

        public void OnSpectateNext(ITDSPlayer player, bool forward)
        {
            if (!(player.Lobby is FightLobby lobby))
                return;
            lobby.SpectateNext(player, forward);
        }

        #endregion Spectate

        #region MapVote

        public void OnMapsListRequest(ITDSPlayer player)
        {
            if (!(player.Lobby is Arena arena))
                return;

            arena.SendMapsForVoting(player);
        }

        #endregion MapVote

        #region Map Rating

        public void OnSendMapRating(ITDSPlayer player, int mapId, int rating)
        {
            _mapsRatingsHandler.AddPlayerMapRating(player, mapId, (byte)rating);
        }

        #endregion Map Rating

        #region MapCreator

        public void OnRemoveMap(ITDSPlayer player, int mapId)
        {
            _mapCreatorHandler.RemoveMap(player, mapId);
        }

        public void OnMapCreatorSyncLastId(ITDSPlayer player, int id)
        {
            if (!(player.Lobby is MapCreateLobby lobby))
                return;
            lobby.SyncLastId(player, id);
        }

        public void OnMapCreatorSyncNewObject(ITDSPlayer player, string json)
        {
            if (!(player.Lobby is MapCreateLobby lobby))
                return;
            lobby.SyncNewObject(player, json);
        }

        public void OnMapCreatorSyncObjectPosition(ITDSPlayer player, string json)
        {
            if (!(player.Lobby is MapCreateLobby lobby))
                return;
            lobby.SyncObjectPosition(player, json);
        }

        public void OnMapCreatorSyncRemoveObject(ITDSPlayer player, int id)
        {
            if (!(player.Lobby is MapCreateLobby lobby))
                return;
            lobby.SyncRemoveObject(player, id);
        }

        public void OnMapCreatorSyncRemoveTeamObjects(ITDSPlayer player, int teamNumber)
        {
            if (!(player.Lobby is MapCreateLobby lobby))
                return;
            lobby.SyncRemoveTeamObjects(player, teamNumber);
        }

        public void OnMapCreatorSyncAllObjects(ITDSPlayer player, int tdsPlayerId, string json, int lastId)
        {
            if (!(player.Lobby is MapCreateLobby lobby))
                return;
            lobby.SyncAllObjectsToPlayer(tdsPlayerId, json, lastId);
        }

        public void OnMapCreatorStartNewMap(ITDSPlayer player)
        {
            if (!(player.Lobby is MapCreateLobby lobby))
                return;
            lobby.StartNewMap();
        }

        public void OnMapCreatorSetInFreeCam(ITDSPlayer tdsPlayer, bool inFreeCam)
        {
            _dataSyncHandler.SetData(tdsPlayer, PlayerDataKey.InFreeCam, PlayerDataSyncMode.Lobby, inFreeCam);
        }
        #endregion MapCreator

        #region Userpanel
        public void OnLoadUserpanelData(ITDSPlayer player, int dataType)
        {
            UserpanelLoadDataType type = (UserpanelLoadDataType)dataType;
            _userpanelHandler.PlayerLoadData(player, type);
        }
        #endregion
    }
}
