using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace APICore
{    
    /// <summary>
    /// Represents parameterized sql query
    /// </summary>
    public class SqlQueryParamizer
    {        
        [UsedImplicitly]
        public static string BaseFolder { get; set; }
        private string _query;
        [UsedImplicitly]
        public string Query
        {
            get => FormatQuery();
            private set => _query = value;
        }

        [UsedImplicitly] public List<SqlQueryParameter> Parameters { get; } = new();

        public SqlQueryParamizer([System.Diagnostics.CodeAnalysis.NotNull] params string[] path)
        {
            // countOfParameters, increments counter to print number in string for replace value using by string format method
            var countOfParameters = 0;
            
            try
            {
                if (string.IsNullOrEmpty(BaseFolder)) throw new NullReferenceException("Base Folder not assigned !");
                if (path.Length == 0) throw new NullReferenceException("Invalid path or null !");

                    // Read SQL File
                var fileLines = File.ReadAllLines(@$"{BaseFolder}/{string.Join('/', path)}.sql");
                // Loop on file lines
                foreach (var line in fileLines)
                {
                    // creates regular expressions matcher
                    var r = new Regex(@"(?<=\/\*@)(.*?)(?=\*\/)");
                    // Matches the text in file line
                    var matches = r.Matches(line);
                    // Checks the have matches
                    if (matches.Any())
                    {
                        // Rebuilds matched parameter comment.
                        var parameterComment = @$"/*@{matches[0].Value}*/";
                        // Adds parameter to parameters list
                        Parameters.Add(new SqlQueryParameter{ Name = matches[0].Value, Index = countOfParameters});  
                        // replace the parameter comment to string format 
                        var replace = line.Replace(parameterComment, "{" + countOfParameters + "}");
                        // Adds line text replaced to query variable
                        _query += $"{replace}\n";
                        // Increments the count of parameters value
                        countOfParameters++;
                    }
                    else
                    {
                        // Adds line text when no matches are available
                        _query += $"{line}\n";
                    }                    
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        private static string Trimmer(string value)
        {
            var splittedArray = value.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return splittedArray.Length switch
            {
                > 1 => string.Join(' ', splittedArray),
                _ => value
            };
        }

        public SqlQueryParameter GetParameter(string paramName)
        {
            try
            {
                var parameters = Parameters.Where(parameter => parameter.Name == paramName).ToList();
                if (!parameters.Any()) throw new Exception("Parameter not found");
                if (parameters.Count > 1) throw new Exception("Has 2 parameter contain this name !");
                
                return parameters[0];
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private string FormatQuery()
        {
            return Trimmer(_query);
        }
    }
}