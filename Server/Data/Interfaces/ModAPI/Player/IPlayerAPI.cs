namespace TDS_Server.Data.Interfaces.ModAPI.Player
{
    #nullable enable 
    public interface IPlayerAPI
    {
        IPlayer? GetPlayerByName(string name);

        void SetHealth(IPlayer player, int health);
    }
}
