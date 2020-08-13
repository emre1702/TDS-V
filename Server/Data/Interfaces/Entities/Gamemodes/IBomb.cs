namespace TDS_Server.Data.Interfaces.Entities.Gamemodes
{
    public interface IBomb : IGamemode
    {
        bool StartBombDefusing(ITDSPlayer player);
        bool StartBombPlanting(ITDSPlayer player);
        void StopBombDefusing(ITDSPlayer player);
        void StopBombPlanting(ITDSPlayer player);
    }
}
