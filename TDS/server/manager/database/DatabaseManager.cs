using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using GrandTheftMultiplayer.Server.API;
using System.Threading.Tasks;
using System.Data.Common;

static class Database {
	private static readonly string ip = "127.0.0.1";
	private static readonly int port = 3306;
	private static readonly string user = "emre1702";
	private static readonly string password = "Ajagrebo1-";
	private static readonly string database = "TDSV";

	/* Variables */
	private static readonly string connStr = "server=" + ip +
					";user=" + user +
					";database=" + database +
					";port=" + port +
					";password=" + password + ";";
	private static Dictionary<string, MySqlDataAdapter> dataAdapters;

	/* Constructor */

	public static void DatabaseOnStart ( API api ) {
		api.onResourceStart += OnResourceStart;
	}

	/* Exports */

	[System.Diagnostics.CodeAnalysis.SuppressMessage ( "Microsoft.Security", "CA2100:SQL-Abfragen auf Sicherheitsrisiken überprüfen" )]
	public static async Task<DataTable> ExecResult ( string sql ) {
		using ( MySqlConnection conn = new MySqlConnection ( connStr ) ) {
			try {
				MySqlCommand cmd = new MySqlCommand ( sql, conn );
				await conn.OpenAsync ();
				DbDataReader rdr = await cmd.ExecuteReaderAsync ();
				DataTable results = new DataTable ();
				results.Load ( rdr );
				rdr.Close ();
				return results;
			} catch ( Exception ex ) {
				Manager.Log.Error ( "DATABASE: [ERROR] " + ex.ToString () );
				return null;
			}
		}
	}

	public static async Task<DataTable> ExecPreparedResult ( string sql, Dictionary<string, string> parameters ) {
		using ( MySqlConnection conn = new MySqlConnection ( connStr ) ) {
			try {
				MySqlCommand cmd = new MySqlCommand ( sql, conn );
				await conn.OpenAsync ();

				foreach ( KeyValuePair<string, string> entry in parameters ) {
					cmd.Parameters.AddWithValue ( entry.Key, entry.Value );
				}

				DbDataReader rdr = await cmd.ExecuteReaderAsync ();
				DataTable results = new DataTable ();
				results.Load ( rdr );
				rdr.Close ();
				return results;
			} catch ( Exception ex ) {
				Manager.Log.Error ( "DATABASE: [ERROR] " + ex.ToString () );
				return null;
			}
		}
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage ( "Microsoft.Security", "CA2100:SQL-Abfragen auf Sicherheitsrisiken überprüfen" )]
	public static async Task Exec ( string sql ) {
		using ( MySqlConnection conn = new MySqlConnection ( connStr ) ) {
			try {
				MySqlCommand cmd = new MySqlCommand ( sql, conn );
				await conn.OpenAsync ();
				await cmd.ExecuteNonQueryAsync ();
			} catch ( Exception ex ) {
				Manager.Log.Error ( "DATABASE: [ERROR] " + ex.ToString () );
			}
		}
	}

	public static async Task ExecPrepared ( string sql, Dictionary<string, string> parameters ) {
		using ( MySqlConnection conn = new MySqlConnection ( connStr ) ) {
			try {
				MySqlCommand cmd = new MySqlCommand ( sql, conn );
				await conn.OpenAsync ();
				foreach ( KeyValuePair<string, string> entry in parameters ) {
					cmd.Parameters.AddWithValue ( entry.Key, entry.Value );
				}
				await cmd.ExecuteNonQueryAsync ();
			} catch ( Exception ex ) {
				Manager.Log.Error ( "DATABASE: [ERROR] " + ex.ToString () );
			}
		}
	}

	/*public static DataTable CreateDataTable ( string sql, string unique_name ) {
		using ( MySqlConnection conn = new MySqlConnection ( connStr ) ) {
			try {
				MySqlDataAdapter dataAdapter;
				DataTable dataTable;
				dataAdapter = new MySqlDataAdapter ( sql, conn );
				MySqlCommandBuilder cb = new MySqlCommandBuilder ( dataAdapter );
				dataAdapters[unique_name] = dataAdapter;
				dataTable = new DataTable ();
				dataAdapter.Fill ( dataTable );
				return dataTable;
			} catch ( Exception ex ) {
				Manager.Log.Error ( "DATABASE: [ERROR] " + ex.ToString () );
				return null;
			}
		}
	}

	public static void UpdateDataTable ( string unique_name, DataTable updatedTable ) {
		try {
			dataAdapters[unique_name].Update ( updatedTable );
		} catch ( Exception ex ) {
			Manager.Log.Error ( "DATABASE: [ERROR] " + ex.ToString () );
		}
	}

	public static void CloseDataTable ( string unique_name ) {
		try {
			MySqlDataAdapter data = dataAdapters[unique_name];
			dataAdapters.Remove ( unique_name );
			data.Dispose ();
		} catch ( Exception ex ) {
			Manager.Log.Error ( "DATABASE: [ERROR] " + ex.ToString () );
		}
	}*/

	/* Hooks */

	public static void OnResourceStart ( ) {
		dataAdapters = new Dictionary<string, MySqlDataAdapter> ();

		using ( MySqlConnection conn = new MySqlConnection ( connStr ) ) {
			try {
				API.shared.consoleOutput ( "DATABASE: [INFO] Attempting connecting to MySQL" );
				conn.Open ();
				if ( conn.State == ConnectionState.Open ) {
					API.shared.consoleOutput ( "DATABASE: [INFO] Connected to MySQL" );
				}
			} catch ( Exception ex ) {
				Manager.Log.Error ( "DATABASE: [ERROR] " + ex.ToString () );
			}

		}
	}
}
