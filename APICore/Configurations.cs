using System;
using System.Data;
using System.IO;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace APICore
{

    
    public static class Configurations
    {
        public enum MessageTypes
        {
            GetAll,
            Get,
            Add,
            Update,
            Delete,
            Login,
            Logout,
            Exception
        }
        
        #region Private Fields
        private static readonly string[] Bases = { "Failed !", "Complete." };
        #endregion
        
        public static dynamic GetConfigurations()
        {
            try
            {
                var config = File.ReadAllText("config.json");
                return JsonConvert.DeserializeObject<dynamic>(config)["Connection"];
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        
        private static string GetConnectionString()
        {
            var config = File.ReadAllText("config.json");
            var connection = JsonConvert.DeserializeObject<dynamic>(config)["Connection"];
            return 
                connection == null 
                    ? throw new Exception("An error has been encountered in the configuration file.")
                    : $"server={connection["Hostname"]};port={connection["Port"]};uid={connection["Username"]};" +
                      $"pwd={connection["Password"]};database={connection["Database"]}";
        }

        public static MySqlConnection CreateDatabaseConnection()
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

        public static string GetMessage(MessageTypes messageType, bool messageValue = false)
        {
            var messageValueType = messageValue ? Bases[1] : Bases[0];
            
            return messageType switch
            {
                MessageTypes.GetAll => $"Get All Data {messageValueType}",
                MessageTypes.Get => $"Get {messageValueType}",
                MessageTypes.Add => $"Insert {messageValueType}",
                MessageTypes.Update => $"Update {messageValueType}",
                MessageTypes.Delete => $"Delete {messageValueType}",
                MessageTypes.Login => $"Authentication {messageValueType}",
                MessageTypes.Logout => $"Logout {messageValueType}",
                MessageTypes.Exception => $"System error !",
                _ => "No message !"
            };
        }

    }    
}