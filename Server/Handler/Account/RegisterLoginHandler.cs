using System;
using System.Linq;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Models;
using TDS.Server.Handler.Events;
using TDS.Server.Handler.Helper;
using TDS.Shared.Core;
using TDS.Shared.Default;

namespace TDS.Server.Handler.Account
{
    public class RegisterLoginHandler
    {
        private readonly DatabasePlayerHelper _databasePlayerHelper;

        public RegisterLoginHandler(RemoteBrowserEventsHandler remoteBrowserEventsHandler, DatabasePlayerHelper databasePlayerHelper)
        {
            _databasePlayerHelper = databasePlayerHelper;

            remoteBrowserEventsHandler.AddAsyncEvent(ToServerEvent.LoadRegisterLoginInitData, LoadRegisterLoginInitData);
        }

        private async Task<object?> LoadRegisterLoginInitData(ITDSPlayer player, ArraySegment<object> _)
        {
            var idName = await _databasePlayerHelper.GetPlayerIdName(player);

            if (idName is null)
                return Serializer.ToBrowser(new RegisterLoginInitData { IsRegistered = false, Name = player.Name });
            return Serializer.ToBrowser(new RegisterLoginInitData { IsRegistered = true, Name = idName.Name });
        }
    }
}
