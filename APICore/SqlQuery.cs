using System;
using System.Collections.Generic;

namespace APICore
{
    public enum WhereClauseOperators
    {
        Equal,
        NotEqual,
        GreaterThen,
        LessThen,
        GreaterThenOrEqual,
        LessThenOrEqual,
        Between,
        Like,
        In        
    }
    
    public class SqlQuery
    {
        // ReSharper disable once MemberCanBePrivate.Global
        public readonly string TableName;
        private string _query;
        public string Query
        {
            get => Trimmer(_query);
            set => _query = value;
        }

        /// <summary>
        /// Creates instance of query class with defined database table name
        /// </summary>
        /// <param name="tableName">Default table name in the database</param>
        public SqlQuery(string tableName)
        {
            TableName = tableName;
        }

        #region Helper methods

        private static string WhereClauseOperatorsMapper(WhereClauseOperators operatorType)
        {
            return operatorType switch
            {
                WhereClauseOperators.NotEqual => " <> ",
                WhereClauseOperators.Equal => " = ",
                WhereClauseOperators.GreaterThen => " > ",
                WhereClauseOperators.LessThen => " < ",
                WhereClauseOperators.GreaterThenOrEqual => " >= ",
                WhereClauseOperators.LessThenOrEqual => " <= ",
                WhereClauseOperators.Like => " LIKE ",
                WhereClauseOperators.In => " IN ",
                _ => ""
            };
        }

        private static object ValueTypeChecker(object value)
        {
            return value switch
            {
                int or bool => $"{value}",
                DateTime dateTime => $"'{dateTime:yyyy-MM-dd}'",
                string str => str.Contains('(') && str.Contains(')')? $"{str}" : $"'{str}'",
                _ => $"'{value}'"
            };
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
        /// <param name="operatorType">Operator type for where clause.</param>
        /// <returns>SqlQuery object</returns>
        public SqlQuery Where(string field, object value, WhereClauseOperators operatorType = WhereClauseOperators.Equal)
        {
            _query += $" WHERE {field}{WhereClauseOperatorsMapper(operatorType)}{ValueTypeChecker(value)} ";
            return this;
        }
        
        /// <summary>
        /// Creates where SQL query without where word.
        /// </summary>
        /// <param name="field">Column's name</param>
        /// <param name="value">Column's value</param>
        /// <returns>SqlQuery object</returns>
        public SqlQuery AndWhere(string field, object value)
        {
            _query += $" AND {field}={ValueTypeChecker(value)} ";
            return this;
        }

        
        /// <summary>
        /// The add 'and' operator before query
        /// </summary>
        /// <returns>SqlQuery object</returns>
        public SqlQuery And()
        {
            _query += " AND ";
            return this;
        }

        public SqlQuery As()
        {
            _query += " AS ";
            return this;
        }

        /// <summary>
        /// Creates the select SQL query
        /// </summary>
        /// <param name="columns">Columns in the table</param>
        /// <param name="tableName"></param>
        /// <returns>SqlQuery object</returns>
        public SqlQuery Select(string tableName, string columns = "*")
        {
            _query += $" SELECT {columns} FROM {tableName} ";
            return this;
        }

        /// <summary>
        /// Creates the insert SQL query
        /// </summary>
        /// <param name="columns">Columns in the table</param>
        /// <returns>SqlQuery object</returns>
        public SqlQuery Insert(string[] columns)
        {
            _query += $" INSERT INTO {TableName} ({StringerArray(columns)}) VALUES({StringerValues(columns)}) ";
            return this;
        }

        /// <summary>
        /// Creates the update SQL query
        /// </summary>
        /// <param name="columns">Columns in the table</param>
        /// <returns>SqlQuery object</returns>
        public SqlQuery Update(string[] columns)
        {
            _query += $" UPDATE {TableName} SET {StringerUpdateSets(columns)} ";
            return this;
        }

        /// <summary>
        /// Creates the update SQL query
        /// </summary>
        /// <returns>SqlQuery object</returns>
        public SqlQuery Delete()
        {
            _query += $" DELETE FROM {TableName} ";
            return this;
        }

        #endregion

        #region Joins

        public SqlQuery InnerJoin(string tableName, string joinOn)
        {
            _query += $" INNER JOIN {tableName} ON {joinOn} ";
            return this;
        }

        public SqlQuery LeftJoin(string tableName, string joinOn)
        {
            _query += $" LEFT JOIN {tableName} ON {joinOn} ";
            return this;
        }

        public SqlQuery LeftOuterJoin(string tableName, string joinOn)
        {
            _query += $" LEFT OUTER JOIN {tableName} ON {joinOn} ";
            return this;
        }

        public SqlQuery RightJoin(string tableName, string joinOn)
        {
            _query += $" RIGHT JOIN {tableName} ON {joinOn} ";
            return this;
        }

        public SqlQuery RightOuterJoin(string tableName, string joinOn)
        {
            _query += $" RIGHT OUTER JOIN {tableName} ON {joinOn} ";
            return this;
        }

        public override string ToString()
        {
            return Trimmer(_query);
        }

        #endregion
    }
}