using BonusBotConnector.Client;
using GTANetworkAPI;
using System;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Data.Utility;
using TDS_Server.Database.Entity.GangEntities;
using TDS_Server.Database.Entity.Player;
using TDS_Server.Handler.Entities.GangSystem;

namespace TDS_Server.Handler.Events
{
    public class EventsHandler
    {
        public AsyncValueTaskEvent<ITDSPlayer>? PlayerLoggedOutBefore;
        public AsyncValueTaskEvent<(ITDSPlayer, Players)>? PlayerRegisteredBefore;

        private int _hourCounter;
        private int _minuteCounter;
        private int _secondCounter;

        private readonly BonusBotConnectorClient _bonusBotConnectorClient;

        public EventsHandler(BonusBotConnectorClient bonusBotConnectorClient)
        {
            Instance = this;
            _bonusBotConnectorClient = bonusBotConnectorClient;
        }

        public delegate void ColshapePlayerDelegate(ITDSColShape colshape, ITDSPlayer player);

        public delegate void CounterDelegate(int counter);

        public delegate void EmptyDelegate();

        public delegate void EntityDelegate(Entity entity);

        public delegate void ErrorDelegate(Exception ex, ITDSPlayer? source = null, bool logToBonusBot = true);

        public delegate void ErrorMessageDelegate(string info, string? stackTrace = null, string? exceptionType = null, ITDSPlayer? source = null, bool logToBonusBot = true);

        public delegate void GangHouseDelegate(GangHouse house);

        public delegate void IncomingConnectionDelegate(string ip, string serial, string socialClubName, ulong socialClubId, CancelEventArgs cancel);

        public delegate void LobbyDelegate(IBaseLobby lobby);

        public delegate void NewBanDelegate(PlayerBans ban, bool inOfficialLobby);

        public delegate void PlayerDeathDelegate(ITDSPlayer player, ITDSPlayer killer, uint reason);

        public delegate void PlayerDelegate(ITDSPlayer player);

        public delegate void PlayerGangDelegate(ITDSPlayer player, IGang gang);

        public delegate void PlayerJoinedGangDelegate(ITDSPlayer player, IGang gang, GangRanks rank);

        public delegate void PlayerLobbyDelegate(ITDSPlayer player, IBaseLobby lobby);

        public delegate void PlayerLobbyDelegateNew(ITDSPlayer player, IBaseLobby lobby);

        public delegate void PlayerWeaponSwitchDelegate(ITDSPlayer player, WeaponHash oldWeapon, WeaponHash newWeapon);

        public delegate void TDSDbPlayerDelegate(ITDSPlayer player, Players dbPlayer);

        public event LobbyDelegate? CustomLobbyCreated;

        public event LobbyDelegate? CustomLobbyRemoved;

        public event EntityDelegate? EntityDeleted;

        public event ErrorDelegate? Error;

        public event ErrorMessageDelegate? ErrorMessage;

        public event GangHouseDelegate? GangHouseLoaded;

        public event CounterDelegate? Hour;

        public event IncomingConnectionDelegate? IncomingConnection;

        public event EmptyDelegate? LoadedServerBans;

        public event LobbyDelegate? LobbyCreated;

        public event EmptyDelegate? MapsLoaded;

        public event CounterDelegate? Minute;

        public event NewBanDelegate? NewBan;

        public event PlayerDelegate? PlayerConnected;

        public event PlayerDeathDelegate? PlayerDeath;

        public event PlayerDelegate? PlayerDisconnected;

        public event ColshapePlayerDelegate? PlayerEnteredColshape;

        public event PlayerDelegate? PlayerJoinedCustomMenuLobby;

        public AsyncValueTaskEvent<(ITDSPlayer player, IGang gang, GangRanks rank)>? PlayerJoinedGang;

        public event PlayerLobbyDelegate? PlayerJoinedLobby;

        public event PlayerLobbyDelegateNew? PlayerJoinedLobbyNew;

        public event PlayerDelegate? PlayerLeftCustomMenuLobby;

        public event PlayerGangDelegate? PlayerLeftGang;

        public event PlayerLobbyDelegate? PlayerLeftLobby;

        public event PlayerLobbyDelegateNew? PlayerLeftLobbyNew;

        public event PlayerDelegate? PlayerLoggedIn;

        public event PlayerDelegate? PlayerLoggedOut;

        public event TDSDbPlayerDelegate? PlayerRegistered;

        public event PlayerDelegate? PlayerSpawned;

        public event PlayerWeaponSwitchDelegate? PlayerWeaponSwitch;

        public event PlayerDelegate? ReloadPlayerChar;

        public event EmptyDelegate? ResourceStop;

        public event CounterDelegate? Second;

        public event EmptyDelegate? Update;

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        public static EventsHandler Instance { get; private set; }
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

        public void OnEntityDeleted(Entity entity)
        {
            EntityDeleted?.Invoke(entity);
        }

        public void OnIncomingConnection(string ip, string serial, string socialClubName, ulong socialClubId, CancelEventArgs cancel)
        {
            IncomingConnection?.Invoke(ip, serial, socialClubName, socialClubId, cancel);
        }

        public void OnMapsLoaded()
        {
            MapsLoaded?.Invoke();
        }

        public void OnPlayerConnected(ITDSPlayer player)
        {
            PlayerConnected?.Invoke(player);
        }

        public void OnPlayerDeath(ITDSPlayer player, ITDSPlayer killer, uint reason)
        {
            player.Lobby?.OnPlayerDeath(player, killer, reason);
            PlayerDeath?.Invoke(player, killer, reason);
        }

        public void OnPlayerDisconnected(ITDSPlayer player)
        {
            PlayerDisconnected?.Invoke(player);
        }

        public void OnPlayerEnterColshape(ITDSColShape colshape, ITDSPlayer player)
        {
            player.Lobby?.OnPlayerEnterColshape(colshape, player);
            PlayerEnteredColshape?.Invoke(colshape, player);
        }

        public async Task OnPlayerLoggedOut(ITDSPlayer tdsPlayer)
        {
            var task = PlayerLoggedOutBefore?.InvokeAsync(tdsPlayer);
            if (task.HasValue)
                await task.Value;
            NAPI.Task.Run(() =>
            {
                PlayerLoggedOut?.Invoke(tdsPlayer);
            });
            await tdsPlayer.Database.ExecuteForDBAsync(async dbContext =>
            {
                await dbContext.DisposeAsync();
            });
        }

        public void OnPlayerLogin(ITDSPlayer tdsPlayer)
        {
            PlayerLoggedIn?.Invoke(tdsPlayer);
        }

        public async void OnPlayerRegister(ITDSPlayer player, Players dbPlayer)
        {
            var task = PlayerRegisteredBefore?.InvokeAsync((player, dbPlayer));
            if (task.HasValue)
                await task.Value;
            PlayerRegistered?.Invoke(player, dbPlayer);
        }

        public void OnPlayerSpawn(ITDSPlayer player)
        {
            player.Lobby?.OnPlayerSpawn(player);
            PlayerSpawned?.Invoke(player);
        }

        public void OnPlayerWeaponSwitch(ITDSPlayer player, WeaponHash previousWeapon, WeaponHash newWeapon)
        {
            if (!(player.Lobby is IFightLobby fightLobby))
                return;

            player.OnPlayerWeaponSwitch(previousWeapon, newWeapon);
            fightLobby.OnPlayerWeaponSwitch(player, previousWeapon, newWeapon);
            PlayerWeaponSwitch?.Invoke(player, previousWeapon, newWeapon);
        }

        public void OnResourceStop()
        {
            ResourceStop?.Invoke();
        }

        public void OnUpdate()
        {
            Update?.Invoke();
        }

        internal void OnGangHouseLoaded(GangHouse house)
        {
            GangHouseLoaded?.Invoke(house);
        }

        public void OnNewBan(PlayerBans ban, bool isOfficial, ulong? targetDiscordUserId)
        {
            NewBan?.Invoke(ban, isOfficial);
            if (isOfficial)
                _bonusBotConnectorClient.OnNewOfficialBan(ban, targetDiscordUserId);
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
            ErrorMessage?.Invoke($"{msgBefore}{Environment.NewLine}{ex.GetBaseException().Message}");
        }

        internal void OnCustomLobbyCreated(IBaseLobby lobby)
        {
            CustomLobbyCreated?.Invoke(lobby);
        }

        public void OnLobbyCreateNew(IBaseLobby lobby)
        {
            LobbyCreated?.Invoke(lobby);
        }

        internal void OnCustomLobbyMenuJoin(ITDSPlayer player)
        {
            PlayerJoinedCustomMenuLobby?.Invoke(player);
        }

        internal void OnCustomLobbyMenuLeave(ITDSPlayer player)
        {
            PlayerLeftCustomMenuLobby?.Invoke(player);
        }

        internal void OnCustomLobbyRemoved(IBaseLobby lobby)
        {
            CustomLobbyRemoved?.Invoke(lobby);
        }

        internal async Task OnGangJoin(ITDSPlayer player, IGang gang, GangRanks rank)
        {
            var task = PlayerJoinedGang?.InvokeAsync((player, gang, rank));
            if (task.HasValue)
                await task.Value;
        }

        internal void OnGangLeave(ITDSPlayer player, IGang gang)
        {
            PlayerLeftGang?.Invoke(player, gang);
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
            LoadedServerBans?.Invoke();
        }

        internal void OnLobbyCreated(IBaseLobby lobby)
        {
            LobbyCreated?.Invoke(lobby);
        }

        internal void OnLobbyJoin(ITDSPlayer player, IBaseLobby lobby)
        {
            PlayerJoinedLobby?.Invoke(player, lobby);
        }

        internal void OnLobbyLeave(ITDSPlayer player, IBaseLobby lobby)
        {
            PlayerLeftLobby?.Invoke(player, lobby);
        }

        public void OnLobbyLeaveNew(ITDSPlayer player, IBaseLobby lobby)
        {
            PlayerLeftLobbyNew?.Invoke(player, lobby);
        }

        public void OnLobbyJoinedNew(ITDSPlayer player, IBaseLobby lobby)
        {
            PlayerJoinedLobbyNew?.Invoke(player, lobby);
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

        //Todo this is not used - check why
        internal void OnReloadPlayerChar(ITDSPlayer player)
        {
            ReloadPlayerChar?.Invoke(player);
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
