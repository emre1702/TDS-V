using System;

namespace TDS.Server.Data.Models
{
#nullable enable

    public class WaitDisposable : IDisposable
    {
        private bool disposedValue;

        private readonly Action _onDispose;

        public WaitDisposable(Action onDispose)
        {
            _onDispose = onDispose;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                _onDispose();
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
