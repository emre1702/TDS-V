using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TDS_Server_DB.Entity
{
    partial class TDSNewContext
    {
        private static string _connectionString;

        public TDSNewContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(_connectionString);
            }
        }*/
    }
}
