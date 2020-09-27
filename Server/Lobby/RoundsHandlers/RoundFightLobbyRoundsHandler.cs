using System;
using System.Threading.Tasks;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Entities.Gamemodes;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Data.Interfaces.LobbySystem.RoundsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.RoundsHandlers.Datas;
using TDS_Server.Data.Models.Map;
using TDS_Server.LobbySystem.GamemodesHandlers;
using TDS_Server.LobbySystem.Lobbies.Abstracts;
using TDS_Server.LobbySystem.RoundsHandlers.Datas;

namespace TDS_Server.LobbySystem.RoundsHandlers
{
    public class RoundFightLobbyRoundsHandler : IRoundFightLobbyRoundsHandler
    {
        public IRoundStatesHandler RoundStates { get; }
        public IGamemode? CurrentGamemode { get; private set; }
        protected readonly IRoundFightLobby Lobby;
        private readonly GamemodesProvider _gamemodesProvider;

        public RoundFightLobbyRoundsHandler(IRoundFightLobby lobby, IServiceProvider serviceProvider)
        {
            Lobby = lobby;
            RoundStates = new RoundFightLobbyRoundStates(lobby);
            _gamemodesProvider = new GamemodesProvider(lobby, serviceProvider);

            lobby.Events.InitNewMap += Events_InitNewMap;
        }

        private void Events_InitNewMap(MapDto map)
        {
            CurrentGamemode = _gamemodesProvider.Get(map);
        }

        public virtual ValueTask<ITeam?> GetTimesUpWinnerTeam()
            => new ValueTask<ITeam?>((ITeam?)null);
    }
}

/*

        private void ShowRoundRanking()
        {
            if (_ranking is null || _ranking.Count == 0)
                return;

            try
            {
                ITDSPlayer winner = _ranking.First().Player;
                ITDSPlayer? second = _ranking.ElementAtOrDefault(1)?.Player;
                ITDSPlayer? third = _ranking.ElementAtOrDefault(2)?.Player;

                //Vector3 rot = new Vector3(0, 0, 345);
                winner.Spawn(new Vector3(-425.48, 1123.55, 325.85), 345);
                winner.Freeze(true);
                winner.Dimension = Dimension;

                if (second is { })
                {
                    second.Spawn(new Vector3(-427.03, 1123.21, 325.85), 345);
                    second.Freeze(true);
                    second.Dimension = Dimension;
                }

                if (third is { })
                {
                    third.Spawn(new Vector3(-424.33, 1122.5, 325.85), 345);
                    third.Freeze(true);
                    third.Dimension = Dimension;
                }

                var othersPos = new Vector3(-425.48, 1123.55, 335.85);
                foreach (var player in Players.Values)
                {
                    if (player != winner && player != second && player != third)
                    {
                        player.Position = othersPos;
                        player.SetInvisible(true);
                    }
                    else
                        player.SetInvisible(false);
                }

                string json = Serializer.ToBrowser(_ranking);
                TriggerEvent(ToClientEvent.StartRankingShowAfterRound, json, winner.RemoteId, second?.RemoteId ?? 0, third?.RemoteId ?? 0);
            }
            catch (Exception ex)
            {
                LoggingHandler.LogError("Error occured: " + ex.GetBaseException().Message, ex.StackTrace ?? Environment.StackTrace);
            }
        }

        private void RoundCheckForEnoughAlive()
        {
            int teamsInRound = GetTeamAmountStillInRound();
            if (teamsInRound <= 1 && CurrentGameMode?.CanEndRound(RoundEndReason.NewPlayer) != false)
            {
                SetRoundStatus(RoundStatus.RoundEnd, RoundEndReason.Death);
            }
        }

        private ITeam? GetRoundWinnerTeam()
        {
            if (CurrentGameMode?.WinnerTeam is { })
                return CurrentGameMode.WinnerTeam;
            return CurrentRoundEndReason switch
            {
                RoundEndReason.Death => GetTeamStillInRound(),
                RoundEndReason.Time => GetTeamWithHighestHP(),
                _ => null,
            };
        }

*/
