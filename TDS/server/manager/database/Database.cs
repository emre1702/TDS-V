namespace TDS.server.manager.database {

	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Data.Common;
	using System.Diagnostics.CodeAnalysis;
	using System.Threading.Tasks;
	using GTANetworkAPI;
	using logs;
	using MySql.Data.MySqlClient;

	class Database : Script {
		private const string ip = "127.0.0.1";
		private const int port = 3306;
		private static string user;
		private static string password;
		private static string database;

		/* Variables */
		private static string connStr;

		/* Constructor */

		public Database () {
            user = NAPI.Resource.GetResourceSetting<string> ( "TDS-V", "mysql_name" );
            password = NAPI.Resource.GetResourceSetting<string> ( "TDS-V", "mysql_password" );
            database = NAPI.Resource.GetResourceSetting<string> ( "TDS-V", "mysql_database" );

            connStr = "server=" + ip + ";user=" + user + ";database=" + database + ";port=" + port + ";password=" + password + ";";
            OnResourceStart ();
        }

		/* Exports */

		[SuppressMessage ( "Microsoft.Security", "CA2100:SQL-Abfragen auf Sicherheitsrisiken überprüfen" )]
		public static async Task<DataTable> ExecResult ( string sql ) {
			using ( MySqlConnection conn = new MySqlConnection ( connStr ) ) {
				try {
					MySqlCommand cmd = new MySqlCommand ( sql, conn );
					await conn.OpenAsync ().ConfigureAwait ( false );
					DbDataReader rdr = await cmd.ExecuteReaderAsync ().ConfigureAwait ( false );
					DataTable results = new DataTable ();
					results.Load ( rdr );
					rdr.Close ();
					return results;
				} catch ( Exception ex ) {
					Log.Error ( "DATABASE: [ERROR] " + ex );
					return null;
				}
			}
		}

		public static async Task<DataTable> ExecPreparedResult ( string sql, Dictionary<string, string> parameters ) {
			using ( MySqlConnection conn = new MySqlConnection ( connStr ) ) {
				try {
					MySqlCommand cmd = new MySqlCommand ( sql, conn );
					await conn.OpenAsync ().ConfigureAwait ( false );

					foreach ( KeyValuePair<string, string> entry in parameters ) {
						cmd.Parameters.AddWithValue ( entry.Key, entry.Value );
					}

					DbDataReader rdr = await cmd.ExecuteReaderAsync ().ConfigureAwait ( false );
					DataTable results = new DataTable ();
					results.Load ( rdr );
					rdr.Close ();
					return results;
				} catch ( Exception ex ) {
					Log.Error ( "DATABASE: [ERROR] " + ex );
					return null;
				}
			}
		}

		[SuppressMessage ( "Microsoft.Security", "CA2100:SQL-Abfragen auf Sicherheitsrisiken überprüfen" )]
		public static async Task Exec ( string sql ) {
			using ( MySqlConnection conn = new MySqlConnection ( connStr ) ) {
				try {
					MySqlCommand cmd = new MySqlCommand ( sql, conn );
					await conn.OpenAsync ().ConfigureAwait ( false );
					await cmd.ExecuteNonQueryAsync ().ConfigureAwait ( false );
				} catch ( Exception ex ) {
					Log.Error ( "DATABASE: [ERROR] " + ex );
				}
			}
		}

		public static async Task ExecPrepared ( string sql, Dictionary<string, string> parameters ) {
			using ( MySqlConnection conn = new MySqlConnection ( connStr ) ) {
				try {
					MySqlCommand cmd = new MySqlCommand ( sql, conn );
					await conn.OpenAsync ().ConfigureAwait ( false );
					foreach ( KeyValuePair<string, string> entry in parameters ) {
						cmd.Parameters.AddWithValue ( entry.Key, entry.Value );
					}
					await cmd.ExecuteNonQueryAsync ().ConfigureAwait ( false );
				} catch ( Exception ex ) {
					Log.Error ( "DATABASE: [ERROR] " + ex );
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

		private async void OnResourceStart () {
			using ( MySqlConnection conn = new MySqlConnection ( connStr ) ) {
				try {
					API.ConsoleOutput ( "DATABASE: [INFO] Attempting connecting to MySQL" );
					await conn.OpenAsync ();
					if ( conn.State == ConnectionState.Open ) {
						API.ConsoleOutput ( "DATABASE: [INFO] Connected to MySQL" );
					} else
                        API.ConsoleOutput ( "DATABASE: [ERROR] Connection to MySQL failed! "+ conn.State.ToString() );
                } catch ( Exception ex ) {
					Log.Error ( "DATABASE: [ERROR] " + ex );
				}
			}
		}
	}

}
