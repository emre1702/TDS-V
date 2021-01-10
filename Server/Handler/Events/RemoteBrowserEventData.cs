using System;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Models;
using static TDS.Server.Handler.Events.RemoteBrowserEventsHandler;

namespace TDS.Server.Handler.Events
{
    public class RemoteBrowserEventData
    {
        private readonly FromBrowserAsyncMethodDelegate? _asyncMethod;
        private readonly FromBrowserMaybeAsyncMethodDelegate? _maybeAsyncMethod;
        private readonly FromBrowserMethodDelegate? _syncMethod;
        private readonly Func<ITDSPlayer, bool>? _condition;

        public RemoteBrowserEventData(FromBrowserAsyncMethodDelegate method, Func<ITDSPlayer, bool>? condition)
            => (_asyncMethod, _condition) = (method, condition);

        public RemoteBrowserEventData(FromBrowserMaybeAsyncMethodDelegate method, Func<ITDSPlayer, bool>? condition)
            => (_maybeAsyncMethod, _condition) = (method, condition);

        public RemoteBrowserEventData(FromBrowserMethodDelegate method, Func<ITDSPlayer, bool>? condition)
            => (_syncMethod, _condition) = (method, condition);

        public async ValueTask<object?> TryExecute(RemoteBrowserEventArgs args)
        {
            if (_condition?.Invoke(args.Player) == false)
                return null;
            if (_asyncMethod is { })
                return await _asyncMethod(args);
            if (_maybeAsyncMethod is { })
                return await _maybeAsyncMethod(args);
            if (_syncMethod is { })
                return _syncMethod(args);

            return null;
        }

        public bool Has(FromBrowserAsyncMethodDelegate method) => _asyncMethod == method;

        public bool Has(FromBrowserMaybeAsyncMethodDelegate method) => _maybeAsyncMethod == method;

        public bool Has(FromBrowserMethodDelegate method) => _syncMethod == method;
    }
}