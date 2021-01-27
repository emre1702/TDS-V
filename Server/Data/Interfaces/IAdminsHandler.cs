using System;
using System.Collections.Generic;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Models;

namespace TDS.Server.Data.Interfaces
{
#nullable enable

    public interface IAdminsHandler
    {
        AdminLevelDto HighestLevel { get; }
        AdminLevelDto LowestLevel { get; }

        void CallMethodForAdmins(Action<ITDSPlayer> func, byte minadminlvl = 1);

        List<ITDSPlayer> GetAllAdmins();

        List<ITDSPlayer> GetAllAdminsSorted();

        AdminLevelDto GetLevel(short adminLvl);

        void SendMessage(Func<ILanguage, string> propertygetter, byte minadminlvl = 1);

        void SendMessage(string msg, byte minadminlvl = 1);

        void SendNotification(Func<ILanguage, string> propertygetter, byte minadminlvl = 1);
    }
}
}