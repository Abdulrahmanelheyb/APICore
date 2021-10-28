using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace APICore
{
    public record SqlQueryParameter
    {
        [UsedImplicitly]
        public int Index { get; set; }
        [UsedImplicitly]
        public string Name { get; set; }
        public object Value { get; set; }
    } 
    
    public class SqlQuery
    {
        private string _query;
        [UsedImplicitly]
        public string Query
        {
            get => FormatQuery();
            private set => _query = value;
        }

        [UsedImplicitly] public List<SqlQueryParameter> Parameters { get; } = new();

        public SqlQuery([System.Diagnostics.CodeAnalysis.NotNull] params string[] path)
        {
            // countOfParameters, increments counter to print number in string
            var countOfParameters = 0;
            
            try
            {
                if (path.Length == 0) throw new NullReferenceException("Invalid path or null !");

                    // Read SQL File
                var fileLines = File.ReadAllLines(@$"Queries/{string.Join('/', path)}.sql");
                // Loop on file lines
                foreach (var line in fileLines)
                {
                    // creates regular expressions matcher
                    var r = new Regex(@"(?<=\/\*\()(.*?)(?=\)\*\/)");
                    // Matches the text in file line
                    var matches = r.Matches(line);
                    // Checks the have matches
                    if (matches.Any())
                    {
                        // Rebuilds matched parameter comment.
                        var parameterComment = @$"/*({matches[0].Value})*/";
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
        
        private static object ValueTypeChecker(object value)
        {
            if (value is null) return "";
            
            return value switch
            {
                int => $"{value}",
                bool => $"{value.ToString()?.ToLower()}",
                string str => str.Contains('(') && str.Contains(')')? $"{str}" : $"'{str}'",
                _ => $"'{value}'"
            };
        }

        public bool SetParameter(string paramName, object value)
        {
            try
            {
                var parameters = Parameters.Where(parameter => parameter.Name == paramName).ToList();
                if (!parameters.Any()) throw new Exception("Parameter not found");
                foreach (var parameter in parameters)
                {
                    parameter.Value = value;
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private string FormatQuery()
        {
            return Trimmer(string.Format(_query, Parameters.Select(param => ValueTypeChecker(param.Value)).ToArray()));
        }
    }
}