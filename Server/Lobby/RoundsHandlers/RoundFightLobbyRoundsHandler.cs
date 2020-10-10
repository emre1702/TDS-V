using GTANetworkAPI;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.GamemodesSystem;
using TDS_Server.Data.Interfaces.GamemodesSystem.Gamemodes;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Data.Interfaces.LobbySystem.RoundsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.RoundsHandlers.Datas;
using TDS_Server.Data.Interfaces.TeamsSystem;
using TDS_Server.Data.Models;
using TDS_Server.Data.Models.Map;
using TDS_Server.Data.RoundEndReasons;
using TDS_Server.LobbySystem.RoundsHandlers.Datas;
using TDS_Server.LobbySystem.RoundsHandlers.Datas.RoundStates;
using TDS_Shared.Default;

namespace TDS_Server.LobbySystem.RoundsHandlers
{
    public class RoundFightLobbyRoundsHandler : IRoundFightLobbyRoundsHandler
    {
        public IRoundStatesHandler RoundStates { get; }
        public IBaseGamemode? CurrentGamemode { get; private set; }
        protected readonly IRoundFightLobby Lobby;
        protected readonly IRoundFightLobbyEventsHandler Events;
        private readonly IGamemodesProvider _gamemodesProvider;

        public RoundFightLobbyRoundsHandler(IRoundFightLobby lobby, IRoundFightLobbyEventsHandler events, IGamemodesProvider gamemodesProvider)
        {
            Lobby = lobby;
            RoundStates = new RoundFightLobbyRoundStates(lobby, events);
            Events = events;
            _gamemodesProvider = gamemodesProvider;

            events.RequestGamemode += Events_RequestGamemode;
            events.RoundEnd += Events_RoundEnd;
            events.PlayerJoined += Events_PlayerJoined;
            events.PlayerLeftAfter += Events_PlayerLeftAfter;
            Events.RemoveAfter += RemoveEvents;
        }

        protected virtual void RemoveEvents(IBaseLobby lobby)
        {
            Events.RequestGamemode -= Events_RequestGamemode;
            if (Events.RoundEnd is { })
                Events.RoundEnd -= Events_RoundEnd;
            if (Events.PlayerJoined is { })
                Events.PlayerJoined -= Events_PlayerJoined;
            if (Events.PlayerLeftAfter is { })
                Events.PlayerLeftAfter -= Events_PlayerLeftAfter;
            Events.RemoveAfter -= RemoveEvents;
        }

        private void Events_RequestGamemode(MapDto map)
        {
            CurrentGamemode = _gamemodesProvider.Create(Lobby, map);
        }

        protected virtual async ValueTask Events_RoundEnd()
        {
            var mapId = Lobby.CurrentMap?.BrowserSyncedData.Id ?? 0;
            await Lobby.Players.DoInMain(player =>
            {
                var noTeamOrSpectator = player.Team is null || player.Team.IsSpectator;
                var roundEndReasonText = Lobby.CurrentRoundEndReason.MessageProvider(player.Language);

                player.TriggerEvent(ToClientEvent.RoundEnd, noTeamOrSpectator, roundEndReasonText, mapId);
                player.Lifes = 0;
            }).ConfigureAwait(false);
        }

        private async ValueTask Events_PlayerJoined((ITDSPlayer Player, int TeamIndex) data)
        {
            await SendPlayerRoundInfoOnJoin(data.Player).ConfigureAwait(false);
        }

        protected virtual async ValueTask Events_PlayerLeftAfter((ITDSPlayer Player, int HadLifes) data)
        {
            switch (RoundStates.CurrentState)
            {
                case InRoundState _:
                    if (data.HadLifes > 0)
                        await CheckForEnoughAlive().ConfigureAwait(false);
                    break;
            }
        }

        protected virtual async Task SendPlayerRoundInfoOnJoin(ITDSPlayer player)
        {
            var teamPlayerAmountsJson = await Lobby.Teams.GetAmountInFightSyncDataJson().ConfigureAwait(false);

            NAPI.Task.Run(() =>
            {
                if (Lobby.CurrentMap is { } map)
                    player.TriggerEvent(ToClientEvent.MapChange, map.ClientSyncedDataJson);
                player.TriggerEvent(ToClientEvent.AmountInFightSync, teamPlayerAmountsJson);

                if (Lobby.Rounds.RoundStates.CurrentState is CountdownState)
                    player.TriggerEvent(ToClientEvent.CountdownStart, true, Lobby.Rounds.RoundStates.TimeToNextStateMs);
                else if (Lobby.Rounds.RoundStates.CurrentState is InRoundState)
                    player.TriggerEvent(ToClientEvent.RoundStart, true, Lobby.Rounds.RoundStates.TimeInStateMs);
            });
        }

        public void RewardPlayerForRound(ITDSPlayer player, RoundPlayerRewardsData rewardsData)
        {
            player.GiveMoney(rewardsData.KillsReward + rewardsData.AssistsReward + rewardsData.DamageReward);
            player.SendChatMessage(rewardsData.Message);
        }

        public void AddRoundRewardsMessage(ITDSPlayer player, StringBuilder useStringBuilder, RoundPlayerRewardsData toModel)
        {
            useStringBuilder.Append("#o#____________________#n#");
            useStringBuilder.AppendFormat(player.Language.ROUND_REWARD_INFO,
                    toModel.KillsReward == 0 ? "-" : toModel.KillsReward.ToString(),
                    toModel.AssistsReward == 0 ? "-" : toModel.AssistsReward.ToString(),
                    toModel.DamageReward == 0 ? "-" : toModel.DamageReward.ToString(),
                    toModel.KillsReward + toModel.AssistsReward + toModel.DamageReward);
            useStringBuilder.Append("#n##o#____________________");

            toModel.Message = useStringBuilder.ToString();
            useStringBuilder.Clear();
        }

        public virtual async Task CheckForEnoughAlive()
        {
            (int teamAmountWithAlive, int teamAmount) = await Lobby.Teams.Do(teams =>
                (teams.Count(t => t.Players.AmountAlive > 0),
                teams.Count(t => !t.IsSpectator))).ConfigureAwait(false);

            switch ((teamAmount, teamAmountWithAlive))
            {
                // 2+ teams, <= 1 in round  ->  end
                case var (amount, amountAlive) when amount > 1 && amountAlive <= 1:
                    var winnerTeam = await Lobby.Teams.Do(teams => teams.FirstOrDefault(t => t.Players.AmountAlive >= 1)).ConfigureAwait(false);
                    Lobby.Rounds.RoundStates.EndRound(new DeathRoundEndReason(winnerTeam));
                    break;

                // 1 team, 0 in round  ->  end
                case (1, 0):
                    Lobby.Rounds.RoundStates.EndRound(new DeathRoundEndReason(null));
                    break;

                // 1 team, 1 in round  ->  check for amount of players, if <= 1   ->  end
                case (1, 1):
                    var amountPlayers = await Lobby.Teams.Do(teams => teams.FirstOrDefault(t => t.Players.AmountAlive > 0)?.Players.AmountAlive)
                        .ConfigureAwait(false);
                    if (amountPlayers <= 1)
                        Lobby.Rounds.RoundStates.EndRound(new DeathRoundEndReason(null));
                    break;
            }
        }

        protected virtual async Task<bool> CheckForEnoughAliveAfterJoin()
        {
            var endRound = false;

            using (await RoundStates.GetContext("CheckForEnoughAliveAfterJoin").ConfigureAwait(false))
            {
                if (!(RoundStates.CurrentState is CountdownState || RoundStates.CurrentState is InRoundState))
                    return true;

                (int teamAmountWithAlive, int teamAmount) = await Lobby.Teams.Do(teams =>
                    (teams.Count(t => t.Players.AmountAlive > 0),
                    teams.Count(t => !t.IsSpectator))).ConfigureAwait(false);

                switch ((teamAmount, teamAmountWithAlive))
                {
                    // 2+ teams, <= 1 in round  ->  end
                    case var (amount, amountAlive) when amount > 1 && amountAlive <= 1:
                    // 1 team, 0 in round  ->  end
                    case (1, 0):
                        endRound = true;

                        break;

                    // 1 team, 1 in round  ->  check for amount of players, if <= 1   ->  end
                    case (1, 1):
                        var amountPlayers = await Lobby.Teams.Do(teams => teams.FirstOrDefault(t => t.Players.AmountAlive > 0)?.Players.AmountAlive)
                            .ConfigureAwait(false);
                        if (amountPlayers <= 1)
                            endRound = true;
                        break;
                }
            }

            if (endRound)
                RoundStates.EndRound(new NewPlayerRoundEndReason());
            return !endRound;
        }

        public virtual void SetPlayerReadyForRound(ITDSPlayer player, bool freeze)
        {
            Lobby.Players.SetPlayerDataAlive(player);
            player.LastHitter = null;

            if (player.Team != null && !player.Team.IsSpectator)
            {
                player.Team.Players.AddToSpectatable(player);

                NAPI.Task.Run(() =>
                {
                    player.SetSpectates(null);
                    player.Freeze(freeze);
                    player.SetInvisible(false);
                });
            }
            else
            {
                NAPI.Task.Run(() =>
                {
                    player.Freeze(true);
                    player.SetInvisible(true);
                });
                Lobby.Spectator.SetPlayerInSpectateMode(player);
            }
        }

        public virtual void StartRoundForPlayer(ITDSPlayer player)
        {
            NAPI.Task.Run(() =>
            {
                player.Freeze(false);
                player.TriggerEvent(ToClientEvent.RoundStart, player.Team is null || player.Team.IsSpectator);
            });
        }

        public virtual ValueTask<ITeam?> GetTimesUpWinnerTeam()
            => new ValueTask<ITeam?>(Lobby.Teams.GetTeamWithHighestHp());
    }
}