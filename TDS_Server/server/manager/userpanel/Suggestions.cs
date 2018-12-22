using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TDS.server.enums;
using TDS.server.extend;
using TDS.server.instance.player;
using TDS.server.manager.database;
using TDS.server.manager.player;
using TDS.server.manager.utility;

namespace TDS.server.manager.userpanel {

	[Serializable]
	class Suggestion {
		public uint ID;
		[JsonIgnore]
		public uint AuthorUID;
		public string Topic;
		public string Author;
		public string Title;
		public SuggestionState State = SuggestionState.OPEN;
		[JsonIgnore]
		public List<SuggestionText> Texts = new List<SuggestionText>();
		[JsonIgnore]
		public Dictionary<string, int> VoteByName = new Dictionary<string, int>();
	}

	[Serializable]
	class SuggestionText {
		public uint ID;
		public string Author;
		public string Text;
		public string Date;
	}

	partial class Userpanel {

		private static List<Suggestion> suggestions = new List<Suggestion>();
		private static Dictionary<uint, Suggestion> suggestionsByID = new Dictionary<uint, Suggestion>();
		private static Dictionary<Suggestion, uint> highestSuggestionsTextID = new Dictionary<Suggestion, uint>();

		private static Dictionary<Suggestion, List<Character>> listOfPlayersInSuggestion = new Dictionary<Suggestion, List<Character>>();
		private static Dictionary<Character, Suggestion> playersInSuggestion = new Dictionary<Character, Suggestion>();
		private static List<Character> playersInSuggestionMenu = new List<Character>();

		[RemoteEvent( "onClientCreateSuggestion" )]
		public static void ClientCreateSuggestion ( Client player, string title, string text, string topic ) {
			Character character = player.GetChar();
			Suggestion suggestion = new Suggestion() {
				ID = suggestionsByID.Keys.Max() + 1,
				AuthorUID = character.UID,
				Author = player.Name,
				Title = title,
				Topic = topic
			};

			suggestions.Add( suggestion );
			suggestionsByID[suggestion.ID] = suggestion;
			listOfPlayersInSuggestion[suggestion] = new List<Character>();

			foreach ( Character target in playersInSuggestionMenu ) {
				NAPI.ClientEvent.TriggerClientEvent( target.Player, "syncSuggestion", JsonConvert.SerializeObject( suggestion ) );
			}

			//Admin.SendLangNotificationToAdmins( "created_suggestion", suggestion.ForAdminlvl, suggestion.ID.ToString() );

			Database.ExecPrepared( $"INSERT INTO suggestions (id, authoruid, topic, title) VALUES ({suggestion.ID}, {suggestion.AuthorUID}, @TOPIC@, @TITLE@);", new Dictionary<string, string> {
				{ "@TOPIC@", suggestion.Topic },
				{ "@TITLE@", suggestion.Title }
			} );

			//SuggestionsLog.Creation( suggestion.ID, suggestion.AuthorUID, title, text );

			ClientAddTextToSuggestion( player, suggestion.ID, text );
		}

		[RemoteEvent( "onClientAddTextToSuggestion" )]
		public static void ClientAddTextToSuggestion ( Client player, uint suggestionid, string text ) {
			if ( !suggestionsByID.ContainsKey( suggestionid ) )
				return;
			Suggestion suggestion = suggestionsByID[suggestionid];
			Character character = player.GetChar();

			SuggestionText suggestiontext = new SuggestionText {
				ID = ++highestSuggestionsTextID[suggestion],
				Author = player.Name,
				Text = text,
				Date = Utility.GetTimestamp()
			};
			suggestion.Texts.Add( suggestiontext );

			foreach ( Character target in listOfPlayersInSuggestion[suggestion] )
				NAPI.ClientEvent.TriggerClientEvent( target.Player, "syncSuggestionText", JsonConvert.SerializeObject( suggestiontext ) );

			/* if ( suggestion.AuthorUID == character.UID ) {
				// if it's not the text when creating the suggestion //
				if ( suggestion.Texts.Count > 1 ) {
					Admin.SendLangNotificationToAdmins( "answered_suggestion", suggestion.ForAdminlvl, suggestion.ID.ToString() );
					//SuggestionsLog.Answer( suggestion.ID, character.UID, text );
				}
			} else {
				string name = Account.GetNameByUID( suggestion.AuthorUID );
				if ( name == string.Empty )
					return;
				Client author = NAPI.Player.GetPlayerFromName( name );
				if ( author == null || !author.Exists )
					return;
				author.SendLangNotification( "got_suggestion_answer", suggestion.ID.ToString() );
				//SuggestionsLog.Answer( suggestion.ID, character.UID, text );
			} */


			Database.ExecPrepared( $"INSERT INTO suggestiontexts (id, suggestionid, authoruid, text, date) VALUES ({suggestiontext.ID}, {suggestionid}, {character.UID}, @TEXT@, '{suggestiontext.Date}');", new Dictionary<string, string> {
				{ "@TEXT@", text }
			} );
		}

		[RemoteEvent( "onClientChangeSuggestionState" )]
		public static void ClientChangeSuggestionState ( Client player, uint suggestionid, uint state ) {
			if ( !suggestionsByID.ContainsKey( suggestionid ) )
				return;
			Suggestion suggestion = suggestionsByID[suggestionid];

			suggestion.State = (SuggestionState) state;

			foreach ( Character target in playersInSuggestionMenu ) {
				NAPI.ClientEvent.TriggerClientEvent( target.Player, "syncSuggestionState", suggestionid, state );
			}

			foreach ( Character target in listOfPlayersInSuggestion[suggestion] ) {
				NAPI.ClientEvent.TriggerClientEvent( target.Player, "syncSuggestionState", suggestionid, state );
			}

			Database.Exec( $"UPDATE suggestions SET state={state} WHERE id={suggestionid};" );

			//Character character = player.GetChar();
			//SuggestionsLog.Answer( suggestion.ID, character.UID, character.UID == suggestion.AuthorUID ? "Creator" : "Admin Lvl " + character.AdminLvl );
		}

		[RemoteEvent( "onClientOpenSuggestion" )]
		public static void ClientOpenSuggestion ( Client player, uint index ) {
			if ( !suggestionsByID.ContainsKey( index ) )
				return;
			Suggestion suggestion = suggestionsByID[index];
			Character character = player.GetChar();
			listOfPlayersInSuggestion[suggestion].Add( character );
			playersInSuggestion[character] = suggestion;
			NAPI.ClientEvent.TriggerClientEvent( player, "syncSuggestionTexts", JsonConvert.SerializeObject( suggestion.Texts ) );
			NAPI.ClientEvent.TriggerClientEvent( player, "syncSuggestionVotes", JsonConvert.SerializeObject( suggestion.VoteByName ) );
		}

		[RemoteEvent( "onClientCloseSuggestion" )]
		public static void ClientCloseSuggestion ( Client player ) {
			Character character = player.GetChar();
			if ( playersInSuggestion.ContainsKey( character ) ) {
				Suggestion suggestion = playersInSuggestion[character];
				playersInSuggestion.Remove( character );
				listOfPlayersInSuggestion[suggestion].Remove( character );
			}
		}

		private static void SendPlayerSuggestions ( Client player, SuggestionState state = SuggestionState.OPEN ) {
			NAPI.ClientEvent.TriggerClientEvent( player, "syncSuggestions", JsonConvert.SerializeObject( suggestions.Where( ( a ) => a.State == state ) ) );
		}

		[RemoteEvent( "onClientOpenSuggestionsMenu" )]
		public static void ClientOpenSuggestionsMenu ( Client player ) {
			playersInSuggestionMenu.Add( player.GetChar() );
			SendPlayerSuggestions( player );
		}

		[RemoteEvent( "onClientCloseSuggestionsMenu" )]
		public static void ClientCloseSuggestionsMenu ( Client player ) {
			playersInSuggestionMenu.Remove( player.GetChar() );
		}

		[RemoteEvent( "onClientRemoveSuggestion" )]
		public static void ClientRemoveSuggestion ( Client player, uint suggestionid ) {
			if ( !suggestionsByID.ContainsKey( suggestionid ) )
				return;

			Character character = player.GetChar();
			if ( character.IsAdminLevel( neededAdminlvls["removeSuggestion"] ) ) {
				Database.Exec( $"DELETE FROM suggestions WHERE id={suggestionid};" );
				Database.Exec( $"DELETE FROM suggestiontexts WHERE suggestionid={suggestionid};" );

				if ( !suggestionsByID.ContainsKey( suggestionid ) )
					return;

				Suggestion suggestion = suggestionsByID[suggestionid];
				suggestions.Remove( suggestion );
				suggestionsByID.Remove( suggestionid );
				listOfPlayersInSuggestion.Remove( suggestion );

				foreach ( Character target in playersInSuggestionMenu ) {
					NAPI.ClientEvent.TriggerClientEvent( target.Player, "syncSuggestionRemove", suggestionid );
				}

				//SuggestionsLog.Remove( suggestion.ID, character.UID, character.UID == suggestion.AuthorUID ? "Creator" : "Admin Lvl " + character.AdminLvl );
			}
		}

		[RemoteEvent( "onClientRequestSuggestionsByState" )]
		public static void RequestSuggestionsByState ( Client player, uint state ) {
			SendPlayerSuggestions( player, (SuggestionState)state );
		}

		// vote: 1 = yes, 0 = no, -1 = neither
		[RemoteEvent( "onClientToggleSuggestionVote" )]
		public static void ToggleSuggestionVote ( Client player, uint suggestionid, int vote ) {
			if ( !suggestionsByID.ContainsKey( suggestionid ) )
				return;

			Suggestion suggestion = suggestionsByID[suggestionid];
			bool hadvotedalready = false;
			if ( suggestion.VoteByName.ContainsKey( player.Name ) )
				hadvotedalready = true;
			suggestion.VoteByName[player.Name] = vote;

			foreach ( Character target in listOfPlayersInSuggestion[suggestion] ) {
				NAPI.ClientEvent.TriggerClientEvent( target.Player, "syncSuggestionVote", player.Name, vote );
			}

			if ( hadvotedalready )
				Database.Exec( $"UPDATE suggestionvotes SET vote = {vote} WHERE uid = {player.GetChar().UID} AND suggestionid = {suggestion.ID};" );
			else
				Database.Exec( $"INSERT INTO suggestionvotes (uid, suggestionid, vote) VALUES ({player.GetChar().UID}, {suggestion.ID}, {vote});" );
		}

		private async static void LoadSuggestionsData () {
			DataTable suggestionstable = await Database.ExecResult( "SELECT * FROM suggestions;" );
			foreach ( DataRow row in suggestionstable.Rows ) {
				Suggestion suggestion = new Suggestion {
					ID = Convert.ToUInt32( row["id"] ),
					AuthorUID = Convert.ToUInt32( row["authoruid"] ),
					Author = Account.GetNameByUID( Convert.ToUInt32( row["authoruid"] ) ),
					Topic = Convert.ToString( row["topic"] ),
					Title = Convert.ToString( row["title"] ),
					State = (SuggestionState) Convert.ToSByte( row["state"] )
				};
				suggestions.Add( suggestion );
				suggestionsByID[suggestion.ID] = suggestion;
				listOfPlayersInSuggestion[suggestion] = new List<Character>();
				highestSuggestionsTextID[suggestion] = 0;
			}

			DataTable textstable = await Database.ExecResult( "SELECT * FROM suggestiontexts;" );
			foreach ( DataRow row in textstable.Rows ) {
				SuggestionText text = new SuggestionText {
					ID = Convert.ToUInt32( row["id"] ),
					Author = Account.GetNameByUID( Convert.ToUInt32( row["authoruid"] ) ),
					Text = Convert.ToString( row["text"] ),
					Date = Convert.ToString( row["date"] )
				};
				uint suggestionid = Convert.ToUInt32( row["suggestionid"] );
				Suggestion suggestion = suggestionsByID[suggestionid];
				suggestion.Texts.Add( text );
				if ( highestSuggestionsTextID[suggestion] < text.ID )
					highestSuggestionsTextID[suggestion] = text.ID;
			}

			DataTable votes = await Database.ExecResult( "SELECT * FROM suggestionvotes;" );
			foreach ( DataRow row in votes.Rows ) {
				Suggestion suggestion = suggestionsByID[Convert.ToUInt32( row["suggestionid"] )];
				string name = Account.GetNameByUID( Convert.ToUInt32( row["uid"] ) );
				suggestion.VoteByName[name] = Convert.ToByte( row["vote"] );
			}
		}
	}
}