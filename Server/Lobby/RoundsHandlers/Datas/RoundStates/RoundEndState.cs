﻿using System.Threading.Tasks;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Data.Interfaces.LobbySystem.RoundsHandlers.Datas.RoundStates;

namespace TDS_Server.LobbySystem.RoundsHandlers.Datas.RoundStates
{
    public class RoundEndState : RoundState, IRoundEndState
    {
        public override int Duration => 500;

        public RoundEndState(IRoundFightLobby lobby) : base(lobby)
        {
        }

        public override ValueTask SetCurrent()
            => Lobby.Events.TriggerRoundEnd();
    }
}
