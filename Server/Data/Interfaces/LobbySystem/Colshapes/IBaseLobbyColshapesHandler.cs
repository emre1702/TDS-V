using TDS_Server.Data.Abstracts.Entities.GTA;

namespace TDS_Server.Data.Interfaces.LobbySystem.Colshapes
{
    public interface IBaseLobbyColshapesHandler
    {
        void OnPlayerEnterColshape(ITDSColshape colshape, ITDSPlayer player);
    }
}
