using BonusBotConnector_Server;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI.Player;
using TDS_Server.Handler.Account;
using TDS_Server.Handler.Player;
using TDS_Server.Handler.Userpanel;

namespace TDS_Server.Handler.Events
{
    public class EventsHandler
    {
        private readonly BansHandler _bansHandler;
        private readonly LobbiesHandler _lobbiesHandler;
        private readonly ResourceStopHandler _resourceStopHandler;
        private readonly UserpanelHandler _userpanelHandler;

        public EventsHandler(BansHandler bansHandler, LobbiesHandler lobbiesHandler, ResourceStopHandler resourceStopHandler,
            BonusBotConnectorServer bonusBotConnectorServer, UserpanelHandler userpanelHandler)
        {
            (_bansHandler, _lobbiesHandler, _resourceStopHandler) 
            = (bansHandler, lobbiesHandler, resourceStopHandler);

            _userpanelHandler = userpanelHandler;
                
            bonusBotConnectorServer.CommandService.OnUsedCommand += BBCommandService_OnUsedCommand;
        }

        public delegate void PlayerDelegate(ITDSPlayer player);
        public event PlayerDelegate? PlayerConnected;

        public event PlayerDelegate? PlayerLoggedIn;
        public event PlayerDelegate? PlayerRegistered;
        public event PlayerDelegate? PlayerLoggedOut;

        public delegate void ModPlayerDelegate(IPlayer player);
        public event ModPlayerDelegate? PlayerDisconnected;

        public delegate ValueTask PlayerAsyncDelegate(ITDSPlayer player);
        public event PlayerAsyncDelegate? PlayerLoggedOutBefore;

        public delegate void PlayerLobbyDelegate(ITDSPlayer player, ILobby lobby);
        public event PlayerLobbyDelegate? PlayerJoinedLobby;
        public event PlayerLobbyDelegate? PlayerLeftLobby;

        public delegate void CounterDelegate(ulong counter);
        public event CounterDelegate? Second;
        public event CounterDelegate? Minute;
        public event CounterDelegate? Hour;

        public delegate void EmptyDelegate();
        public event EmptyDelegate? MapsLoaded;
        public event EmptyDelegate? Update;

        public delegate void ErrorDelegate(Exception ex, ITDSPlayer? source = null, bool logToBonusBot = true);
        public event ErrorDelegate? Error;

        public void OnUpdate()
        {
            Update?.Invoke();
        }

        public void OnPlayerConnected(ITDSPlayer tdsPlayer)
        {
            PlayerConnected?.Invoke(tdsPlayer);
        }

        public void OnPlayerDisconnected(IPlayer modPlayer)
        {
            PlayerDisconnected?.Invoke(modPlayer);
        }

        public void OnIncomingConnection(string ip, string serial, string socialClubName, ulong socialClubId, CancelEventArgs cancel)
        {
            var ban = _bansHandler.GetServerBan(null, ip, serial, socialClubName, socialClubId, true);
            if (ban is { })
                cancel.Cancel = true;
        }

        public void OnResourceStop()
        {
            _resourceStopHandler?.OnResourceStop();
        }




        public void OnPlayerLogin(ITDSPlayer tdsPlayer)
        {
            PlayerLoggedIn?.Invoke(tdsPlayer);
        }

        public void OnPlayerRegister(ITDSPlayer tdsPlayer)
        {
            PlayerRegistered?.Invoke(tdsPlayer);
        }

        public void OnMapsLoaded()
        {
            MapsLoaded?.Invoke();
        }

        internal void OnLobbyJoin(ITDSPlayer player, ILobby lobby)
        {
            PlayerJoinedLobby?.Invoke(player, lobby);
        }

        internal void OnLobbyLeave(ITDSPlayer player, ILobby lobby)
        {
            PlayerLeftLobby?.Invoke(player, lobby);
        }







        private ulong _hourCounter;
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

        private ulong _minuteCounter;
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

        private ulong _secondCounter;
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




        private string? BBCommandService_OnUsedCommand(ulong userId, string command)
        {
            return command switch
            {
                "confirmtds" => _userpanelHandler.SettingsNormalHandler.ConfirmDiscordUserId(userId),
                _ => null,
            };
        }
    }
}
