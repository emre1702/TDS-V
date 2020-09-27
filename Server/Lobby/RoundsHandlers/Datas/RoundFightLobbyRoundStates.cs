using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Data.Interfaces.LobbySystem.RoundsHandlers.Datas;
using TDS_Server.Data.RoundEndReasons;
using TDS_Server.LobbySystem.RoundsHandlers.Datas.RoundStates;
using TDS_Shared.Core;

namespace TDS_Server.LobbySystem.RoundsHandlers.Datas
{
    public class RoundFightLobbyRoundStates : IRoundStatesHandler
    {
        public LinkedList<RoundState> List = new LinkedList<RoundState>();
        public LinkedListNode<RoundState> Current;
        public LinkedListNode<RoundState> Next => Current == List.Last ? List.First! : Current.Next!;

        public IRoundEndReason CurrentRoundEndReason { get; protected set; } = new TimeRoundEndReason(null);

        private TDSTimer? _nextTimer;
        private readonly IRoundFightLobby _lobby;
        private bool _lobbyRemoved;

        public RoundFightLobbyRoundStates(IRoundFightLobby lobby)
        {
            _lobby = lobby;

            var node = List.AddFirst(new NewMapChooseState(lobby));
            node = List.AddAfter(node, new TeamPreparationState(lobby));
            node = List.AddAfter(node, new WeaponsLoadingState(lobby));
            node = List.AddAfter(node, new PlayerPreparationState(lobby));
            node = List.AddAfter(node, new CountdownState(lobby));
            node = List.AddAfter(node, new InRoundState(lobby));
            node = List.AddAfter(node, new RoundEndState(lobby));
            node = List.AddAfter(node, new RoundEndStatsState(lobby));
            List.AddAfter(node, new RoundEndRankingState(lobby));
            List.AddLast(new RoundClear(lobby));

            Current = List.Last!;

            lobby.Events.PlayerLeft += Events_PlayerLeft;
            lobby.Events.RemoveAfter += Events_RemoveAfter;
        }

        public virtual ValueTask SetNext()
        {
            if (_lobbyRemoved)
                Stop();
            else
                SetState(Next);
            return default;
        }

        public void Start()
            => Task.Run(SetNext);

        public void Stop()
        {
            _nextTimer?.Kill();
            if (Current != List.Last)
                List.Last!.Value.SetCurrent();
            Current = List.Last!;
        }

        public void EndRound(IRoundEndReason roundEndReason)
        {
            if (_lobbyRemoved)
                Stop();
            else
            {
                CurrentRoundEndReason = roundEndReason;
                SetState<RoundState>();
            }
        }

        private void SetState<T>() where T : RoundState
            => SetState(Get<T>());

        private void SetState(LinkedListNode<RoundState> state)
        {
            _nextTimer?.Kill();
            Current = state;
            Current.Value.SetCurrent();

            _nextTimer = new TDSTimer(() => SetNext(), (uint)Current.Value.Duration);
        }

        private LinkedListNode<RoundState> Get<T>() where T : RoundState
        {
            var roundState = List.First(state => state is T);
            return List.Find(roundState)!;
        }

        private async ValueTask Events_PlayerLeft(ITDSPlayer player)
        {
            if (!await _lobby.Players.Any())
                Stop();
        }

        private void Events_RemoveAfter(IBaseLobby lobby)
            => _lobbyRemoved = true;
    }
}
