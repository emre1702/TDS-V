using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Enums;
using TDS_Server.Database.Entity.GangEntities;

namespace TDS_Server.Data.Interfaces
{
#nullable enable

    public interface IGang : IDatabaseEntityWrapper
    {
        Gangs Entity { get; set; }
        ITeam GangLobbyTeam { get; set; }
        IGangHouse? House { get; set; }
        bool InAction { get; set; }
        bool Initialized { get; set; }
        List<ITDSPlayer> PlayersOnline { get; }
        Vector3? SpawnPosition { get; }
        float? SpawnHeading { get; }

        void FuncIterate(Action<ITDSPlayer> player);

        void SendMessage(Func<ILanguage, string> langGetter);

        void SendNotification(Func<ILanguage, string> langGetter);

        void AppointNextSuitableLeader();

        Task Delete();

        bool IsAllowedTo(ITDSPlayer player, GangCommand type);
    }
}
