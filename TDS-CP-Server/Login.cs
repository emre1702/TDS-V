﻿using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TDSCPServer
{
    public class Login
    {

        private static string ConvertToSHA512(string input)
        {
            byte[] hashbytes = SHA512.Create().ComputeHash(Encoding.Default.GetBytes(input));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; hashbytes != null && i < hashbytes.Length; i++)
            {
                sb.AppendFormat("{0:x2}", hashbytes[i]);
            }
            return sb.ToString();
        }

        public static async Task<string> AuthenticateAsync(UserData data)
        {
            DataTable result = await Database.ExecPreparedResult("SELECT uid, adminlvl, password FROM player WHERE name = @1@", new Dictionary<string, string>
            {
                { "@1@", data.Username }
            });
            if (result.Rows.Count == 0)
            {
                return "The user doesn't exist.";
            }
            DataRow row = result.Rows[0];
            string password = ConvertToSHA512(data.Password);
            if (row["password"].ToString() != password)
            {
                return "Wrong password.";
            }
            data.UID = Convert.ToUInt32(row["uid"]);
            data.AdminLvl = Convert.ToUInt32(row["adminlvl"]);

            return null;
        }
    }
}
