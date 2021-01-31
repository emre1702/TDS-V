using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BonusBotConnector.Client
{
    public class ActionHandler : IDisposable
    {
        private const int _maxFailsPerAction = 5;

        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private readonly Queue<Func<Task>> _actionQueue = new Queue<Func<Task>>();
        private int _failCount = 0;
        private DateTime? _cooldownUntil;

        private readonly Action<Exception> _errorLogger;

        public ActionHandler(Action<Exception> errorLogger)
        {
            _errorLogger = errorLogger;
        }

        public async void DoAction(Func<Task> action, bool useErrorLogger = true)
        {
            try
            {
                await _semaphore.WaitAsync();
                _actionQueue.Enqueue(action);

                if (CheckInCooldown()) return;
                while (_actionQueue.Count > 0)
                    await ExecuteFirstInQueue();
            }
            catch (RpcException rpcException) when (rpcException.StatusCode == StatusCode.DeadlineExceeded || rpcException.StatusCode == StatusCode.Internal)
            {
                HandleRpcOrHttpException();
            }
            catch (HttpRequestException)
            {
                HandleRpcOrHttpException();
            }
            catch (Exception ex)
            {
                if (useErrorLogger)
                    _errorLogger.Invoke(ex);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private async Task ExecuteFirstInQueue()
        {
            var actionInQueue = _actionQueue.Peek();
            await actionInQueue();
            _actionQueue.Dequeue();
            _failCount = 0;
        }

        private void HandleRpcOrHttpException()
        {
            if (++_failCount > _maxFailsPerAction)
            {
                _actionQueue.Dequeue();
                _failCount = 0;
            }
            else
            {
                _cooldownUntil = DateTime.Now.AddSeconds(_failCount * _failCount * _failCount);
            }
        }

        private bool CheckInCooldown()
        {
            if (!_cooldownUntil.HasValue)
                return false;

            if (_cooldownUntil.Value < DateTime.Now)
            {
                _cooldownUntil = null;
                return false;
            }

            return true;
        }

        public void Dispose()
        {
            _actionQueue.Clear();
            _semaphore.Dispose();
        }
    }
}