using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TDS.Shared.Core
{
    /// <summary>
    /// Timer-class
    /// </summary>
    public class TDSTimer
    {
        /// <summary>A sorted List of Timers</summary>
        private static readonly List<TDSTimer> _timer = new List<TDSTimer>();

        /// <summary>List used to put the Timers in timer-List after the possible List-iteration</summary>
        private static readonly List<TDSTimer> _insertAfterList = new List<TDSTimer>();

        /// <summary>Logger</summary>
        private static Action<Exception> _logger;

        /// <summary>The Action getting called by the Timer. Can be changed dynamically.</summary>
        public Action Func;

        /// <summary>After how many milliseconds (after the last execution) the timer should get called. Can be changed dynamically</summary>
        public uint ExecuteAfterMs
        {
            get => _executeAfterMs;
            set 
            {
                _executeAfterMs = value;
                _executeAtMs = (uint)_msGetter() + value;
                lock (_timer)
                {
                    _timer.Remove(this);
                    InsertSorted();
                }
            }
        }
        private uint _executeAfterMs;

        /// <summary>When the Timer is ready to execute (Stopwatch is used).</summary>
        private uint _executeAtMs;

        /// <summary>How many executes the timer has left - use 0 for infinitely. Can be changed dynamically</summary>
        public uint ExecutesLeft;

        /// <summary>If the Timer should handle exceptions with a try-catch-finally. Can be changed dynamically</summary>
        public bool HandleException;

        /// <summary>If the Timer will get removed.</summary>
        private bool _willRemoved = false;

        /// <summary>Use this to check if the timer is still running.</summary>
        public bool IsRunning => !_willRemoved;

        public static int ElapsedMs => _msGetter?.Invoke() ?? 0;

        private static Func<int> _msGetter;

        /// <summary>The remaining ms to execute</summary>
        public uint RemainingMsToExecute => _executeAtMs - (uint)_msGetter();
        public uint ElapsedMsSinceLastExecOrCreate => (uint)_msGetter() - (_executeAtMs - _executeAfterMs);

        /// <summary>
        /// Can be used once at server and once at client to define a logger
        /// </summary>
        public static void Init(Action<Exception> thelogger, Func<int> msGetter)
        {
            _logger = thelogger;
            _msGetter = msGetter;
        }

        /// <summary>
        /// Use this constructor to create the Timer.
        /// </summary>
        /// <param name="thefunc">The Action which you want to get called.</param>
        /// <param name="executeafterms">Execute after milliseconds.</param>
        /// <param name="executes">Amount of executes. Use 0 for infinitely.</param>
        /// <param name="handleexception">If try-catch-finally should be used when calling the Action</param>
        public TDSTimer(Action thefunc, uint executeafterms, uint executes = 1, bool handleexception = true)
        {
            uint executeatms = executeafterms + (uint)_msGetter();
            Func = thefunc;
            _executeAfterMs = executeafterms;
            _executeAtMs = executeatms;
            ExecutesLeft = executes;
            HandleException = handleexception;
            lock (_insertAfterList)
            {
                _insertAfterList.Add(this);   // Needed to put in the timer later, else it could break the script when the timer gets created from a Action of another timer.
            }
        }

        /// <summary>
        /// Use this method to stop the Timer.
        /// </summary>
        public void Kill()
        {
            _willRemoved = true;
        }

        /// <summary>
        /// Executes a timer.
        /// </summary>
        private void ExecuteMe()
        {
            Func();
            if (ExecutesLeft == 1)
            {
                ExecutesLeft = 0;
                _willRemoved = true;
            }
            else
            {
                if (ExecutesLeft != 0)
                    ExecutesLeft--;
                _executeAtMs += _executeAfterMs;
                lock (_insertAfterList)
                {
                    _insertAfterList.Add(this);
                }
            }
        }

        /// <summary>
        /// Executes a timer with try-catch-finally.
        /// </summary>
        private void ExecuteMeSafe()
        {
            try
            {
                Func();
            }
            catch (Exception ex)
            {
                _logger?.Invoke(ex);
            }
            finally
            {
                if (ExecutesLeft == 1)
                {
                    ExecutesLeft = 0;
                    _willRemoved = true;
                }
                else
                {
                    if (ExecutesLeft != 0)
                        ExecutesLeft--;
                    _executeAtMs += _executeAfterMs;
                    lock (_insertAfterList)
                    {
                        _insertAfterList.Add(this);
                    }
                }
            }
        }

        /// <summary>
        /// Executes the timer now.
        /// </summary>
        /// <param name="changeexecutems">If the timer should change it's execute-time like it would have been executed now. Use false to ONLY execute it faster this time.</param>
        public void Execute(bool changeexecutems = true)
        {
            if (changeexecutems)
            {
                _executeAtMs = (uint)_msGetter();
            }
            if (HandleException)
                ExecuteMeSafe();
            else
                ExecuteMe();
        }

        /// <summary>
        /// Used to insert the timer back to timer-List with sorting.
        /// </summary>
        private void InsertSorted()
        {
            bool putin = false;
            lock (_timer)
            {
                for (int i = _timer.Count - 1; i >= 0 && !putin; i--)
                    if (_executeAtMs <= _timer[i]._executeAtMs)
                    {
                        _timer.Insert(i + 1, this);
                        putin = true;
                    }

                if (!putin)
                    _timer.Insert(0, this);
            }
        }

        /// <summary>
        /// Call this on every update/tick at clientside/serverside.
        /// Iterates the timers and calls the Action of the ready/finished ones.
        /// If IsRunning is false, the timer gets removed/killed.
        /// Because the timer-List is sorted, the iteration stops when a timer is not ready yet, cause then the others won't be ready, too.
        /// </summary>
        public static void OnUpdateFunc()
        {
            uint elapsedMs = (uint)_msGetter();
            lock (_timer)
            {
                for (int i = _timer.Count - 1; i >= 0; i--)
                {
                    if (_timer[i]._willRemoved)
                    {
                        _timer.RemoveAt(i);
                        continue;
                    }

                    if (_timer[i]._executeAtMs > elapsedMs)
                        break;

                    TDSTimer thetimer = _timer[i];
                    _timer.RemoveAt(i);   // Remove the timer from the list (because of sorting and executeAtMs will get changed)
                    if (thetimer.HandleException)
                        thetimer.ExecuteMeSafe();
                    else
                        thetimer.ExecuteMe();                        
                }
            }

            lock (_insertAfterList)
            {
                // Put the timers back in the list
                if (_insertAfterList.Count > 0)
                {
                    foreach (TDSTimer timer in _insertAfterList)
                    {
                        timer.InsertSorted();
                    }
                    _insertAfterList.Clear();
                }
            }
        }
    }
}
