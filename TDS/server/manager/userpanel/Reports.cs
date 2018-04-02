using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TDS.server.extend;
using TDS.server.instance.player;
using TDS.server.manager.database;
using TDS.server.manager.player;
using TDS.server.manager.utility;

namespace TDS.server.manager.userpanel
{

	[Serializable]
	class Report
	{
		public uint ID;
		[JsonIgnore]
		public uint AuthorUID;
		[JsonIgnore]
		public uint ForAdminlvl;
		public string Author;
		public string Title;
		public bool Open = true;
		[JsonIgnore]
		public List<ReportText> Texts = new List<ReportText>();
	}

	[Serializable]
	class ReportText
	{
		public uint ID;
		public string Author;
		public string Text;
		public string Date;
	}

	partial class Userpanel
	{

		private static Dictionary<string, uint> neededAdminlvls = new Dictionary<string, uint> {
			{ "remove", 2 }
		};

		private static List<Report> reports = new List<Report>();
		private static Dictionary<uint, Report> reportsByID = new Dictionary<uint, Report>();
		private static Dictionary<Report, uint> highestTextID = new Dictionary<Report, uint>();

		private static Dictionary<Report, List<Character>> listOfPlayersInReport = new Dictionary<Report, List<Character>>();
		private static Dictionary<Character, Report> playersInReport = new Dictionary<Character, Report>();
		private static List<Character> playersInReportMenu = new List<Character>();

		[RemoteEvent("onClientCreateReport")]
		public static void ClientCreateReport(Client player, string title, string text, uint forminadminlvl)
		{
			Character character = player.GetChar();
			Report report = new Report()
			{
				ID = reportsByID.Keys.Max() + 1,
				AuthorUID = character.UID,
				Author = player.Name,
				Title = title,
				ForAdminlvl = forminadminlvl
			};

			reports.Add(report);
			reportsByID[report.ID] = report;
			listOfPlayersInReport[report] = new List<Character>();

			foreach ( Character target in playersInReportMenu )
			{
				if ( player == target.Player || target.AdminLvl >= report.ForAdminlvl )
					NAPI.ClientEvent.TriggerClientEvent(target.Player, "syncReport", JsonConvert.SerializeObject(report));
			}

			Admin.SendLangNotificationToAdmins("created_report", report.ForAdminlvl, report.ID.ToString());

			Database.ExecPrepared($"INSERT INTO reports (id, authoruid, foradminlvl, title) VALUES ({report.ID}, {report.AuthorUID}, {report.ForAdminlvl}, @TITLE@);", new Dictionary<string, string> {
				{ "@TITLE@", report.Title }
			});

			ClientAddTextToReport(player, report.ID, text);
		}

		[RemoteEvent("onClientAddTextToReport")]
		public static void ClientAddTextToReport(Client player, uint reportid, string text)
		{
			if ( !reportsByID.ContainsKey(reportid) )
				return;
			Report report = reportsByID[reportid];
			Character character = player.GetChar();

			ReportText reporttext = new ReportText
			{
				ID = ++highestTextID[report],
				Author = player.Name,
				Text = text,
				Date = Utility.GetTimestamp()
			};
			report.Texts.Add(reporttext);

			foreach ( Character target in listOfPlayersInReport[report] )
			{
				if ( target.UID == report.AuthorUID || target.AdminLvl >= report.ForAdminlvl )
					NAPI.ClientEvent.TriggerClientEvent(target.Player, "syncReportText", JsonConvert.SerializeObject(reporttext));
			}

			// if it's not the text when creating the report //
			if ( report.AuthorUID == character.UID )
			{
				if ( report.Texts.Count > 1 )
					Admin.SendLangNotificationToAdmins("answered_report", report.ForAdminlvl, report.ID.ToString());
			} else
			{
				string name = Account.GetNameByUID(report.AuthorUID);
				if ( name == "" )
					return;
				Client author = NAPI.Player.GetPlayerFromName(name);
				if ( author == null || !author.Exists )
					return;
				author.SendLangNotification("got_report_answer", report.ID.ToString());
			}
				

			Database.ExecPrepared($"INSERT INTO reporttexts (id, reportid, authoruid, text, date) VALUES ({reporttext.ID}, {reportid}, {character.UID}, @TEXT@, '{reporttext.Date}');", new Dictionary<string, string> {
				{ "@TEXT@", text }
			});
		}

		[RemoteEvent("onClientChangeReportState")]
		public static void ClientChangeReportState(Client player, uint reportid, bool state)
		{
			if ( !reportsByID.ContainsKey(reportid) )
				return;
			Report report = reportsByID[reportid];

			report.Open = state;

			foreach ( Character target in playersInReportMenu )
			{
				if ( target.UID == report.AuthorUID || target.AdminLvl >= report.ForAdminlvl )
					NAPI.ClientEvent.TriggerClientEvent(target.Player, "syncReportState", reportid, state);
			}

			foreach ( Character target in listOfPlayersInReport[report] )
			{
				NAPI.ClientEvent.TriggerClientEvent(target.Player, "syncReportState", reportid, state);
			}

			Database.Exec($"UPDATE reports SET open={(state ? 1 : 0)} WHERE id={reportid};");
		}

		[RemoteEvent("onClientOpenReport")]
		public static void ClientOpenReport(Client player, uint index)
		{
			if ( !reportsByID.ContainsKey(index) )
				return;
			Report report = reportsByID[index];
			Character character = player.GetChar();
			if ( character.AdminLvl >= report.ForAdminlvl || report.AuthorUID == character.UID )
			{
				listOfPlayersInReport[report].Add(character);
				playersInReport[character] = report;
				NAPI.ClientEvent.TriggerClientEvent(player, "syncReportTexts", JsonConvert.SerializeObject(report.Texts));
			}
		}

		[RemoteEvent("onClientCloseReport")]
		public static void ClientCloseReport(Client player)
		{
			Character character = player.GetChar();
			if ( playersInReport.ContainsKey(character) )
			{
				Report report = playersInReport[character];
				playersInReport.Remove(character);
				listOfPlayersInReport[report].Remove(character);
			}
		}

		private static void SendPlayerReports(Client player)
		{
			Character character = player.GetChar();
			List<Report> reportstosend = new List<Report>();
			foreach ( Report report in reports )
				if ( report.AuthorUID == character.UID )
					reportstosend.Add(report);
			if ( character.AdminLvl > 0 )
				foreach ( Report report in reports )
					if ( report.ForAdminlvl <= character.AdminLvl && report.AuthorUID != character.UID )
						reportstosend.Add(report);
			NAPI.ClientEvent.TriggerClientEvent(player, "syncReports", JsonConvert.SerializeObject(reportstosend));
		}

		[RemoteEvent("onClientOpenReportsMenu")]
		public static void ClientOpenReportsMenu(Client player)
		{
			playersInReportMenu.Add(player.GetChar());
			SendPlayerReports(player);
		}

		[RemoteEvent("onClientCloseReportsMenu")]
		public static void ClientCloseReportsMenu(Client player)
		{
			playersInReportMenu.Remove(player.GetChar());
		}

		[RemoteEvent("onClientRemoveReport")]
		public static void ClientRemoveReport(Client player, uint reportid)
		{
			if ( player.IsAdminLevel(neededAdminlvls["remove"]) )
			{
				Database.Exec($"DELETE FROM reports WHERE id={reportid};");
				Database.Exec($"DELETE FROM reporttexts WHERE reportid={reportid};");

				for ( int i = 0; i < reports.Count; ++i )
				{
					if ( reports[i].ID == reportid )
					{
						Report report = reports[i];
						reports.RemoveAt(i);
						reportsByID.Remove(reportid);
						listOfPlayersInReport.Remove(report);
						break;
					}
				}

				foreach ( Character target in playersInReportMenu )
				{
					NAPI.ClientEvent.TriggerClientEvent(target.Player, "syncReportRemove", reportid);
				}
			}
		}

		private async static void LoadReportsData()
		{
			DataTable reportstable = await Database.ExecResult("SELECT * FROM reports;");
			foreach ( DataRow row in reportstable.Rows )
			{
				Report report = new Report
				{
					ID = Convert.ToUInt32(row["id"]),
					AuthorUID = Convert.ToUInt32(row["authoruid"]),
					Author = Account.GetNameByUID(Convert.ToUInt32(row["authoruid"])),
					ForAdminlvl = Convert.ToUInt32(row["foradminlvl"]),
					Title = Convert.ToString(row["Title"]),
					Open = Convert.ToBoolean(row["open"])
				};
				reports.Add(report);
				reportsByID[report.ID] = report;
				listOfPlayersInReport[report] = new List<Character>();
				highestTextID[report] = 0;
			}

			DataTable textstable = await Database.ExecResult("SELECT * FROM reporttexts;");
			foreach ( DataRow row in textstable.Rows )
			{
				ReportText text = new ReportText
				{
					ID = Convert.ToUInt32(row["id"]),
					Author = Account.GetNameByUID(Convert.ToUInt32(row["authoruid"])),
					Text = Convert.ToString(row["Title"]),
					Date = Convert.ToString(row["state"])
				};
				uint reportid = Convert.ToUInt32(row["reportid"]);
				Report report = reportsByID[reportid];
				report.Texts.Add(text);
				if ( highestTextID[report] < text.ID )
					highestTextID[report] = text.ID;
			}
		}
	}
}

