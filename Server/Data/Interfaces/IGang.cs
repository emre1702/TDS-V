using System;
using System.Collections.Generic;
using System.Text;
using TDS_Server.Database.Entity.GangEntities;

namespace TDS_Server.Data.Interfaces
{
    #nullable enable
    public interface IGang
    {
        Gangs Entity { get; set; }
        bool InAction { get; set; }
        List<ITDSPlayer> PlayersOnline { get; }

        void SendNotification(Func<ILanguage, string> langGetter);
        void SendMessage(Func<ILanguage, string> langGetter);
        void FuncIterate(Action<ITDSPlayer> player);
    }
}
