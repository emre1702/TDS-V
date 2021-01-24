using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BonusBotConnector.Client
{
    public class ActionHandler
    {
        private readonly Queue<Func<Task>> _actionQueue = new Queue<Func<Task>>();
        private int _failCount = 0;
        private readonly Dictionary<Func<Task>, int> _actionFailedAmount = new Dictionary<Func<Task>, int>();
        private const int _maxFailsPerAction = 5;
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
                if (!CheckInCooldown())
                    await ExecuteAction(action);
                else
                    _actionQueue.Enqueue(action);
            }
            catch (RpcException rpcException) when (rpcException.StatusCode == StatusCode.DeadlineExceeded || rpcException.StatusCode == StatusCode.Internal)
            {
                HandleRpcOrHttpException(action);
            }
            catch (HttpRequestException)
            {
                HandleRpcOrHttpException(action);
            }
            catch (Exception ex)
            {
                if (useErrorLogger)
                    _errorLogger.Invoke(ex);
            }
            finally
            {
                HandleDoActionFinally();
            }
        }

        private async ValueTask ExecuteAction(Func<Task> action)
        {
            if (_actionQueue.Count > 0)
            {
                _actionQueue.Enqueue(action);
                return;
            }
            await action();
            _failCount = 0;
            if (_actionFailedAmount.ContainsKey(action))
                _actionFailedAmount.Remove(action);
        }

        private void HandleRpcOrHttpException(Func<Task> action)
        {
            ++_failCount;
            if (!_actionFailedAmount.TryGetValue(action, out int amountFailed))
                _actionFailedAmount[action] = 0;
            ++_actionFailedAmount[action];

            if (_actionFailedAmount[action] < _maxFailsPerAction)
                _actionQueue.Enqueue(action);
        }

        private void HandleDoActionFinally()
        {
            if (_failCount == 0 && _actionQueue.Count > 0)
                ExecuteAllInQueue();

            if (_failCount > 0 && !_cooldownUntil.HasValue)
                _cooldownUntil = DateTime.Now.AddMinutes(_failCount * _failCount);
        }

        private void ExecuteAllInQueue()
        {
            var queue = _actionQueue.ToList();
            _actionQueue.Clear();
            foreach (var action in queue)
                DoAction(action);
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
    }
}