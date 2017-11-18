namespace TDS.server.instance.utility {

	using System;
	using System.Collections.Generic;
	using GTANetworkAPI;
	using manager.logs;

	class Timer : Script {

		private static readonly List<Timer> timer = new List<Timer> ();
		private static List<Timer> insertAfterList = new List<Timer> ();

		private readonly Action func;
		private readonly uint executeAfterMs;
		private uint executeAtMs;
		private int executesLeft;
		public bool IsRunning = true;

		public Timer () {
			this.API.OnUpdate += OnUpdateFunc;
		}

		Timer ( Action thefunc, uint executeafterms, uint executeatms, int executes ) {
			this.func = thefunc;
			this.executeAfterMs = executeafterms;
			this.executeAtMs = executeatms;
			this.executesLeft = executes;
		}


		public static Timer SetTimer ( Action thefunc, uint executeafterms, int executes = 1 ) {
			uint executeatms = executeafterms + (uint) Environment.TickCount;
			Timer thetimer = new Timer ( thefunc, executeafterms, executeatms, executes );
			insertAfterList.Add ( thetimer );
			return thetimer;
		}

		public void Kill () {
			this.IsRunning = false;
		}


		public void Execute () {
			try {
				this.func ();
			} catch ( Exception e ) {
				Log.Error ( "Error in Execute in Timer.cs: " + e );
			} finally {
				if ( this.executesLeft == 1 ) {
					this.executesLeft = 0;
					this.IsRunning = false;
				} else {
					if ( this.executesLeft != -1 )
						this.executesLeft--;
					this.executeAtMs += this.executeAfterMs;
					insertAfterList.Add ( this );
				}
			}
		}


		private void InsertSorted () {
			bool putin = false;
			for ( int i = timer.Count - 1; i >= 0 && !putin; i-- )
				if ( this.executeAtMs <= timer[i].executeAtMs ) {
					timer.Insert ( i + 1, this );
					putin = true;
				}

			if ( !putin )
				timer.Insert ( 0, this );
		}


		private static void OnUpdateFunc () {
			uint tick = (uint) Environment.TickCount;
			for ( int i = timer.Count - 1; i >= 0; i-- ) {
				if ( timer[i].IsRunning ) {
					if ( timer[i].executeAtMs <= tick ) {
						Timer thetimer = timer[i];
						timer.RemoveAt ( i );
						thetimer.Execute ();
					} else
						break;
				} else
					timer.RemoveAt ( i );
			}

			if ( insertAfterList.Count > 0 ) {
				foreach ( Timer timer in insertAfterList ) {
					timer.InsertSorted ();
				}
				insertAfterList = new List<Timer> ();
			}
		}
	}

	/* Beispiel: 

		// Ja, die Funktion DARF private sein //
		private void testTimerFunc ( Client player, string text ) {
			API.sendChatMessageToPlayer ( player, "[TIMER] "+text );
		}

		void testTimerFunc ( ) {
			API.sendChatMessageToAll ( "[TIMER2] Hallo" );
		}

		[Command("ttimer")]
		public void timerTesting ( Client player ) {
			// Lamda für Funktion mit Parameter //
			cTimer.setTimer ( () => testTimerFunc ( player, "hi" ), 1000, 1 );
			// Geht auch normal, dann aber Funktion ohne Parameter //
			cTimer.setTimer ( testTimerFunc, 1000, 1 );
			// Geht auch ohne schon vorhandene Funktion //
			cTimer.setTimer ( () => { API.sendChatMessageToPlayer ( player, "[TIMER] Hi du Ei" ); }, 1000, 1 );
			// Unendlich //
			cTimer.setTimer ( () => { API.sendChatMessageToPlayer ( player, "[TIMER] Hi du Ei" ); }, 1000, -1 );
			// Auch Unendlich //
			cTimer.setTimer ( () => { API.sendChatMessageToPlayer ( player, "[TIMER] Hi du Ei" ); }, 1000, 0 );
		}
	*/

}
