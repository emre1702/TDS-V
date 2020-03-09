namespace TDS_Server.Data.Interfaces.ModAPI.Player
{
    public interface IPlayerAPI
    {
        IPlayer GetPlayerByName(string name);

        void SetHealth(IPlayer player, int health);
    }
}
