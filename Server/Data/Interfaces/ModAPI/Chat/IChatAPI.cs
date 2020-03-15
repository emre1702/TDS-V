using System.Collections.Generic;

namespace TDS_Server.Data.Interfaces.ModAPI.Chat
{
    #nullable enable
    public interface IChatAPI
    {
        public void SendMessage(string message, ICollection<int>? ignorePlayersWithId = null);

        public void SendMessage(ILobby lobby, string message, ICollection<int>? ignorePlayersWithId = null);

        public void SendMessage(ITeam lobby, string message, ICollection<int>? ignorePlayersWithId = null);
    }
}
