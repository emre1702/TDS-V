using System.Collections.Generic;
using TDS_Server.Data.Abstracts.Entities.GTA;

namespace TDS_Server.Data.Interfaces.PlayersSystem
{
#nullable enable

    public interface IPlayerSpectateHandler
    {
        void AddSpectator(ITDSPlayer spectator);

        List<ITDSPlayer> GetSpectators();

        bool HasSpectators();

        void Init(ITDSPlayer player);

        void RemoveSpectator(ITDSPlayer spectator);

        void SetSpectates(ITDSPlayer? target);
    }
}
