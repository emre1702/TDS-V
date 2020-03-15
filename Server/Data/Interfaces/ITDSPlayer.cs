using System;
using TDS_Server.Data.Interfaces.ModAPI.Player;
using TDS_Server.Data.Models;
using TDS_Server.Database.Entity.Player;

namespace TDS_Server.Data.Interfaces
{
#nullable enable

    public interface ITDSPlayer : IEquatable<ITDSPlayer>
    {
        AdminLevelDto AdminLevel { get; }
        RoundStatsDto? CurrentRoundStats { get; set; }
        string DisplayName { get; }
        Players? Entity { get; }
        int Id { get; }
        bool IsVip { get; }
        ILanguage Language { get; }
        ILobby? Lobby { get; set; }
        IPlayer? ModPlayer { get; }
        ushort RemoteId { get; }
        ITeam? Team { get; }

        void SendBrowserEvent(string eventName, params object[] args);
        void SendEvent(string eventName, params object[] args);
        void SendMessage(string msg);
        void SendNotification(string msg, bool flashing = false);
        
    }
}
