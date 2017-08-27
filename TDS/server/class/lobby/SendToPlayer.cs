using GrandTheftMultiplayer.Server.API;

namespace Class {

	partial class Lobby {

		public void SendAllPlayerEvent ( string eventName, int teamindex = -1, params object[] args ) {
			if ( teamindex == -1 ) {
				for ( int i = 0; i < this.players.Count; i++ )
					for ( int j = this.players[i].Count - 1; j >= 0; j-- )
						if ( this.players[i][j].exists )
							this.players[i][j].triggerEvent ( eventName, args );

			} else
				for ( int j = 0; j < this.players[teamindex].Count; j++ )
					if ( this.players[teamindex][j].exists )
						this.players[teamindex][j].triggerEvent ( eventName, args );
		}

		public void SendAllPlayerLangNotification ( string langstr, int teamindex = -1, params string[] args ) {
			if ( teamindex == -1 ) { 
				for ( int i = 0; i < this.players.Count; i++ )
					for ( int j = 0; j < this.players[i].Count; j++ )
						if ( this.players[i][j].exists )
							this.players[i][j].SendLangNotification ( langstr, args );
			} else
				for ( int j = 0; j < this.players[teamindex].Count; j++ )
					if ( this.players[teamindex][j].exists )
						this.players[teamindex][j].SendLangNotification ( langstr, args );
		}

		public void SendAllPlayerChatMessage ( string message, int teamindex = -1 ) {
			if ( teamindex == -1 ) { 
				for ( int i = 0; i < this.players.Count; i++ )
					for ( int j = 0; j < this.players[i].Count; j++ )
						if ( this.players[i][j].exists )
							this.players[i][j].sendChatMessage ( message );
			} else
				for ( int j = 0; j < this.players[teamindex].Count; j++ )
					if ( this.players[teamindex][j].exists )
						this.players[teamindex][j].sendChatMessage ( message );
		}
	}
}