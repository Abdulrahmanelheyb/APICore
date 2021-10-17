using System.Collections.Generic;

namespace APICore
{
    public class Queries
    {
        private readonly string _tableName;

        public Queries(string tableName)
        {
            _tableName = tableName;
        }

        #region Helper methods

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

        #endregion

        public static string WhereQuery(string field, object value, string more = "")
        {
            return !ReferenceEquals(value, typeof(int)) ? $" WHERE {field}='{value}'" : $"WHERE {field}={value} {more} ";
        }

        public string Select(string columns = "*", string options = "")
        {
            return $" SELECT {columns} FROM {_tableName} {options} ";
        }

        public static string SelectQuery(string tablename, string columns = "*", string options = "")
        {
            return $" SELECT {columns} FROM {tablename} {options} ";
        }

        public string Insert(string[] columns, string options = "")
        {
            return $" INSERT INTO {_tableName} ({StringerArray(columns)}) VALUES({StringerValues(columns)}) {options} ";
        }

        public string Update(string[] columns, string options = "")
        {
            return $" UPDATE {_tableName} SET {StringerUpdateSets(columns)} {options} ";
        }

        public string Delete(string options = "")
        {
            return $" DELETE FROM {_tableName} {options} ";
        }
    }
}