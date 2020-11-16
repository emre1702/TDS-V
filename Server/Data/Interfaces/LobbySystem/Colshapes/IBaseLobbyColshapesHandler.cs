using TDS.Server.Data.Abstracts.Entities.GTA;

namespace TDS.Server.Data.Interfaces.LobbySystem.Colshapes
{
    public interface IBaseLobbyColshapesHandler
    {
        void OnPlayerEnterColshape(ITDSColshape colshape, ITDSPlayer player);
    }
}
