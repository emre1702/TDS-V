using TDS_Server.Data.Models.GTA;

namespace TDS_Server.Data.Interfaces.ModAPI.Player
{
    public interface IPlayer
    {
        string Name { get; }
        ulong SocialClubId { get; }
        string SocialClubName { get; }
        Position3D Position { get; set; }
        ushort RemoteId { get; }
        int Transparency { get; set; }

        void SendEvent(string eventName, params object[] args);
        void SetHealth(int health);

    }
}
