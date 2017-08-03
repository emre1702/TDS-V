using System;
using System.Collections.Generic;
using GrandTheftMultiplayer.Server.API;

namespace Class {

	class Timer {

		private static List<Timer> timer = new List<Timer> ();
		private static List<Timer> insertAfterList = new List<Timer> ();

		private Action func;
		private int executeAfterMs;
		private int executeAtMs;
		private int executesLeft;
		public bool isRunning = true;

		public static void TimerOnStart ( API api ) {
			api.onUpdate += OnUpdateFunc;
		}

		Timer ( Action thefunc, int executeafterms, int executeatms, int executes ) {
			this.func = thefunc;
			this.executeAfterMs = executeafterms;
			this.executeAtMs = executeatms;
			this.executesLeft = executes;
		}


		public static Timer SetTimer ( Action thefunc, int executeafterms, int executes = 1 ) {
			int executeatms = executeafterms + Environment.TickCount;
			Timer thetimer = new Timer ( thefunc, executeafterms, executeatms, executes );
			insertAfterList.Add ( thetimer );
			return thetimer;
		}

		public void Kill ( ) {
			this.isRunning = false;
		}


		public void Execute () {
			try {
				this.func ();
			} catch ( Exception e ) {
				API.shared.consoleOutput ( e.ToString() );
			} finally {
				if ( this.executesLeft == 1 ) {
					this.executesLeft = 0;
					this.isRunning = false;
				} else {
					if ( this.executesLeft != -1 )
						this.executesLeft--;
					this.executeAtMs += this.executeAfterMs;
					insertAfterList.Add ( this );
				}
			}
		}


		private void InsertSorted ( ) {
			bool putin = false;
			for ( int i = timer.Count - 1; i >= 0 && !putin; i-- )
				if ( this.executeAtMs <= timer[i].executeAtMs ) {
					timer.Insert ( i+1, this );
					putin = true;
				}

			if ( !putin )
				timer.Insert ( 0, this );
		}


		private static void OnUpdateFunc ( ) {
			int tick = Environment.TickCount;
			for ( int i = timer.Count - 1; i >= 0; i-- ) {
				if ( timer[i].isRunning ) {
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
				for ( int j = 0; j < insertAfterList.Count; j++ ) {
					insertAfterList[j].InsertSorted ();
				}
				insertAfterList = new List<Timer> ();
			}
		}
	}

	/* Beispiel: 

		// Ja, die Funktion DARF private sein //
		private void testTimerFunc ( Client player, string text ) {
			API.shared.shared.sendChatMessageToPlayer ( player, "[TIMER] "+text );
		}

		void testTimerFunc ( ) {
			API.shared.shared.sendChatMessageToAll ( "[TIMER2] Hallo" );
		}

		[Command("ttimer")]
		public void timerTesting ( Client player ) {
			// Lamda für Funktion mit Parameter //
			cTimer.setTimer ( () => testTimerFunc ( player, "hi" ), 1000, 1 );
			// Geht auch normal, dann aber Funktion ohne Parameter //
			cTimer.setTimer ( testTimerFunc, 1000, 1 );
			// Geht auch ohne schon vorhandene Funktion //
			cTimer.setTimer ( () => { API.shared.shared.sendChatMessageToPlayer ( player, "[TIMER] Hi du Ei" ); }, 1000, 1 );
			// Unendlich //
			cTimer.setTimer ( () => { API.shared.shared.sendChatMessageToPlayer ( player, "[TIMER] Hi du Ei" ); }, 1000, -1 );
			// Auch Unendlich //
			cTimer.setTimer ( () => { API.shared.shared.sendChatMessageToPlayer ( player, "[TIMER] Hi du Ei" ); }, 1000, 0 );
		}
	*/
}
