using System;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Models.GTA;

namespace TDS_Server.Data.Interfaces.ModAPI.Player
{
    public interface IPlayer : IEquatable<IPlayer>
    {
        bool Dead { get; }
        string IPAddress { get; }
        string Name { get; set; }
        Position3D Position { get; set; }
        ushort RemoteId { get; }
        string Serial { get; }
        ulong SocialClubId { get; }
        string SocialClubName { get; }
        int Transparency { get; set; }

        void Kick(string reason);
        void SendEvent(string eventName, params object[] args);
        void SetHealth(int health);
        void SetSkin(PedHash pedHash);
        void SetVoiceTo(ITDSPlayer target, bool on);
        void Spawn(Position3D position, float heading = 0);
        
    }
}
