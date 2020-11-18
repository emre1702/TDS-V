using GTANetworkAPI;
using System;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Extensions;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.GamemodesSystem.Gamemodes;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Handler;
using TDS.Server.Handler.Extensions;
using TDS.Server.LobbySystem.Players;
using TDS.Shared.Data.Enums;
using TDS.Shared.Default;

namespace TDS.Server.LobbySystem.EventsHandlers
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
            try
            {
                if (!player.LoggedIn)
                    return;
                if (!(player.Lobby?.Players is ArenaPlayers players))
                    return;
                await players.ChooseTeam(player, index).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _loggingHandler.LogError(ex);
            }
        }

        public void OnGotHit(ITDSPlayer player, int damage)
        {
            try
            {
                if (!player.LoggedIn)
                    return;

                player.HealthAndArmor.Remove(damage, out _, out _);
            }
            catch (Exception ex)
            {
                _loggingHandler.LogError(ex);
            }
        }

        public void OnHitOtherPlayer(ITDSPlayer attacker, int targetRemoteId, long weaponHashLong, int bodyPartValue)
        {
            try
            {
                if (!attacker.LoggedIn)
                    return;
                var target = _tdsPlayerHandler.GetIfLoggedIn((ushort)targetRemoteId);
                if (target is null)
                    return;
                var weaponHash = (WeaponHash)weaponHashLong;
                var bodyPart = (PedBodyPart)bodyPartValue;

                if (target.Lobby is not IFightLobby fightLobby)
                {
                    _loggingHandler.LogError(
                        string.Format("Attacker {0} dealt damage on body part {1} to {2} - but this player isn't in fightlobby.",
                            attacker.DisplayName, bodyPart, target.DisplayName),
                        Environment.StackTrace, null, attacker);
                    return;
                }

                fightLobby.Deathmatch.Damage.DamageDealer.DamagePlayer(target, weaponHash, bodyPart, attacker);
            }
            catch (Exception ex)
            {
                _loggingHandler.LogError(ex);
            }
        }

        public async void OnLeaveLobby(ITDSPlayer player)
        {
            try
            {
                if (!player.LoggedIn)
                    return;
                if (player.Lobby is null)
                    return;

                await player.Lobby.Players.RemovePlayer(player).ConfigureAwait(false);
                await _lobbiesHandler.MainMenu.Players.AddPlayer(player, 0).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _loggingHandler.LogError(ex);
            }
        }

        public void OnMapCreatorStartNewMap(ITDSPlayer player)
        {
            try
            {
                if (!player.LoggedIn)
                    return;
                if (!(player.Lobby is IMapCreatorLobby lobby))
                    return;
                lobby.Sync.StartNewMap();
            }
            catch (Exception ex)
            {
                _loggingHandler.LogError(ex);
            }
        }

        public void OnMapCreatorSyncLastId(ITDSPlayer player, int id)
        {
            try
            {
                if (!player.LoggedIn)
                    return;
                if (!(player.Lobby is IMapCreatorLobby lobby))
                    return;
                lobby.Sync.SyncLastId(player, id);
            }
            catch (Exception ex)
            {
                _loggingHandler.LogError(ex);
            }
        }

        public void OnMapCreatorSyncNewObject(ITDSPlayer player, string json)
        {
            try
            {
                if (!player.LoggedIn)
                    return;
                if (!(player.Lobby is IMapCreatorLobby lobby))
                    return;
                lobby.Sync.SyncNewObject(player, json);
            }
            catch (Exception ex)
            {
                _loggingHandler.LogError(ex);
            }
        }

        public void OnMapCreatorSyncObjectPosition(ITDSPlayer player, string json)
        {
            try
            {
                if (!player.LoggedIn)
                    return;
                if (!(player.Lobby is IMapCreatorLobby lobby))
                    return;
                lobby.Sync.SyncObjectPosition(player, json);
            }
            catch (Exception ex)
            {
                _loggingHandler.LogError(ex);
            }
        }

        public void OnMapCreatorSyncRemoveObject(ITDSPlayer player, int id)
        {
            try
            {
                if (!player.LoggedIn)
                    return;
                if (!(player.Lobby is IMapCreatorLobby lobby))
                    return;
                lobby.Sync.SyncRemoveObject(player, id);
            }
            catch (Exception ex)
            {
                _loggingHandler.LogError(ex);
            }
        }

        public void OnMapCreatorSyncRemoveTeamObjects(ITDSPlayer player, int teamNumber)
        {
            try
            {
                if (!player.LoggedIn)
                    return;
                if (!(player.Lobby is IMapCreatorLobby lobby))
                    return;
                lobby.Sync.SyncRemoveTeamObjects(player, teamNumber);
            }
            catch (Exception ex)
            {
                _loggingHandler.LogError(ex);
            }
        }

        public void OnMapsListRequest(ITDSPlayer player)
        {
            try
            {
                if (!player.LoggedIn)
                    return;
                if (!(player.Lobby is IArena arena))
                    return;

                var mapsJson = arena.MapHandler.GetMapsJson();
                NAPI.Task.RunSafe(() => player.TriggerEvent(ToClientEvent.MapsListRequest, mapsJson));
            }
            catch (Exception ex)
            {
                _loggingHandler.LogError(ex);
            }
        }

        public void OnOutsideMapLimit(ITDSPlayer player)
        {
            try
            {
                if (!player.LoggedIn)
                    return;
                if (!(player.Lobby is IArena arena))
                    return;
                if (arena.Entity.LobbyMapSettings.MapLimitType == MapLimitType.KillAfterTime)
                    arena.Deathmatch.Kill(player, player.Language.TOO_LONG_OUTSIDE_MAP);
            }
            catch (Exception ex)
            {
                _loggingHandler.LogError(ex);
            }
        }

        public void OnSendTeamOrder(ITDSPlayer player, int teamOrderInt)
        {
            try
            {
                if (!player.LoggedIn)
                    return;
                if (!(player.Lobby is IFightLobby fightLobby))
                    return;
                if (!Enum.IsDefined(typeof(TeamOrder), teamOrderInt))
                    return;
                fightLobby.Teams.SendTeamOrder(player, (TeamOrder)teamOrderInt);
            }
            catch (Exception ex)
            {
                _loggingHandler.LogError(ex);
            }
        }

        public async void OnSpectateNext(ITDSPlayer player, bool forward)
        {
            try
            {
                if (!player.LoggedIn)
                    return;
                if (!(player.Lobby is IFightLobby lobby))
                    return;
                await lobby.Spectator.SpectateNext(player, forward);
            }
            catch (Exception ex)
            {
                _loggingHandler.LogError(ex);
            }
        }

        public void OnStartDefusing(ITDSPlayer player)
        {
            try
            {
                if (!player.LoggedIn)
                    return;
                if (player.Lobby is not IRoundFightLobby roundFightLobby)
                    return;
                if (roundFightLobby.Rounds.CurrentGamemode is not IBombGamemode bombMode)
                    return;
                if (!bombMode.Specials.StartBombDefusing(player))
                    NAPI.Task.RunSafe(() => player.TriggerEvent(ToClientEvent.StopBombPlantDefuse));
            }
            catch (Exception ex)
            {
                _loggingHandler.LogError(ex);
            }
        }

        public void OnStartPlanting(ITDSPlayer player)
        {
            try
            {
                if (!player.LoggedIn)
                    return;
                if (player.Lobby is not IRoundFightLobby roundFightLobby)
                    return;
                if (roundFightLobby.Rounds.CurrentGamemode is not IBombGamemode bombMode)
                    return;
                if (!bombMode.Specials.StartBombPlanting(player))
                    NAPI.Task.RunSafe(() => player.TriggerEvent(ToClientEvent.StopBombPlantDefuse));
            }
            catch (Exception ex)
            {
                _loggingHandler.LogError(ex);
            }
        }

        public void OnStopDefusing(ITDSPlayer player)
        {
            try
            {
                if (!player.LoggedIn)
                    return;
                if (!(player.Lobby is IRoundFightLobby roundFightLobby))
                    return;
                if (!(roundFightLobby.Rounds.CurrentGamemode is IBombGamemode bombMode))
                    return;
                bombMode.Specials.StopBombDefusing(player);
            }
            catch (Exception ex)
            {
                _loggingHandler.LogError(ex);
            }
        }

        public void OnStopPlanting(ITDSPlayer player)
        {
            try
            {
                if (!player.LoggedIn)
                    return;
                if (!(player.Lobby is IRoundFightLobby roundFightLobby))
                    return;
                if (!(roundFightLobby.Rounds.CurrentGamemode is IBombGamemode bombMode))
                    return;
                bombMode.Specials.StopBombPlanting(player);
            }
            catch (Exception ex)
            {
                _loggingHandler.LogError(ex);
            }
        }

        public void OnSuicideKill(ITDSPlayer player)
        {
            try
            {
                if (!player.LoggedIn)
                    return;
                if (player.Lifes == 0)
                    return;
                if (!(player.Lobby is IFightLobby lobby))
                    return;
                lobby.Deathmatch.Kill(player, player.Language.COMMITED_SUICIDE);
            }
            catch (Exception ex)
            {
                _loggingHandler.LogError(ex);
            }
        }

        public void OnSuicideShoot(ITDSPlayer player)
        {
            try
            {
                if (!player.LoggedIn)
                    return;
                if (player.Lifes == 0)
                    return;
                if (!(player.Lobby is IFightLobby fightLobby))
                    return;

                NAPI.Task.RunSafe(() => 
                    fightLobby.Natives.Send(NativeHash.SET_PED_SHOOTS_AT_COORD, player.RemoteId, 0f, 0f, 0f, false));
            }
            catch (Exception ex)
            {
                _loggingHandler.LogError(ex);
            }
        }
    }
}
