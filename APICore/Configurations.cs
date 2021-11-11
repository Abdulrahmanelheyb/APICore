using System;
using System.Data;
using System.IO;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace APICore
{

    
    public static class Configurations
    {
        /// <summary>
        ///     All messages types for response
        /// </summary>
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
        
        /// <summary>
        ///     Read the 'config.json' file configurations
        /// </summary>
        /// <returns>Object of config.json</returns>
        public static dynamic GetConfigurations()
        {
            try
            {
                var config = File.ReadAllText("config.json");
                return JsonConvert.DeserializeObject<dynamic>(config);
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
        
        /// <summary>
        ///     Read the database connection config
        /// </summary>
        /// <returns>Database connection</returns>
        public static IDbConnection CreateDatabaseConnection()
        {
            try
            {
                var con = new MySqlConnection(GetConnectionString());
                if (con.State != ConnectionState.Open) con.Open();
                return con;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
        
        /// <summary>
        ///     
        /// </summary>
        /// <param name="messageType">Type of the response message</param>
        /// <param name="messageValue">Value of Success or failed message type</param>
        /// <returns>Message string</returns>
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