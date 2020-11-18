using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Data.Interfaces.LobbySystem.RoundsHandlers.Datas;
using TDS.Server.Data.Interfaces.LobbySystem.RoundsHandlers.Datas.RoundStates;
using TDS.Server.Data.Models;
using TDS.Server.Data.RoundEndReasons;
using TDS.Server.Handler;
using TDS.Server.Handler.Extensions;
using TDS.Server.LobbySystem.RoundsHandlers.Datas.RoundStates;
using TDS.Shared.Core;

namespace TDS.Server.LobbySystem.RoundsHandlers.Datas
{
    public class RoundFightLobbyRoundStates : IRoundStatesHandler, IDisposable
    {
        public LinkedList<RoundState> List { get; } = new LinkedList<RoundState>();
        public LinkedListNode<RoundState> Current { get; private set; }
        public IRoundState CurrentState => Current.Value;
        public LinkedListNode<RoundState> Next => Current == List.Last ? List.First! : Current.Next!;
        public int TimeToNextStateMs => (int?)_nextTimer?.RemainingMsToExecute ?? 0;
        public int TimeInStateMs => (int?)_nextTimer?.ElapsedMsSinceLastExecOrCreate ?? 0;
        public bool Started { get; private set; }

        public IRoundEndReason CurrentRoundEndReason { get; protected set; } = new TimeRoundEndReason(null);

        private TDSTimer? _nextTimer;
        private readonly IRoundFightLobby _lobby;
        private readonly IRoundFightLobbyEventsHandler _events;
        private bool _lobbyRemoved;
        private readonly SemaphoreSlim _roundWaitSemaphore = new SemaphoreSlim(1, 1);

        public RoundFightLobbyRoundStates(IRoundFightLobby lobby, IRoundFightLobbyEventsHandler events)
        {
            _lobby = lobby;
            _events = events;

            var node = List.AddFirst(new NewMapChooseState(lobby));
            node = List.AddAfter(node, new TeamPreparationState(lobby));
            node = List.AddAfter(node, new WeaponsLoadingState(lobby));
            node = List.AddAfter(node, new PlayerPreparationState(lobby));
            node = List.AddAfter(node, new CountdownState(lobby));
            node = List.AddAfter(node, new InRoundState(lobby));
            node = List.AddAfter(node, new RoundEndState(lobby));

            if (lobby.Entity.LobbyRoundSettings.ShowRanking)
                node = List.AddAfter(node, new RoundEndRankingState(lobby));

            node = List.AddAfter(node, new RoundEndStatsState(lobby));
            List.AddLast(new RoundClear(lobby));

            Current = List.Last!;

            events.PlayerJoinedAfter += Events_PlayerJoinedAfter;
            events.PlayerLeft += Events_PlayerLeft;
            events.RemoveAfter += Events_RemoveAfter;
        }

        protected virtual void RemoveEvents()
        {
            if (_events.PlayerJoinedAfter is { })
                _events.PlayerJoinedAfter -= Events_PlayerJoinedAfter;
            if (_events.PlayerLeft is { })
                _events.PlayerLeft -= Events_PlayerLeft;
            _events.RemoveAfter -= Events_RemoveAfter;
        }

        public virtual async void SetNext()
        {
            try
            {
                await _roundWaitSemaphore.Do(() =>
                {
                    if (_lobbyRemoved)
                        StopRound();
                    else
                        SetState(Next);
                }).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance.LogError(ex);
            }
        }

        public void StartRound()
        {
            if (Started)
                return;
            Started = true;
            Task.Run(SetNext);
        }

        public async void StopRound()
        {
            try
            {
                if (!Started)
                    return;
                Started = false;
                await _roundWaitSemaphore.Do(() =>
                {
                    _nextTimer?.Kill();
                    if (Current != List.Last)
                        List.Last!.Value.SetCurrent();
                    Current = List.Last!;
                }).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance.LogError(ex);
            }
        }

        public void EndRound(IRoundEndReason roundEndReason)
        {
            if (CurrentState is RoundEndState)
                return;
            if (_lobbyRemoved)
                StopRound();
            else
            {
                CurrentRoundEndReason = roundEndReason;
                SetState<RoundEndState>();
            }
        }

        private void SetState<T>() where T : RoundState
            => SetState(Get<T>());

        private void SetState(LinkedListNode<RoundState> state)
        {
            _nextTimer?.Kill();
            Current = state;
            Current.Value.SetCurrent();

            _nextTimer = new TDSTimer(SetNext, (uint)Current.Value.Duration);
        }

        private LinkedListNode<RoundState> Get<T>() where T : RoundState
        {
            var roundState = List.First(state => state is T);
            return List.Find(roundState)!;
        }

        private ValueTask Events_PlayerJoinedAfter((ITDSPlayer Player, int TeamIndex) data)
        {
            if (data.TeamIndex == 0)
                return default;
            if (_lobby.Players.Count == 1)
                StartRound();
            return default;
        }

        private async ValueTask Events_PlayerLeft((ITDSPlayer Player, int HadLifes) _)
        {
            if (!await _lobby.Players.Any().ConfigureAwait(false))
                StopRound();
        }

        private void Events_RemoveAfter(IBaseLobby lobby)
        {
            _lobbyRemoved = true;
            RemoveEvents();
        }

        public async Task<IDisposable> GetContext([CallerMemberName] string calledFrom = "")
        {
            await _roundWaitSemaphore.WaitAsync().ConfigureAwait(false);
            Console.WriteLine($"Semaphore started from: {calledFrom}");
            return new WaitDisposable(() =>
            {
                _roundWaitSemaphore.Release();
                Console.WriteLine($"Semaphore ended from: {calledFrom}");
            });
        }

        public bool IsCurrentStateBeforeRoundEnd()
        {
            var node = Current;
            while (node is { } && !(node.Value is RoundEndState))
                node = node.Next;
            return node is { };
        }

        public bool IsCurrentStateAfterRound()
        {
            var node = Current;
            while (node is { } && !(node.Value is InRoundState))
                node = node.Previous;
            return node is null;
        }

        public Task<bool> IsCurrentStateBeforeRoundEndBlocked()
        {
            return _roundWaitSemaphore.Do(() =>
            {
                var node = Current;
                while (node is { } && !(node.Value is RoundEndState))
                    node = node.Next;
                return node is { };
            });
        }

        public void Dispose()
        {
            _nextTimer?.Kill();
            _lobbyRemoved = true;
            _roundWaitSemaphore.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
