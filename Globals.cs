using MySql.Data.MySqlClient;
using System.Data;
using System;

namespace Core
{
    public static class Globals
    {
        private static string GetConnectionString()
        {            
            return $"server={Properties.Resources.Host};" +
                   $"port={Properties.Resources.Port};" +
                   $"uid={Properties.Resources.Username};" +
                   $"pwd={Properties.Resources.Password};" +
                   $"database={Properties.Resources.Database}";
        }
        
        public static MySqlConnection GetConnection()
        {
            try
            {
                var con = new MySqlConnection(GetConnectionString());
                if (con.State != ConnectionState.Open) con.Open();
                return con;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}