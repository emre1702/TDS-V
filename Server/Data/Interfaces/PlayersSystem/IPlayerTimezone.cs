using System;
using TDS_Server.Data.Abstracts.Entities.GTA;

namespace TDS_Server.Data.Interfaces.PlayersSystem
{
    public interface IPlayerTimezone
    {
        DateTime GetLocalDateTime(DateTime dateTime);

        string GetLocalDateTimeString(DateTime dateTime);

        void Init(ITDSPlayer player, IPlayerEvents events);
    }
}
