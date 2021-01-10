using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Extensions;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Models;
using TDS.Server.Handler.Extensions;
using TDS.Shared.Default;

namespace TDS.Server.Handler.Events
{
    public class RemoteBrowserEventsHandler
    {
        private readonly Dictionary<string, List<RemoteBrowserEventData>> _eventHandlers = new();

        public RemoteBrowserEventsHandler()
        {
            NAPI.ClientEvent.Register<ITDSPlayer, object[]>(ToServerEvent.FromBrowserEvent, this, OnFromBrowserEvent);
            NAPI.ClientEvent.Register<ITDSPlayer, object[]>(ToServerEvent.FromBrowserEventCallback, this, OnFromBrowserEventCallback);
        }

        public delegate Task<object?> FromBrowserAsyncMethodDelegate(RemoteBrowserEventArgs args);

        public delegate ValueTask<object?> FromBrowserMaybeAsyncMethodDelegate(RemoteBrowserEventArgs args);

        public delegate object? FromBrowserMethodDelegate(RemoteBrowserEventArgs args);

        private async void OnFromBrowserEvent(ITDSPlayer player, params object[] args)
        {
            if (player is null)
                return;
            var eventName = (string)args[0];
            var ret = await OnFromBrowserEventMethod(player, args);
            if (ret is { })
                NAPI.Task.RunSafe(() => player.TriggerEvent(ToClientEvent.FromBrowserEventReturn, eventName, ret));
        }

        private async void OnFromBrowserEventCallback(ITDSPlayer player, params object[] args)
        {
            if (player is null)
                return;
            var eventName = (string)args[0];
            var ret = await OnFromBrowserEventMethod(player, args);
            NAPI.Task.RunSafe(() => player.TriggerEvent(ToClientEvent.FromBrowserEventReturn, eventName, ret ?? ""));
        }

        private async Task<object?> OnFromBrowserEventMethod(ITDSPlayer player, params object[] args)
        {
            try
            {
                await Task.Yield();
                var eventName = (string)args[0];
                List<RemoteBrowserEventData>? methods;
                lock (_eventHandlers)
                {
                    if (!_eventHandlers.TryGetValue(eventName, out methods))
                        return null;
                }
                var eventArgs = new RemoteBrowserEventArgs(player, args);

                object? ret = null;
                foreach (var eventHandler in methods)
                    ret ??= await eventHandler.TryExecute(eventArgs);

                return ret;
            }
            catch (Exception ex)
            {
                var baseEx = ex.GetBaseException();
                LoggingHandler.Instance?.LogError(baseEx.Message + "\n"
                    + string.Join('\n', args.Select(a => Convert.ToString(a)?.Substring(0, Math.Min(Convert.ToString(a)?.Length ?? 0, 20)) ?? "-")),
                    ex.StackTrace ?? Environment.StackTrace, ex.GetType().Name + "|" + baseEx.GetType().Name, player);
                return null;
            }
        }

        public void Add(string eventName, FromBrowserMethodDelegate method, Func<ITDSPlayer, bool>? condition = null)
            => Add(eventName, new RemoteBrowserEventData(method, condition));

        public void Add(string eventName, FromBrowserAsyncMethodDelegate method, Func<ITDSPlayer, bool>? condition = null)
            => Add(eventName, new RemoteBrowserEventData(method, condition));

        public void Add(string eventName, FromBrowserMaybeAsyncMethodDelegate method, Func<ITDSPlayer, bool>? condition = null)
            => Add(eventName, new RemoteBrowserEventData(method, condition));

        public void Add(string eventName, RemoteBrowserEventData data)
        {
            lock (_eventHandlers)
            {
                if (!_eventHandlers.TryGetValue(eventName, out var methods))
                {
                    methods = new();
                    _eventHandlers[eventName] = methods;
                }
                methods.Add(data);
            }
        }

        public void Remove(string eventName, FromBrowserMethodDelegate method)
        {
            lock (_eventHandlers)
            {
                if (!_eventHandlers.TryGetValue(eventName, out var methods))
                    return;
                methods.RemoveAll(m => m.Has(method));
            }
        }

        public void Remove(string eventName, FromBrowserAsyncMethodDelegate method)
        {
            lock (_eventHandlers)
            {
                if (!_eventHandlers.TryGetValue(eventName, out var methods))
                    return;
                methods.RemoveAll(m => m.Has(method));
            }
        }

        public void Remove(string eventName, FromBrowserMaybeAsyncMethodDelegate method)
        {
            lock (_eventHandlers)
            {
                if (!_eventHandlers.TryGetValue(eventName, out var methods))
                    return;
                methods.RemoveAll(m => m.Has(method));
            }
        }
    }
}