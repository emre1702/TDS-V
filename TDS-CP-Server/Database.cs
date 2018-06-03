using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace TDSCPServer
{
    public class Database
    {
        public static string connStr;

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(connStr);
        }

        [SuppressMessage("Microsoft.Security", "CA2100:SQL-Abfragen auf Sicherheitsrisiken überprüfen")]
        public static async Task<DataTable> ExecResult(string sql)
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    await conn.OpenAsync().ConfigureAwait(false);
                    DbDataReader rdr = await cmd.ExecuteReaderAsync().ConfigureAwait(false);
                    DataTable results = new DataTable();
                    results.Load(rdr);
                    rdr.Close();
                    return results;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("DATABASE: [ERROR] " + sql + "\n" + ex.ToString());
                    return null;
                }
            }
        }

        public static async Task<DataTable> ExecPreparedResult(string sql, Dictionary<string, string> parameters)
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    await conn.OpenAsync().ConfigureAwait(false);

                    foreach (KeyValuePair<string, string> entry in parameters)
                    {
                        cmd.Parameters.AddWithValue(entry.Key, entry.Value);
                    }

                    DbDataReader rdr = await cmd.ExecuteReaderAsync().ConfigureAwait(false);
                    DataTable results = new DataTable();
                    results.Load(rdr);
                    rdr.Close();
                    return results;
                }
                catch (Exception ex)
                {
                    string s = string.Join(";", parameters.Select(x => x.Key + "=" + x.Value).ToArray());
                    Console.WriteLine("DATABASE: [ERROR] " + sql + "\n" + s + "\n" + ex.ToString());
                    return null;
                }
            }
        }

        [SuppressMessage("Microsoft.Security", "CA2100:SQL-Abfragen auf Sicherheitsrisiken überprüfen")]
        public static async void Exec(string sql)
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    await conn.OpenAsync().ConfigureAwait(false);
                    await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("DATABASE: [ERROR]\n" + ex.ToString());
                }
            }
        }

        public static async void ExecPrepared(string sql, Dictionary<string, string> parameters)
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    await conn.OpenAsync().ConfigureAwait(false);
                    foreach (KeyValuePair<string, string> entry in parameters)
                    {
                        cmd.Parameters.AddWithValue(entry.Key, entry.Value);
                    }
                    await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("DATABASE: [ERROR]\n" + sql + "\n" + ex.ToString());
                }
            }
        }
    }
}
