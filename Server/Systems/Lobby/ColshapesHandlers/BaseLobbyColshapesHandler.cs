using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.LobbySystem.Colshapes;

namespace TDS.Server.LobbySystem.ColshapesHandlers
{
    public class BaseLobbyColshapesHandler : IBaseLobbyColshapesHandler
    {
        public virtual void OnPlayerEnterColshape(ITDSColshape colshape, ITDSPlayer player)
        {
        }
    }
}
