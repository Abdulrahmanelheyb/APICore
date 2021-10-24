using System;
using System.Collections.Generic;

namespace APICore
{
    public class SqlQuery
    {
        private readonly string _tableName;
        private string _query;

        /// <summary>
        /// Creates instance of query class with defined database table name
        /// </summary>
        /// <param name="tableName">Table name in the database</param>
        public SqlQuery(string tableName)
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
                if (column != data[^1])
                    rlt += $"{column}, ";
                else
                    rlt += $"{column}";

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
                if (value != data[^1])
                    rlt += $"@{UpperFirstChar(value)}, ";
                else
                    rlt += $"@{UpperFirstChar(value)}";

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

        #region Basic Queries

        /// <summary>
        /// Creates the where SQL query
        /// </summary>
        /// <param name="field">Column's name</param>
        /// <param name="value">Column's value</param>
        /// <returns>SqlQuery object</returns>
        public SqlQuery Where(string field, object value)
        {
            if (ReferenceEquals(value, typeof(int)) || ReferenceEquals(value, typeof(bool)))
            {
                _query += $"WHERE {field}={value} ";
            }
            else if (Re)
            {
                
            }
            else
            {
                _query +=  $" WHERE {field}='{value}' ";
            }
            return this;
        }

        
        /// <summary>
        /// The add 'and' operator before query
        /// </summary>
        /// <returns>SqlQuery object</returns>
        public SqlQuery And()
        {
            _query += " and ";
            return this;
        }

        /// <summary>
        /// Creates the select SQL query
        /// </summary>
        /// <param name="columns">Columns in the table</param>
        /// <returns>SqlQuery object</returns>
        public SqlQuery Select(string columns = "*")
        {
            _query += Trimmer($" SELECT {columns} FROM {_tableName} ");
            return this;
        }

        /// <summary>
        /// Creates the insert SQL query
        /// </summary>
        /// <param name="columns">Columns in the table</param>
        /// <returns>SqlQuery object</returns>
        public SqlQuery Insert(string[] columns)
        {
            _query += Trimmer($" INSERT INTO {_tableName} ({StringerArray(columns)}) VALUES({StringerValues(columns)}) ");
            return this;
        }

        /// <summary>
        /// Creates the update SQL query
        /// </summary>
        /// <param name="columns">Columns in the table</param>
        /// <returns>SqlQuery object</returns>
        public SqlQuery Update(string[] columns)
        {
            _query += Trimmer($" UPDATE {_tableName} SET {StringerUpdateSets(columns)} ");
            return this;
        }

        /// <summary>
        /// Creates the update SQL query
        /// </summary>
        /// <returns>SqlQuery object</returns>
        public SqlQuery Delete()
        {
            _query += Trimmer($" DELETE FROM {_tableName} ");
            return this;
        }

        #endregion

        #region Joins

        public SqlQuery InnerJoin(string tableName, string joinOn)
        {
            _query += Trimmer($" INNER JOIN {tableName} ON {joinOn} ");
            return this;
        }

        public SqlQuery LeftJoin(string tableName, string joinOn)
        {
            _query += Trimmer($" LEFT JOIN {tableName} ON {joinOn} ");
            return this;
        }

        public SqlQuery LeftOuterJoin(string tableName, string joinOn)
        {
            _query += Trimmer($" LEFT OUTER JOIN {tableName} ON {joinOn} ");
            return this;
        }

        public SqlQuery RightJoin(string tableName, string joinOn)
        {
            _query += Trimmer($" RIGHT JOIN {tableName} ON {joinOn} ");
            return this;
        }

        public SqlQuery RightOuterJoin(string tableName, string joinOn)
        {
            _query += Trimmer($" RIGHT OUTER JOIN {tableName} ON {joinOn} ");
            return this;
        }
        
        #endregion

        public override string ToString()
        {
            return _query;
        }
    }
}