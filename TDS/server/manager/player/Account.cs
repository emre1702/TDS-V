namespace TDS.server.manager.player {

	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Text;
	using System.Threading.Tasks;
	using database;
	using extend;
	using GTANetworkAPI;
	using GTANetworkInternals;
	using instance.player;
	using logs;
	using TDS.server.enums;
	using TDS.server.instance.utility;
	using utility;

	class Account: Script {

		public static Dictionary<string, uint> PlayerUIDs = new Dictionary<string, uint> { { "", 0 } };
		private static Dictionary<uint, string> playerUIDNames = new Dictionary<uint, string> { { 0, "" } };
		private static readonly Dictionary<string, bool> socialClubNameBanDict = new Dictionary<string, bool>();
		private static readonly Dictionary<string, bool> addressBanDict = new Dictionary<string, bool>();
		private static uint lastPlayerUID;

		public Account () {
		}

		private static void SendWelcomeMessage ( Client player ) {
			StringBuilder builder = new StringBuilder();
			builder.Append( "#o#__________________________________________#w#" );
			for ( int i = 1; i <= 7; i++ ) {
				builder.Append( "#n#" + player.GetLang( "welcome_" + i ) );
			}
			builder.Append( "#n##o#__________________________________________" );
			player.SendChatMessage( builder.ToString() );
		}

		public static string GetNameByUID ( uint uid ) {
			if ( playerUIDNames.ContainsKey( uid ) ) {
				return playerUIDNames[uid];
			} else
				return "-";
		}

		[DisableDefaultOnConnectSpawn]
		[ServerEvent( Event.PlayerConnected )]
		public static void OnPlayerConnected ( Client player ) {
			player.Position = new Vector3( 0, 0, 1000 ).Around( 10 );
			player.Freeze( true );
			player.Name = player.SocialClubName;
			NAPI.ClientEvent.TriggerClientEvent( player, "startRegisterLogin", player.SocialClubName, PlayerUIDs.ContainsKey( player.SocialClubName ) );
		}

		[RemoteEvent( "onPlayerTryRegister" )]
		public void OnPlayerTryRegisterEvent ( Client player, string password, string email ) {
			if ( PlayerUIDs.ContainsKey( player.SocialClubName ) )
				return;
			password = Utility.ConvertToSHA512( password );
			PlayerUIDs[player.SocialClubName] = ++lastPlayerUID;
			Register.RegisterPlayer( player, lastPlayerUID, password, email );
		}

		[RemoteEvent( "onPlayerChatLoad" )]
		public void OnPlayerChatLoadEvent ( Client player, string language, bool hitsoundon ) {
			OnPlayerLanguageChangeEvent( player, language );
			Character character = player.GetChar();
			character.HitsoundOn = hitsoundon;
			SendWelcomeMessage( player );
		}

		[RemoteEvent( "onPlayerTryLogin" )]
		public void OnPlayerTryLoginEvent ( Client player, string password ) {
			if ( PlayerUIDs.ContainsKey( player.SocialClubName ) ) {
				password = Utility.ConvertToSHA512( password );
				Login.LoginPlayer( player.GetChar(), PlayerUIDs[player.SocialClubName], password );
			} else
				player.SendLangNotification( "account_doesnt_exist" );
		}

		[RemoteEvent( "onPlayerLanguageChange" )]
		public void OnPlayerLanguageChangeEvent ( Client player, string language ) {
			player.GetChar().Language = (Language)Enum.Parse( typeof( Language ), language );
		}


		public static void AddAccount ( string name, uint uid ) {
			PlayerUIDs[name] = uid;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Await.Warning", "CS4014:Await.Warning" )]
		[ServerEvent( Event.PlayerConnected )]
		public static async void OnPlayerBeginConnect ( Client player ) {       //TODO it's on connected, not connect anymore 
			try {
				player.Name = player.SocialClubName;
				if ( socialClubNameBanDict.ContainsKey( player.SocialClubName ) || addressBanDict.ContainsKey( player.Address ) ) {
					DataTable result = await Database.ExecPreparedResult( "SELECT * FROM ban WHERE socialclubname = @SCN OR address = @address", new Dictionary<string, string> {
						{
							"@scn", player.SocialClubName
						}, {
							"@address", player.Address
						}
					} ).ConfigureAwait( false );
					if ( result.Rows.Count > 0 ) {
						DataRow row = result.Rows[0];
						if ( row["type"].ToString() == "permanent" ) {
							player.Kick( "You are permanently banned by " + row["admin"] + ". Reason: " + row["reason"] );
							return;
						}
						if ( Convert.ToInt32( row["endsec"] ) > Utility.GetTimespan() ) {
							player.Kick( "You are banned until " + row["endoptic"] + " by " + row["admin"] + ". Reason: " + row["reason"] );
							return;
						}
						Database.Exec( $"DELETE FROM ban WHERE id = {row["id"]};" );
					}
					socialClubNameBanDict.Remove( player.SocialClubName );
					addressBanDict.Remove( player.Address );
				}
			} catch ( Exception ex ) {
				Log.Error( ex.ToString() );
			}
		}

		[ServerEvent( Event.ResourceStart )]
		public static async void OnResourceStart () {
			try {
				DataTable result = await Database.ExecResult( "SELECT uid, name FROM player" ).ConfigureAwait( false );
				foreach ( DataRow row in result.Rows ) {
					string name = row["name"].ToString();
					uint uid = Convert.ToUInt32( row["uid"] );
					PlayerUIDs[name] = uid;
					playerUIDNames[uid] = name;
				}
				DataTable maxuidresult = await Database.ExecResult( "SELECT Max(uid) AS Maxuid FROM player" ).ConfigureAwait( false );
				lastPlayerUID = Convert.ToUInt32( maxuidresult.Rows[0]["Maxuid"] );
			} catch ( Exception ex ) {
				Log.Error( ex.ToString() );
				lastPlayerUID = 0;

			}
		}

		[ServerEvent( Event.PlayerDisconnected )]
		public static void OnPlayerDisconnected ( Client player, DisconnectionType type, string reason ) {
			try {
				Character character = player.GetChar();
				character.SaveData();
				if ( character.AdminLvl > 0 )
					Admin.SetOffline( character );
				//NAPI.ClientEvent.TriggerClientEventForAll ( "onClientPlayerQuit", player.Value );   //TODO NOT USED RIGHT NOW
			} catch ( Exception ex ) {
				Log.Error( ex.ToString() );
			}
		}

		public static void PermaBanPlayer ( Character admincharacter, Client target, string targetname, string targetaddress, string reason ) {
			Client admin = admincharacter.Player;
			Database.ExecPrepared( $"REPLACE INTO ban (socialclubname, address, type, startsec, startoptic, admin, reason) VALUES " +
				$"(@socialclubname, @address, 0, '{Utility.GetTimespan()}', '{Utility.GetTimestamp()}', {admincharacter.UID}, @reason)",
				new Dictionary<string, string> {
					{ "@socialclubname", targetname }, { "@address", targetaddress }, { "@reason", reason } }
			);
			socialClubNameBanDict[targetname] = true;
			if ( targetaddress != "-" )
				addressBanDict[targetaddress] = true;
			ServerLanguage.SendMessageToAll( "permaban", targetname, admin.Name, reason );
			if ( target != null )
				target.Kick( target.GetLang( "youpermaban", admin.Name, reason ) );
			// LOG //
			AdminLog.Log( AdminLogType.PERMABAN, admincharacter.UID, PlayerUIDs[targetname], reason );
			/////////
		}

		public static void TimeBanPlayer ( Character admincharacter, Client target, string targetname, string targetaddress, string reason, int hours ) {
			Client admin = admincharacter.Player;
			Database.ExecPrepared( $"REPLACE INTO ban (socialclubname, address, type, startsec, startoptic, endsec, endoptic, admin, reason) VALUES " +
				$"(@socialclubname, @address, 1, {Utility.GetTimespan()}, '{Utility.GetTimestamp()}', " +
				$"{Utility.GetTimespan( hours * 3600 )}, '{Utility.GetTimestamp( hours * 3600 )}', {admincharacter.UID}, @reason)", new Dictionary<string, string> {
					{ "@socialclubname", targetname },
					{ "@address", targetaddress },
					{ "@admin", admin.SocialClubName },
					{ "@reason", reason }
			} );
			socialClubNameBanDict[targetname] = true;
			if ( targetaddress != "-" )
				addressBanDict[targetaddress] = true;
			ServerLanguage.SendMessageToAll( "timeban", targetname, hours.ToString(), admin.Name, reason );
			if ( target != null )
				target.Kick( target.GetLang( "youtimeban", hours.ToString(), admin.Name, reason ) );
			// LOG //
			AdminLog.Log( AdminLogType.TIMEBAN, admincharacter.UID, PlayerUIDs[targetname], reason, hours );
			/////////
		}

		public static async Task UnBanPlayer ( Character admincharacter, Client target, string targetname, string reason, uint uid ) {
			DataTable result = await Database.ExecResult( $"SELECT address FROM ban WHERE uid = {uid}" ).ConfigureAwait( false );
			string targetaddress = result.Rows[0]["address"].ToString();
			Database.Exec( $"DELETE FROM ban WHERE uid = {uid}" );
			socialClubNameBanDict.Remove( targetname );
			if ( targetaddress != "-" )
				addressBanDict.Remove( targetaddress );

			ServerLanguage.SendMessageToAll( "unban", targetname, admincharacter.Player.Name, reason );
			// LOG //
			AdminLog.Log( AdminLogType.UNBAN, admincharacter.UID, PlayerUIDs[targetname], reason );
			/////////
		}

		public static void ChangePlayerMuteTime ( Character admincharacter, Client target, uint targetUID, int minutes, string reason ) {
			Client admin = admincharacter.Player;
			Database.Exec( $"UPDATE player SET mutetime={targetUID} WHERE uid = {targetUID}" );
			if ( target != null && target.Exists && target.GetChar().LoggedIn )
				target.GetChar().MuteTime = minutes;
			switch ( minutes ) {
				case -1:
					ServerLanguage.SendMessageToAll( "permamute", GetNameByUID( targetUID ), admin.Name, reason );
					AdminLog.Log( AdminLogType.PERMAMUTE, admincharacter.UID, targetUID, reason );
					break;
				case 0:
					ServerLanguage.SendMessageToAll( "unmute", GetNameByUID( targetUID ), admin.Name, reason );
					AdminLog.Log( AdminLogType.UNMUTE, admincharacter.UID, targetUID, reason );
					break;
				default:
					ServerLanguage.SendMessageToAll( "timemute", GetNameByUID( targetUID ), minutes.ToString(), admin.Name, reason );
					AdminLog.Log( AdminLogType.TIMEMUTE, admincharacter.UID, targetUID, reason, minutes );
					break;
			}
			

		}

	}

}
