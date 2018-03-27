using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TDS.server.extend;
using TDS.server.instance.player;
using TDS.server.manager.database;
using TDS.server.manager.utility;

namespace TDS.server.manager.userpanel {

	[Serializable]
	class Report {
		public uint ID;
		public uint AuthorUID;
		public uint ForAdminlvl;
		public string Title;
		public bool Open = true;
	}

	[Serializable]
	class ReportText {
		public uint ID;
		public uint ReportID;
		public uint AuthorUID;
		public string Text;
		public string Date;
	}

	partial class Userpanel {

		private static List<Report> reports = new List<Report> ();
		private static Dictionary<uint, Report> reportsByID = new Dictionary<uint, Report> ();
		private static Dictionary<Report, List<ReportText>> reportTextsByReport = new Dictionary<Report, List<ReportText>> ();
		private static Dictionary<Report, uint> highestTextID = new Dictionary<Report, uint> ();

		private static Dictionary<Report, List<Client>> listOfPlayersInReport = new Dictionary<Report, List<Client>>();
		private static Dictionary<Client, Report> playersInReport = new Dictionary<Client, Report> ();
		private static List<Client> playersInReportMain = new List<Client> ();

		[RemoteEvent("onPlayerCreateReport")]
		public static void PlayerCreateReport ( Client player, string json, string text ) {
			Character character = player.GetChar ();
			Report report = JsonConvert.DeserializeObject<Report> ( json );

			report.ID = reportsByID.Keys.Max() + 1;
			report.AuthorUID = character.UID;
			
			reports.Add ( report );
			reportsByID[report.ID] = report;
			reportTextsByReport[report] = new List<ReportText> ();
			listOfPlayersInReport[report] = new List<Client> ();

			foreach ( Client target in playersInReportMain ) {
				NAPI.ClientEvent.TriggerClientEvent ( target, "syncReport", JsonConvert.SerializeObject ( report ) );
			}

			Database.ExecPrepared ( $"INSERT INTO reports (id, authoruid, foradminlvl, title) VALUES ({report.ID}, {report.AuthorUID}, {report.ForAdminlvl}, @TITLE@);", new Dictionary<string, string> {
				{ "@TITLE@", report.Title }
			} );

			PlayerAddTextToReport ( player, report.ID, text );
		}

		[RemoteEvent("onPlayerAddTextToReport")]
		public static void PlayerAddTextToReport ( Client player, uint reportid, string text ) {
			if ( !reportsByID.ContainsKey ( reportid ) )
				return;
			Report report = reportsByID[reportid];
			Character character = player.GetChar ();

			ReportText reporttext = new ReportText {
				ID = ++highestTextID[report],
				ReportID = reportid,
				AuthorUID = character.UID,
				Text = text,
				Date = Utility.GetTimestamp ()
			};

			foreach ( Client target in listOfPlayersInReport[report] ) {
				NAPI.ClientEvent.TriggerClientEvent ( target, "syncReportText", JsonConvert.SerializeObject ( reporttext ) );
			}

			Database.ExecPrepared ( $"INSERT INTO reporttexts (id, reportid, authoruid, text, date) VALUES ({reporttext.ID}, {reportid}, {character.UID}, @TEXT@, '{reporttext.Date}');", new Dictionary<string, string> {
				{ "@TEXT@", text }
			} );
		}

		[RemoteEvent("onPlayerChangeReportState")] 
		public static void PlayerChangeReportState ( Client player, uint reportid, bool state ) {
			if ( !reportsByID.ContainsKey ( reportid ) )
				return;
			Report report = reportsByID[reportid];

			report.Open = state;

			foreach ( Client target in playersInReportMain ) {
				NAPI.ClientEvent.TriggerClientEvent ( target, "syncReportState", reportid, state );
			}

			foreach ( Client target in listOfPlayersInReport[report] ) {
				NAPI.ClientEvent.TriggerClientEvent ( target, "syncReportState", reportid, state );
			}

			Database.Exec ( $"UPDATE reports SET open={( state ? 1 : 0 )} WHERE id={reportid};" );
		}
		
		[RemoteEvent("onPlayerJoinReport")]
		public static void PlayerJoinReport ( Client player, uint index ) {
			if ( !reportsByID.ContainsKey ( index ) )
				return;
			Report report = reportsByID[index];
			Character character = player.GetChar ();
			if ( character.AdminLvl >= report.ForAdminlvl || report.AuthorUID == character.UID ) {
				listOfPlayersInReport[report].Add ( player );
				playersInReport[player] = report;
				NAPI.ClientEvent.TriggerClientEvent ( player, "syncReportTexts", JsonConvert.SerializeObject ( reportTextsByReport[report] ) );
			}
		}

		[RemoteEvent("onPlayerLeaveReport")]
		public static void PlayerLeaveReport ( Client player ) {
			if ( playersInReport.ContainsKey ( player ) ) {
				Report report = playersInReport[player];
				playersInReport.Remove ( player );
				listOfPlayersInReport[report].Remove ( player );
			}
		}

		private static void SendPlayerReports ( Client player ) {
			Character character = player.GetChar ();
			List<Report> reportstosend = new List<Report> ();
			foreach ( Report report in reports )
				if ( report.AuthorUID == character.UID )
					reportstosend.Add ( report );
			if ( character.AdminLvl > 0 )
				foreach ( Report report in reports )
					if ( report.ForAdminlvl <= character.AdminLvl && report.AuthorUID != character.UID )
						reportstosend.Add ( report );
			NAPI.ClientEvent.TriggerClientEvent ( player, "syncReports", JsonConvert.SerializeObject ( reportstosend ) );
		}

		[RemoteEvent("onPlayerJoinReportMain")]
		public static void PlayerJoinReportMain ( Client player ) {
			playersInReportMain.Add ( player );
			SendPlayerReports ( player );
		}

		[RemoteEvent("onPlayerLeaveReportMain")]
		public static void PlayerLeaveReportMain ( Client player ) {
			playersInReportMain.Remove ( player );
		}

		private async static void LoadReportsData ( ) {
			DataTable reportstable = await Database.ExecResult ( "SELECT * FROM reports;" ); 	
			foreach ( DataRow row in reportstable.Rows ) {
				Report report = new Report {
					ID = Convert.ToUInt32 ( row["id"] ),
					AuthorUID = Convert.ToUInt32 ( row["authoruid"] ),
					ForAdminlvl = Convert.ToUInt32 ( row["foradminlvl"] ),
					Title = Convert.ToString ( row["Title"] ),
					Open = Convert.ToBoolean ( row["open"] )
				};
				reports.Add ( report );
				reportsByID[report.ID] = report;
				reportTextsByReport[report] = new List<ReportText> ();
				listOfPlayersInReport[report] = new List<Client> ();
				highestTextID[report] = 0;
			}

			DataTable textstable = await Database.ExecResult ( "SELECT * FROM reporttexts;" );
			foreach ( DataRow row in textstable.Rows ) {
				ReportText text = new ReportText {
					ID = Convert.ToUInt32 ( row["id"] ),
					ReportID = Convert.ToUInt32 ( row["authoruid"] ),
					AuthorUID = Convert.ToUInt32 ( row["authoruid"] ),
					Text = Convert.ToString ( row["Title"] ),
					Date = Convert.ToString ( row["state"] )
				};
				Report report = reportsByID[text.ReportID];
				reportTextsByReport[report].Add ( text );
				if ( highestTextID[report] < text.ID )
					highestTextID[report] = text.ID;
			}
		}
	}
}

