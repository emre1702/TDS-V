namespace TDS_Server.Data.Interfaces.ModAPI.Player
{
    public interface IPlayer
    {
        string Name { get; }
        ulong SocialClubId { get; }
        string SocialClubName { get; }

        void SetHealth(int health);
    }
}
