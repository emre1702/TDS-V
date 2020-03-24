//using BonusBotConnector_Client;
//using BonusBotConnector_Client.Requests;
//using GTANetworkAPI;
//using System;
//using System.Linq;
//using TDS_Common.Instance.Utility;
//using TDS_Server.Instance.LobbyInstances;
//using TDS_Server.Instance.PlayerInstance;
//using TDS_Server.Manager.Utility;

//namespace TDS_Server.Core.Manager.Timer
//{
//    static class SecondTimer
//    {
//        private static int _counter = 0;

//        public static void Execute()
//        {
//            CreateTimer();

//            ++_counter;

//            if (Main.Settings is { })
//            {
//                if (_counter % Main.Settings.RefreshServerStatsFrequencySec == 0 || _counter == 1)
//                {
//                    var request = new RAGEServerStatsRequest
//                    {
//                        PlayerAmountInArena = LobbyManager.Arena.Players.Count,
//                        PlayerAmountInCustomLobby = LobbyManager.Lobbies.Where(p => !p.IsOfficial).Sum(l => l.Players.Count),
//                        PlayerAmountInGangLobby = LobbyManager.Lobbies.Where(p => p is GangLobby || (p is Arena arena && arena.IsGangActionLobby)).Sum(l => l.Players.Count),
//                        PlayerAmountInMainMenu = PlayerManager.PlayerManager.LoggedInPlayers.Where(p => p.Lobby is null || p.Lobby.Type == TDS_Shared.Data.Enums.LobbyType.MainMenu).Count(),
//                        PlayerAmountOnline = PlayerManager.PlayerManager.AmountLoggedInPlayers,
//                        ServerPort = NAPI.Server.GetServerPort(),
//                        Version = "1.0.0",   // Todo: Save Version somewhere else
//                        ServerName = NAPI.Server.GetServerName(),
//                        RefreshFrequencySec = Main.Settings.RefreshServerStatsFrequencySec
//                    };

//                    ServerInfos.Refresh(request);
//                }
//            }
//        }

//        public static void CreateTimer()
//        {
//            _ = new TDSTimer(Execute, Utils.GetMsToNextSecond(), 1);
//        }
//    }
//}
