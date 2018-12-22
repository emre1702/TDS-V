using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TDS.server.enums;
using TDS.server.extend;
using TDS.server.instance.player;
using TDS.server.manager.database;
using TDS.server.manager.logs;
using TDS.server.manager.utility;

namespace TDS.server.instance.lobby {

	public class LobbyBan {
		public sbyte Type;
		public string End;
		public uint EndSpan;
		public string ByAdmin;
		public string Reason;
	}

	partial class Lobby {

		public bool BanAllowed = true;
		private bool SaveBans = false;
		public Dictionary<uint, LobbyBan> lobbyBans = new Dictionary<uint, LobbyBan>();

		public LobbyBan GetPlayerLobbyBan ( Character character ) {
			return lobbyBans.ContainsKey( character.UID ) ? lobbyBans[character.UID] : null;
		}

		public async Task ActivateBansSaving () {
			SaveBans = true;
			await LoadBans();
		}

		private async Task LoadBans () {
			DataTable table = await Database.ExecResult( $"SELECT lobbyban.*, player.name FROM lobbyban, player WHERE lobbyban.lobbyid = {ID} AND lobbyban.admin = player.uid;" );

			uint timespan = manager.utility.Utility.GetTimespan();
			foreach ( DataRow row in table.Rows ) {
				sbyte type = Convert.ToSByte( row["type"] );
				int endtimespan = Convert.ToInt32( row["endsec"] );
				if ( type == 0 /*permaban*/ || timespan < endtimespan ) {
					lobbyBans[Convert.ToUInt32( row["uid"] )] = new LobbyBan {
						Type = Convert.ToSByte( row["type"] ),
						End = Convert.ToString( row["endoptic"] ),
						EndSpan = Convert.ToUInt32( row["endsec"] ),
						ByAdmin = Convert.ToString( row["name"] ),
						Reason = Convert.ToString( row["reason"] )
					};
				} else
					Database.Exec( $"DELETE FROM lobbyban WHERE id = {row["id"].ToString()}" );
			}
		}

		public void PermaBanPlayer ( Character admincharacter, Client target, string targetname, uint targetuid, string reason ) {
			Client admin = admincharacter.Player;

			LobbyBan ban = new LobbyBan {
				Type = 0,
				ByAdmin = admincharacter.Player.Name,
				Reason = reason
			};
			lobbyBans[targetuid] = ban;

			SendAllPlayerLangMessage( "permaban_lobby", -1, targetname, admin.Name, reason ); 
			if ( target != null ) {
				Character targetcharacter = target.GetChar();
				if ( targetcharacter.Lobby == this )
					RemovePlayerDerived( targetcharacter );
			}

			if ( SaveBans ) {
				Database.ExecPrepared( $"REPLACE INTO lobbyban (uid, lobbyid, type, startsec, startoptic, admin, reason) VALUES " +
					$"({targetuid}, {ID}, 0, {Utility.GetTimespan()}, '{Utility.GetTimestamp()}', {admincharacter.UID}, @reason)",
					new Dictionary<string, string> {
						{ "@reason", reason }
					}
				);
				AdminLog.Log( AdminLogType.PERMABANLOBBY, admincharacter.UID, targetuid, reason, lobbyid: ID );
			}
		}

		public void TimeBanPlayer ( Character admincharacter, Client target, string targetname, uint targetuid, int hours, string reason ) {
			Client admin = admincharacter.Player;

			LobbyBan ban = new LobbyBan {
				Type = 1,
				End = Utility.GetTimestamp( hours * 3600 ),
				EndSpan = Utility.GetTimespan( hours * 3600 ),
				ByAdmin = admincharacter.Player.Name,
				Reason = reason
			};
			lobbyBans[targetuid] = ban;

			SendAllPlayerLangMessage( "timeban_lobby", -1, targetname, admin.Name, hours.ToString(), reason );
			if ( target != null ) {
				Character targetcharacter = target.GetChar();
				if ( targetcharacter.Lobby == this )
					RemovePlayerDerived( targetcharacter );
			}

			if ( SaveBans ) {
				Database.ExecPrepared( $"REPLACE INTO lobbyban (uid, lobbyid, type, startsec, startoptic, endsec, endoptic, admin, reason) VALUES " +
					$"({targetuid}, {ID}, 1, {Utility.GetTimespan()}, '{Utility.GetTimestamp()}', {ban.EndSpan}, {ban.End}, {admincharacter.UID}, @reason)",
					new Dictionary<string, string> {
						{ "@reason", reason }
					}
				);
				AdminLog.Log( AdminLogType.TIMEBANLOBBY, admincharacter.UID, targetuid, reason, time: hours, lobbyid: ID );
			}
		}

		public void UnBanPlayer ( Character admincharacter, Client target, string targetname, uint targetuid, string reason ) {
			if ( lobbyBans.ContainsKey( targetuid ) ) {

				RemoveBan( targetuid );

				SendAllPlayerLangMessage( "unban_lobby", -1, targetname, admincharacter.Player.Name, reason );
				if ( target != null ) {
					target.SendLangMessage( "unbaned_in_lobby", Name, admincharacter.Player.Name, reason );
				}

				if ( SaveBans ) {
					AdminLog.Log( AdminLogType.UNBANLOBBY, admincharacter.UID, targetuid, reason, lobbyid: ID );
				}
			}
		}

		private void RemoveBan ( uint uid ) {
			lobbyBans.Remove( uid );
			if ( SaveBans ) {
				Database.Exec( $"DELETE FROM lobbyban WHERE uid = {uid} AND lobbyid = {ID};" );
			}
		}
	}
}
