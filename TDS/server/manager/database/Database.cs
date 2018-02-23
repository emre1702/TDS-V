namespace TDS.server.manager.database {

	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Data.Common;
	using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Xml;
    using GTANetworkAPI;
	using logs;
	using MySql.Data.MySqlClient;

	class Database {

		/* Variables */
		private static string connStr;

        public static async Task LoadConnStr ( ) {
            using ( XmlReader reader = XmlReader.Create ( "bridge/resources/TDS-V/config/mysql.xml", new XmlReaderSettings { Async = true } ) ) {
                while ( await reader.ReadAsync() ) {
                    if ( reader.NodeType == XmlNodeType.Element ) {
                        connStr = "server=" + reader["ip"] + ";user=" + reader["user"] + ";database=" + reader["database"] + ";port=" + reader["port"] + ";password=" + reader["password"] + ";";
                    }
                }
            }
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
					Log.Error ( "DATABASE: [ERROR] " + sql + "\n" + ex.ToString() );
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
                    string s = string.Join ( ";", parameters.Select ( x => x.Key + "=" + x.Value ).ToArray () );
                    Log.Error ( "DATABASE: [ERROR] " + sql + "\n" + s + "\n" + ex.ToString() );
					return null;
				}
			}
		}

		[SuppressMessage ( "Microsoft.Security", "CA2100:SQL-Abfragen auf Sicherheitsrisiken überprüfen" )]
		public static async void Exec ( string sql ) {
			using ( MySqlConnection conn = new MySqlConnection ( connStr ) ) {
				try {
					MySqlCommand cmd = new MySqlCommand ( sql, conn );
					await conn.OpenAsync ().ConfigureAwait ( false );
					await cmd.ExecuteNonQueryAsync ().ConfigureAwait ( false );
				} catch ( Exception ex ) {
					Log.Error ( "DATABASE: [ERROR]\n" + ex.ToString() );
				}
			}
		}

		public static async void ExecPrepared ( string sql, Dictionary<string, string> parameters ) {
			using ( MySqlConnection conn = new MySqlConnection ( connStr ) ) {
				try {
					MySqlCommand cmd = new MySqlCommand ( sql, conn );
					await conn.OpenAsync ().ConfigureAwait ( false );
					foreach ( KeyValuePair<string, string> entry in parameters ) {
						cmd.Parameters.AddWithValue ( entry.Key, entry.Value );
					}
					await cmd.ExecuteNonQueryAsync ().ConfigureAwait ( false );
				} catch ( Exception ex ) {
					Log.Error ( "DATABASE: [ERROR]\n" + sql + "\n" + ex.ToString() );
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
				Manager.Log.Error ( "DATABASE: [ERROR]\n" + ex.ToString() );
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

        private static void OnResourceStart () {
			using ( MySqlConnection conn = new MySqlConnection ( connStr ) ) {
				try {
					NAPI.Util.ConsoleOutput ( "DATABASE: [INFO] Attempting to connect to MySQL" );
					conn.Open ();
					if ( conn.State == ConnectionState.Open ) {
                        NAPI.Util.ConsoleOutput ( "DATABASE: [INFO] Connected to MySQL" );
					} else
                        NAPI.Util.ConsoleOutput ( "DATABASE: [ERROR] Connection to MySQL failed! "+ conn.State.ToString() );
                } catch ( Exception ex ) {
                    NAPI.Util.ConsoleOutput ( "DATABASE: [ERROR]\n" + ex.ToString() );
				}
			}
		}
	}

}
