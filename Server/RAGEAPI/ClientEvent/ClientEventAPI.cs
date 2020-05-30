using GTANetworkAPI;
using System;
using System.Reflection;
using TDS_Server.Data.Interfaces.ModAPI.ClientEvent;

namespace TDS_Server.RAGEAPI.ClientEvent
{
    internal class ClientEventAPI : IClientEventAPI
    {
        #region Public Methods

        public void Add(string eventName, object classInstance, MethodInfo methodInfo)
        {
            NAPI.ClientEvent.Register(methodInfo, eventName, classInstance);
        }

        #endregion Public Methods
    }
}
