using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using ValidationResult = FluentValidation.Results.ValidationResult;

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
            Patch,
            Delete,
            Login,
            Logout,
            Exception
        }

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
        public static MySqlConnection CreateDatabaseConnection()
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
        /// <param name="successValue">Value of Success or failed message type</param>
        /// <returns>Message string</returns>
        public static string GetMessage(MessageTypes messageType, bool successValue = false)
        {
            var messageValueType = successValue ? "Complete." : "Failed !";
            
            return messageType switch
            {
                MessageTypes.GetAll => $"Get All Data {messageValueType}",
                MessageTypes.Get => $"Get {messageValueType}",
                MessageTypes.Add => $"Insert {messageValueType}",
                MessageTypes.Update => $"Update {messageValueType}",
                MessageTypes.Patch => $"Patch {messageValueType}",
                MessageTypes.Delete => $"Delete {messageValueType}",
                MessageTypes.Login => $"Authentication {messageValueType}",
                MessageTypes.Logout => $"Logout {messageValueType}",
                MessageTypes.Exception => $"System error !",
                _ => "No message !"
            };
        }

        /// <summary>
        /// Converts fluent validation messages to the string array
        /// </summary>
        /// <param name="validationResult">Validation result object</param>
        /// <returns>String array of error messages</returns>
        public static string[] ValidationMessagesToArray(ValidationResult validationResult)
        {
            var errors = new List<string>();
            validationResult.Errors.ForEach(error =>
            {
                errors.Add(error.ErrorMessage);
            });

            return errors.ToArray();
        }

        public static string GetToken(HttpContext context)
        {
            return context.Request.Headers?["token"];
        }

    }
}