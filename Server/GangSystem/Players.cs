using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces.GangsSystem;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS_Server.Handler;
using TDS_Server.Handler.Extensions;
using TDS_Server.Handler.GangSystem;
using TDS_Server.Handler.Sync;
using TDS_Shared.Data.Enums;

namespace TDS_Server.GangsSystem
{
    public class Players : IGangPlayers
    {
        private readonly List<ITDSPlayer> _onlinePlayers = new List<ITDSPlayer>();

        private readonly GangsHandler _gangsHandler;
        private readonly LobbiesHandler _lobbiesHandler;
        private readonly DataSyncHandler _dataSyncHandler;

        public Players(GangsHandler gangsHandler, LobbiesHandler lobbiesHandler, DataSyncHandler dataSyncHandler)
            => (_gangsHandler, _lobbiesHandler, _dataSyncHandler) = (gangsHandler, lobbiesHandler, dataSyncHandler);


        public void AddOnline(ITDSPlayer player)
        {
            lock (_onlinePlayers)
            {
                _onlinePlayers.Add(player);
            }
        }

        public void RemoveOnline(ITDSPlayer player)
        {
            lock (_onlinePlayers)
            {
                _onlinePlayers.Remove(player);
            }
        }

        public ITDSPlayer? GetOnline(int playerId)
        {
            lock (_onlinePlayers)
            {
                return _onlinePlayers.FirstOrDefault(p => p.Id == playerId);
            }
        }

        public void Do(Action<ITDSPlayer> action)
        {
            lock (_onlinePlayers)
            {
                foreach (var player in _onlinePlayers)
                    action(player);
            }
        }

        public async Task Do(Func<ITDSPlayer, Task> action)
        {
            foreach (var player in _onlinePlayers.ToList())
                await action(player);
        }

        public void DoInMain(Action<ITDSPlayer> action)
        {
            NAPI.Task.RunSafe(() =>
            {
                lock (_onlinePlayers)
                {
                    foreach (var player in _onlinePlayers)
                        action(player);
                }
            });
        }

        public Task DoInMainWait(Action<ITDSPlayer> action)
        {
            return NAPI.Task.RunWait(() =>
            {
                lock (_onlinePlayers)
                {
                    foreach (var player in _onlinePlayers)
                        action(player);
                }
            });
        }

        public async Task RemoveAll()
        {
            await Do(async player =>
            {
                player.Gang = _gangsHandler.None;
                player.GangRank = _gangsHandler.NoneRank;

                if (player.Lobby is IGangLobby || player.Lobby is IGangActionLobby)
                    await _lobbiesHandler.MainMenu.Players.AddPlayer(player, 0);
            });

            await DoInMainWait(player =>
            {
                _dataSyncHandler.SetData(player, PlayerDataKey.GangId, DataSyncMode.Player, player.Gang.Entity.Id);
            });

        }
    }
}
