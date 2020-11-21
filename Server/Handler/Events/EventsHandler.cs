using BonusBotConnector.Client;
using GTANetworkAPI;
using System;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.GangsSystem;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Data.Utility;
using TDS.Server.Database.Entity.GangEntities;
using TDS.Server.Database.Entity.Player;
using TDS.Server.Handler.Entities.GangSystem;

namespace TDS.Server.Handler.Events
{
    public class EventsHandler
    {
        public AsyncValueTaskEvent<ITDSPlayer>? PlayerLoggedOutBefore { get; set; }
        public AsyncValueTaskEvent<(ITDSPlayer, Players)>? PlayerRegisteredBefore { get; set; }

        private int _hourCounter;
        private int _minuteCounter;
        private int _secondCounter;

        private readonly BonusBotConnectorClient _bonusBotConnectorClient;

        public EventsHandler(BonusBotConnectorClient bonusBotConnectorClient)
        {
            Instance = this;
            _bonusBotConnectorClient = bonusBotConnectorClient;
        }

        public delegate void ColshapePlayerDelegate(ITDSColshape colshape, ITDSPlayer player);

        public delegate void CounterDelegate(int counter);

        public delegate void EmptyDelegate();

        public delegate void EntityDelegate(Entity entity);

        public delegate void ErrorDelegate(Exception ex, ITDSPlayer? source = null, bool logToBonusBot = true);

        public delegate void ErrorMessageDelegate(string info, string? stackTrace = null, string? exceptionType = null, ITDSPlayer? source = null, bool logToBonusBot = true);

        public delegate void GangDelegate(IGang gang);

        public delegate void GangHouseDelegate(GangHouse house);

        public delegate void IncomingConnectionDelegate(string ip, string serial, string socialClubName, ulong socialClubId, CancelEventArgs cancel);

        public delegate void LobbyDelegate(IBaseLobby lobby);

        public delegate void NewBanDelegate(PlayerBans ban, bool inOfficialLobby);

        public delegate void PlayerDeathDelegate(ITDSPlayer player, ITDSPlayer killer, uint reason);

        public delegate void PlayerDelegate(ITDSPlayer player);

        public delegate void PlayerGangDelegate(ITDSPlayer player, IGang gang);

        public delegate void PlayerJoinedGangDelegate(ITDSPlayer player, IGang gang, GangRanks rank);

        public delegate void PlayerLobbyDelegate(ITDSPlayer player, IBaseLobby lobby);

        public delegate void PlayerWeaponSwitchDelegate(ITDSPlayer player, WeaponHash oldWeapon, WeaponHash newWeapon);

        public delegate void TDSDbPlayerDelegate(ITDSPlayer player, Players dbPlayer);

        public event EntityDelegate? EntityDeleted;

        public event ErrorDelegate? Error;

        public event ErrorMessageDelegate? ErrorMessage;

        public event GangHouseDelegate? GangHouseLoaded;

        public event GangDelegate? GangObjectCreated;

        public event CounterDelegate? Hour;

        public event IncomingConnectionDelegate? IncomingConnection;

        public event EmptyDelegate? LoadedServerBans;

        public event LobbyDelegate? LobbyCreated;

        public event LobbyDelegate? LobbyRemoved;

        public event EmptyDelegate? MapsLoaded;

        public event CounterDelegate? Minute;

        public event NewBanDelegate? NewBan;

        public event PlayerDelegate? PlayerConnected;

        public event PlayerDeathDelegate? PlayerDeath;

        public event PlayerDelegate? PlayerDisconnected;

        public event ColshapePlayerDelegate? PlayerEnteredColshape;

        public event PlayerDelegate? PlayerJoinedCustomMenuLobby;

        public AsyncValueTaskEvent<(ITDSPlayer player, IGang gang, GangRanks rank)>? PlayerJoinedGang { get; set; }

        public event PlayerLobbyDelegate? PlayerJoinedLobby;

        public event PlayerDelegate? PlayerLeftCustomMenuLobby;

        public event PlayerGangDelegate? PlayerLeftGang;

        public event PlayerLobbyDelegate? PlayerLeftLobby;

        public event PlayerDelegate? PlayerLoggedIn;

        public event PlayerDelegate? PlayerLoggedOut;

        public event TDSDbPlayerDelegate? PlayerRegistered;

        public event PlayerDelegate? PlayerSpawned;

        public event PlayerWeaponSwitchDelegate? PlayerWeaponSwitch;

        public event EmptyDelegate? ResourceStop;

        public event CounterDelegate? Second;

        public event EmptyDelegate? Update;

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        public static EventsHandler Instance { get; private set; }
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

        public void OnEntityDeleted(Entity entity)
        {
            try
            {
                EntityDeleted?.Invoke(entity);
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance?.LogError(ex);
            }
        }

        public void OnIncomingConnection(string ip, string serial, string socialClubName, ulong socialClubId, CancelEventArgs cancel)
        {
            try
            {
                IncomingConnection?.Invoke(ip, serial, socialClubName, socialClubId, cancel);
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance?.LogError(ex);
            }
        }

        public void OnMapsLoaded()
        {
            try
            {
                MapsLoaded?.Invoke();
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance?.LogError(ex);
            }
        }

        public void OnPlayerConnected(ITDSPlayer player)
        {
            try
            {
                PlayerConnected?.Invoke(player);
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance?.LogError(ex);
            }
        }

        public void OnPlayerDeath(ITDSPlayer player, ITDSPlayer killer, uint reason)
        {
            try
            {
                PlayerDeath?.Invoke(player, killer, reason);
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance?.LogError(ex);
            }
        }

        public void OnPlayerDisconnected(ITDSPlayer player)
        {
            try
            {
                PlayerDisconnected?.Invoke(player);
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance?.LogError(ex);
            }
        }

        public void OnPlayerEnterColshape(ITDSColshape colshape, ITDSPlayer player)
        {
            try
            {
                PlayerEnteredColshape?.Invoke(colshape, player);
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance?.LogError(ex);
            }
        }

        public async Task OnPlayerLoggedOut(ITDSPlayer tdsPlayer)
        {
            try
            {
                var task = PlayerLoggedOutBefore?.InvokeAsync(tdsPlayer);
                if (task.HasValue)
                    await task.Value.ConfigureAwait(false);
                PlayerLoggedOut?.Invoke(tdsPlayer);
                await tdsPlayer.Database.ExecuteForDBAsync(async dbContext =>
                {
                    await dbContext.DisposeAsync().ConfigureAwait(false);
                }).ConfigureAwait(false);
                tdsPlayer.Events.TriggerRemoved();
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance?.LogError(ex);
            }
        }

        public void OnPlayerLogin(ITDSPlayer tdsPlayer)
        {
            try
            {
                PlayerLoggedIn?.Invoke(tdsPlayer);
            }
            catch (Exception ex) { LoggingHandler.Instance?.LogError(ex); }
        }

        public async Task OnPlayerRegistering(ITDSPlayer player, Players dbPlayer)
        {
            var task = PlayerRegisteredBefore?.InvokeAsync((player, dbPlayer));
            if (task.HasValue)
                await task.Value.ConfigureAwait(false);
        }

        public void OnPlayerRegistered(ITDSPlayer player, Players dbPlayer)
        {
            try
            {
                PlayerRegistered?.Invoke(player, dbPlayer);
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance?.LogError(ex);
            }
            
        }

        public void OnPlayerSpawn(ITDSPlayer player)
        {
            try
            {
                PlayerSpawned?.Invoke(player);
            }
            catch (Exception ex) { LoggingHandler.Instance?.LogError(ex); }
        }

        public void OnPlayerWeaponSwitch(ITDSPlayer player, WeaponHash previousWeapon, WeaponHash newWeapon)
        {
            try
            {
                if (player.Lobby is not IFightLobby fightLobby)
                    return;

                player.Events.TriggerWeaponSwitch(previousWeapon, newWeapon);
                PlayerWeaponSwitch?.Invoke(player, previousWeapon, newWeapon);
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance?.LogError(ex);
            }
        }

        public void OnResourceStop()
        {
            try
            {
                ResourceStop?.Invoke();
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance?.LogError(ex);
            }
        }

        public void OnUpdate()
        {
            try
            {
                Update?.Invoke();
            }
            catch (Exception ex) { LoggingHandler.Instance?.LogError(ex); }
        }

        internal void OnGangHouseLoaded(GangHouse house)
        {
            try
            {
                GangHouseLoaded?.Invoke(house);
            }
            catch (Exception ex) { LoggingHandler.Instance?.LogError(ex); }
        }

        public void OnNewBan(PlayerBans ban, bool isOfficial, ulong? targetDiscordUserId)
        {
            try
            {
                NewBan?.Invoke(ban, isOfficial);
                if (isOfficial)
                    _bonusBotConnectorClient.OnNewOfficialBan(ban, targetDiscordUserId);
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance?.LogError(ex);
            }
        }

        /*public void OnPlayerEnterVehicle(ITDSPlayer tdsPlayer, ITDSVehicle vehicle, sbyte seatId)
        {
            tdsPlayer.Lobby?.OnPlayerEnterVehicle(tdsPlayer, vehicle, seatId);
        }

        public void OnPlayerExitVehicle(ITDSPlayer tdsPlayer, ITDSVehicle vehicle)
        {
            tdsPlayer.Lobby?.OnPlayerExitVehicle(tdsPlayer, vehicle);
        }*/

        public void OnError(Exception ex, string msgBefore)
        {
            try
            {
                ErrorMessage?.Invoke($"{msgBefore}{Environment.NewLine}{ex.GetBaseException().Message}");
            }
            catch
            {

            }
        }

        public void OnLobbyCreated(IBaseLobby lobby)
        {
            try
            {
                LobbyCreated?.Invoke(lobby);
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance?.LogError(ex);
            }
        }

        public void OnLobbyRemoved(IBaseLobby lobby)
        {
            try
            {
                LobbyRemoved?.Invoke(lobby);
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance?.LogError(ex);
            }
        }

        internal void OnCustomLobbyMenuJoin(ITDSPlayer player)
        {
            try
            {
                PlayerJoinedCustomMenuLobby?.Invoke(player);
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance?.LogError(ex);
            }
        }

        internal void OnCustomLobbyMenuLeave(ITDSPlayer player)
        {
            try
            {
                PlayerLeftCustomMenuLobby?.Invoke(player);
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance?.LogError(ex);
            }
        }

        internal async Task OnGangJoin(ITDSPlayer player, IGang gang, GangRanks rank)
        {
            try
            {
                var task = PlayerJoinedGang?.InvokeAsync((player, gang, rank));
                if (task.HasValue)
                    await task.Value.ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance?.LogError(ex);
            }
        }

        internal void OnGangLeave(ITDSPlayer player, IGang gang)
        {
            try
            {
                PlayerLeftGang?.Invoke(player, gang);
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance?.LogError(ex);
            }
        }

        internal void OnHour()
        {
            try
            {
                Hour?.Invoke(++_hourCounter);
            }
            catch (Exception ex)
            {
                Error?.Invoke(ex);
            }
        }

        internal void OnLoadedServerBans()
        {
            try
            {
                LoadedServerBans?.Invoke();
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance?.LogError(ex);
            }
        }

        public void OnLobbyJoin(ITDSPlayer player, IBaseLobby lobby)
        {
            try
            {
                PlayerJoinedLobby?.Invoke(player, lobby);
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance?.LogError(ex);
            }
        }

        public void OnLobbyLeave(ITDSPlayer player, IBaseLobby lobby)
        {
            try
            {
                PlayerLeftLobby?.Invoke(player, lobby);
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance?.LogError(ex);
            }
        }

        public void OnGangObjectCreated(IGang gang)
        {
            try
            {
                GangObjectCreated?.Invoke(gang);
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance?.LogError(ex);
            }
        }

        internal void OnMinute()
        {
            try
            {
                Minute?.Invoke(++_minuteCounter);
            }
            catch (Exception ex)
            {
                Error?.Invoke(ex);
            }
        }

        internal void OnSecond()
        {
            try
            {
                Second?.Invoke(++_secondCounter);
            }
            catch (Exception ex)
            {
                Error?.Invoke(ex);
            }
        }
    }
}
