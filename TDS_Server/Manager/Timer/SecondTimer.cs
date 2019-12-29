using BonusBotConnector_Client;
using BonusBotConnector_Client.Requests;
using GTANetworkAPI;
using System;
using System.Linq;
using TDS_Common.Instance.Utility;
using TDS_Server.Instance.LobbyInstances;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Utility;

namespace TDS_Server.Manager.Timer
{
    static class SecondTimer
    {
        private static int _counter = 0;

        public static void Execute()
        {
            CreateTimer();

            Console.WriteLine(DateTime.Now.Second);

            ++_counter;

            if (Main.Settings is { })
            {
                if (_counter % Main.Settings.RefreshServerStatsFrequencySec == 0 || _counter == 1)
                {
                    var request = new RAGEServerStatsRequest
                    {
                        PlayerAmountInArena = LobbyManager.Arena.Players.Count,
                        PlayerAmountInCustomLobby = LobbyManager.Lobbies.Where(p => !p.IsOfficial).Sum(l => l.Players.Count),
                        PlayerAmountInGangLobby = LobbyManager.Lobbies.Where(p => p is GangLobby || p is GangActionLobby).Sum(l => l.Players.Count),
                        PlayerAmountInMainMenu = Player.Player.LoggedInPlayers.Where(p => p.CurrentLobby is null || p.CurrentLobby.Type == TDS_Common.Enum.ELobbyType.MainMenu).Count(),
                        PlayerAmountOnline = Player.Player.AmountLoggedInPlayers,
                        ServerPort = NAPI.Server.GetServerPort(),
                        Version = "1.0.0",   // Todo: Save Version somewhere else
                        ServerName = NAPI.Server.GetServerName(),
                        RefreshFrequencySec = Main.Settings.RefreshServerStatsFrequencySec
                    };

                    ServerInfos.Refresh(request);
                }
            }
        }

        public static void CreateTimer()
        {
            _ = new TDSTimer(Execute, Utils.GetMsToNextSecond(), 0);
        }
    }
}
