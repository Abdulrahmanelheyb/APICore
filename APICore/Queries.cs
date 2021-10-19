using System;
using System.Collections.Generic;

namespace APICore
{
    public class Queries
    {
        private readonly string _tableName;
        
        /// <summary>
        ///     Creates instance of query class with defined database table name
        /// </summary>
        /// <param name="tableName">Table name in the database</param>
        public Queries(string tableName)
        {
            _tableName = tableName;
        }

        #region Helper methods

        private static string Trimmer(string value)
        {
            return string.Join(' ', value.Split(' ', StringSplitOptions.RemoveEmptyEntries));
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
                if (column == data[^1])
                {
                    rlt += $"{column}=@{column}";
                    break;
                }
                
                rlt += $"{column}=@{column}, ";
            }

            return rlt;
        }

        #endregion
        
        /// <summary>
        ///     Creates the where SQL query
        /// </summary>
        /// <param name="field">Column's name</param>
        /// <param name="value">Column's value</param>
        /// <param name="more">Can add more option to where statement</param>
        /// <returns>string</returns>
        public static string WhereQuery(string field, object value, string more = "")
        {
            return Trimmer(!ReferenceEquals(value, typeof(int)) ? $" WHERE {field}='{value}' {more}" : $"WHERE {field}={value} {more} ");
        }
        
        /// <summary>
        ///     Creates the select SQL query
        /// </summary>
        /// <param name="columns">Columns in the table</param>
        /// <param name="options">Can add more options here ex: where statement etc.</param>
        /// <returns>string</returns>
        public string Select(string columns = "*", string options = "")
        {
            return Trimmer($" SELECT {columns} FROM {_tableName} {options} ");
        }
        
        /// <summary>
        ///     Creates the select SQL query
        /// </summary>
        /// <param name="tablename">Table name in database</param>
        /// <param name="columns">Columns in the table</param>
        /// <param name="options">Can add more options here ex: where statement etc.</param>
        /// <returns>string</returns>
        public static string SelectQuery(string tablename, string columns = "*", string options = "")
        {
            return Trimmer($" SELECT {columns} FROM {tablename} {options} ");
        }
        
        /// <summary>
        ///     Creates the insert SQL query
        /// </summary>
        /// <param name="columns">Columns in the table</param>
        /// <param name="options">Can add more options here</param>
        /// <returns>string</returns>
        public string Insert(string[] columns, string options = "")
        {
            return Trimmer($" INSERT INTO {_tableName} ({StringerArray(columns)}) VALUES({StringerValues(columns)}) {options} ");
        }

        /// <summary>
        ///     Creates the update SQL query
        /// </summary>
        /// <param name="columns">Columns in the table</param>
        /// <param name="options">Can add more options here</param>
        /// <returns>string</returns>
        public string Update(string[] columns, string options = "")
        {
            return Trimmer($" UPDATE {_tableName} SET {StringerUpdateSets(columns)} {options} ");
        }
        
        /// <summary>
        ///     Creates the update SQL query
        /// </summary>
        /// <param name="options">Can add more options here</param>
        /// <returns>string</returns>
        public string Delete(string options = "")
        {
            return Trimmer($" DELETE FROM {_tableName} {options} ");
        }
    }
}