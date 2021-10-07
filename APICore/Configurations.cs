using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace APICore
{
    public record ConnectionInfo
    {
        public string Hostname { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Database { get; set; }
    }
    
    public static class Configurations
    {
        private static string GetConnectionString()
        {
            var info = new ConnectionInfo()
            {
                Hostname = "localhost",
                Port = 3306,
                Database = "sbss",
                Username = "dev",
                Password = "deve!0perE><"
            };
            
            return $"server={info.Hostname};port={info.Port};uid={info.Username};pwd={info.Password};database={info.Database}";
        }
        
        public static MySqlConnection CreateConnection()
        {
            try
            {
                var con = new MySqlConnection(GetConnectionString());
                if (con.State != ConnectionState.Open) con.Open();
                return con;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}