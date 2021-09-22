using System.Collections.Generic;

namespace Core
{
    public class Queries
    {
        private readonly string _tableName;

        public Queries(string tableName)
        {
            _tableName = tableName;
        }

        private static string StringerArray(IReadOnlyList<string> data)
        {
            var rlt = "";
            
            foreach (var column in data)
            {
                    
                if (column != data[^1])
                {
                    rlt += $"{column}, ";
                }
                else
                {
                    rlt += $"{column}";
                }
            }

            return rlt;
        }
        
        private static string UpperFirstChar(string value)
        {
            return char.ToUpper(value[0]) + value[1..];
        }
        
        private static string StringerValues(IReadOnlyList<string> data)
        {
            var rlt = "";
            foreach (var value in data)
            {
                if (value != data[^1])
                {
                    rlt += $"@{UpperFirstChar(value)}, ";
                }
                else
                {
                    rlt += $"@{UpperFirstChar(value)}";
                }
            }
            return rlt;
        }

        private static string StringerUpdateSets(IReadOnlyList<string> data)
        {
            var rlt = "";
            foreach (var column in data)
            {
                if (column != data[^1])
                {
                    rlt += $"{column}=@{column}, ";
                }
                else
                {
                    rlt += $"{column}=@{column}";
                }
            }
            return rlt;
        }
        
        public static string WhereQuery(string field, object value,bool withQuote = false)
        {
            return withQuote ? $"WHERE {field}='{value}'" : $"WHERE {field}={value}";
        }

        public string SelectQuery(string columns = "*", string options = "")
        {
            return $"SELECT {columns} FROM {_tableName} {options}";
        }

        public string InsertQuery(string[] columns, string options = "")
        {
            return $"INSERT INTO {_tableName} ({StringerArray(columns)}) VALUES({StringerValues(columns)}) {options}";
        }

        public string UpdateQuery(string[] columns, string options = "")
        {
            return $"UPDATE {_tableName} SET {StringerUpdateSets(columns)} {options}";
        }

        public string DeleteQuery(string options = "")
        {
            return $"DELETE FROM {_tableName} {options}";
        }
        
    }
}