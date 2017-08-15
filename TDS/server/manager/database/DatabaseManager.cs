﻿using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using GrandTheftMultiplayer.Server.API;

class Database {
	private static readonly string ip = "127.0.0.1";
	private static readonly int port = 3306;
	private static readonly string user = "emre1702";
	private static readonly string password = "Ajagrebo1-";
	private static readonly string database = "TDS";

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

	public static DataTable ExecResult ( string sql ) {
		using ( MySqlConnection conn = new MySqlConnection ( connStr ) ) {
			try {
				MySqlCommand cmd = new MySqlCommand ( sql, conn );
				conn.Open ();
				MySqlDataReader rdr = cmd.ExecuteReader ();
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

	public static DataTable ExecPreparedResult ( string sql, Dictionary<string, string> parameters ) {
		using ( MySqlConnection conn = new MySqlConnection ( connStr ) ) {
			try {
				MySqlCommand cmd = new MySqlCommand ( sql, conn );
				conn.Open ();

				foreach ( KeyValuePair<string, string> entry in parameters ) {
					cmd.Parameters.AddWithValue ( entry.Key, entry.Value );
				}

				MySqlDataReader rdr = cmd.ExecuteReader ();
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

	public static void Exec ( string sql ) {
		using ( MySqlConnection conn = new MySqlConnection ( connStr ) ) {
			try {
				MySqlCommand cmd = new MySqlCommand ( sql, conn );
				conn.Open ();
				cmd.ExecuteNonQuery ();
			} catch ( Exception ex ) {
				Manager.Log.Error ( "DATABASE: [ERROR] " + ex.ToString () );
			}
		}
	}

	public static void ExecPrepared ( string sql, Dictionary<string, string> parameters ) {
		using ( MySqlConnection conn = new MySqlConnection ( connStr ) ) {
			try {
				MySqlCommand cmd = new MySqlCommand ( sql, conn );
				conn.Open ();
				foreach ( KeyValuePair<string, string> entry in parameters ) {
					cmd.Parameters.AddWithValue ( entry.Key, entry.Value );
				}
				cmd.ExecuteNonQuery ();
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
