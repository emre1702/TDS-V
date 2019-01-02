namespace TDS_Common.Instance.Utility
{

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// Timer-class
    /// </summary>
    public class TDSTimer
    {

        /// <summary>A sorted List of Timers</summary>
        private static readonly List<TDSTimer> timer = new List<TDSTimer>();
        /// <summary>List used to put the Timers in timer-List after the possible List-iteration</summary>
        private static readonly List<TDSTimer> insertAfterList = new List<TDSTimer>();
        /// <summary>Stopwatch to get the tick counts (Environment.TickCount is only int)</summary>
        private static Action<string> logger;
        private static Func<ulong> tickGetter;

        /// <summary>The Action getting called by the Timer. Can be changed dynamically.</summary>
        public Action Func;
        /// <summary>After how many milliseconds (after the last execution) the timer should get called. Can be changed dynamically</summary>
        public readonly uint ExecuteAfterMs;
        /// <summary>When the Timer is ready to execute (Stopwatch is used).</summary>
        private ulong executeAtMs;
        /// <summary>How many executes the timer has left - use 0 for infinitely. Can be changed dynamically</summary>
        public uint ExecutesLeft;
        /// <summary>If the Timer should handle exceptions with a try-catch-finally. Can be changed dynamically</summary>
        public bool HandleException;
        /// <summary>If the Timer will get removed.</summary>
        private bool willRemoved = false;
        /// <summary>Use this to check if the timer is still running.</summary>
        public bool IsRunning => !willRemoved;
        /// <summary>The remaining ms to execute</summary>
        public ulong RemainingMsToExecute => executeAtMs - tickGetter();

        /// <summary>
        /// Needs to be used once at server and once and client
        /// </summary>
        public static void Init(Action<string> thelogger, Func<ulong> theTickGetter)
        {
            logger = thelogger;
            tickGetter = theTickGetter;
        }

        /// <summary>
        /// Use this constructor to create the Timer.
        /// </summary>
        /// <param name="thefunc">The Action which you want to get called.</param>
        /// <param name="executeafterms">Execute after milliseconds.</param>
        /// <param name="executes">Amount of executes. Use 0 for infinitely.</param>
        /// <param name="handleexception">If try-catch-finally should be used when calling the Action</param>
        public TDSTimer(Action thefunc, uint executeafterms, uint executes = 1, bool handleexception = false)
        {
            ulong executeatms = executeafterms + tickGetter();
            Func = thefunc;
            ExecuteAfterMs = executeafterms;
            executeAtMs = executeatms;
            ExecutesLeft = executes;
            HandleException = handleexception;
            insertAfterList.Add(this);   // Needed to put in the timer later, else it could break the script when the timer gets created from a Action of another timer.
        }

        /// <summary>
        /// Use this method to stop the Timer.
        /// </summary>
        public void Kill()
        {
            willRemoved = true;
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
                willRemoved = true;
            }
            else
            {
                if (ExecutesLeft != 0)
                    ExecutesLeft--;
                executeAtMs += ExecuteAfterMs;
                insertAfterList.Add(this);
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
                logger?.Invoke(ex.ToString());
            }
            finally
            {
                if (ExecutesLeft == 1)
                {
                    ExecutesLeft = 0;
                    willRemoved = true;
                }
                else
                {
                    if (ExecutesLeft != 0)
                        ExecutesLeft--;
                    executeAtMs += ExecuteAfterMs;
                    insertAfterList.Add(this);
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
                executeAtMs = tickGetter();
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
            for (int i = timer.Count - 1; i >= 0 && !putin; i--)
                if (executeAtMs <= timer[i].executeAtMs)
                {
                    timer.Insert(i + 1, this);
                    putin = true;
                }

            if (!putin)
                timer.Insert(0, this);
        }

        /// <summary>
        /// Iterate the timers and call the Action of the ready/finished ones.
        /// If IsRunning is false, the timer gets removed/killed.
        /// Because the timer-List is sorted, the iteration stops when a timer is not ready yet, cause then the others won't be ready, too.
        /// </summary>
        public static void OnUpdateFunc()
        {
            ulong tick = tickGetter();
            for (int i = timer.Count - 1; i >= 0; i--)
            {
                if (!timer[i].willRemoved)
                {
                    if (timer[i].executeAtMs <= tick)
                    {
                        TDSTimer thetimer = timer[i];
                        timer.RemoveAt(i);   // Remove the timer from the list (because of sorting and executeAtMs will get changed)
                        if (thetimer.HandleException)
                            thetimer.ExecuteMeSafe();
                        else
                            thetimer.ExecuteMe();
                    }
                    else
                        break;
                }
                else
                    timer.RemoveAt(i);
            }

            // Put the timers back in the list
            if (insertAfterList.Count > 0)
            {
                foreach (TDSTimer timer in insertAfterList)
                {
                    timer.InsertSorted();
                }
                insertAfterList.Clear();
            }
        }
    }
}

/* Examples: 

	// Yes, the method can be private //
	private void testTimerFunc ( Client player, string text ) {
		NAPI.Chat.SendChatMessageToPlayer ( player, "[TIMER] "+text );
	}

	void testTimerFunc ( ) {
		NAPI.Chat.SendChatMessageToAll ( "[TIMER2] Hello" );
	}

	[Command("ttimer")]
	public void timerTesting ( Client player ) {
		// Lamda for parameter //
		Timer.SetTimer ( () => testTimerFunc ( player, "hi" ), 1000, 1 );
		// Normal without parameters //
		Timer.SetTimer ( testTimerFunc, 1000, 1 );
		// Without existing method //
		Timer.SetTimer ( () => { NAPI.Chat.SendChatMessageToPlayer ( player, "[TIMER3] Bonus is da best" ); }, 1000, 0 );
	}
*/
