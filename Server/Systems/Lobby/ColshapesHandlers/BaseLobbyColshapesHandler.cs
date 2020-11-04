using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.LobbySystem.Colshapes;

namespace TDS_Server.LobbySystem.ColshapesHandlers
{
    public class BaseLobbyColshapesHandler : IBaseLobbyColshapesHandler
    {
        public virtual void OnPlayerEnterColshape(ITDSColshape colshape, ITDSPlayer player)
        {
        }
    }
}
