using System;
using System.Data;
using System.IO;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace APICore
{

    
    public class Configurations
    {
        private readonly string _connectionString;        
        public Configurations()
        {
            var config = File.ReadAllText("config.json");
            var connection = JsonConvert.DeserializeObject<dynamic>(config)["Connection"];
            _connectionString = 
                connection == null 
                    ? throw new Exception("An error has been encountered in the configuration file.")
                    : $"server={connection["Hostname"]};port={connection["Port"]};uid={connection["Username"]};" +
                      $"pwd={connection["Password"]};database={connection["Database"]}";
        }

        public MySqlConnection CreateDatabaseConnection()
        {
            try
            {
                var con = new MySqlConnection(_connectionString);
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