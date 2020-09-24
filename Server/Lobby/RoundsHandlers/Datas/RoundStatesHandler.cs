using System.Collections.Generic;
using System.Threading.Tasks;
using TDS_Server.LobbySystem.Lobbies.Abstracts;
using TDS_Server.LobbySystem.RoundsHandlers.Datas.RoundStates;
using TDS_Shared.Core;

namespace TDS_Server.LobbySystem.RoundsHandlers.Datas
{
    internal class RoundStatesHandler
    {
        public LinkedList<RoundState> List = new LinkedList<RoundState>();
        public LinkedListNode<RoundState> Current;
        private TDSTimer? _nextTimer;

        public RoundStatesHandler(RoundFightLobby lobby)
        {
            var node = List.AddFirst(new NewMapChooseState(lobby));
            node = List.AddAfter(node, new CountdownState(lobby));
            node = List.AddAfter(node, new InRoundState(lobby));
            node = List.AddAfter(node, new RoundEndState(lobby));
            List.AddAfter(node, new RoundEndRankingState(lobby));
            List.AddLast(new MapClearState(lobby));

            Current = List.Last!;
        }

        public void SetNext()
        {
            _nextTimer?.Kill();
            Current = Current == List.Last ? List.First! : Current.Next!;
            Current.Value.SetCurrent();

            _nextTimer = new TDSTimer(SetNext, (uint)Current.Value.Duration);
        }

        public void Start()
        {
            Task.Run(SetNext);
        }

        public void Stop()
        {
            _nextTimer?.Kill();
            Current = List.Last!;
            Current.Value.SetCurrent();
        }
    }
}
