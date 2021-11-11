using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

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
    public enum JoinsTypes
    {
        InnerJoin,
        LeftJoin,
        RightJoin,
        LeftOuterJoin,
        RightOuterJoin,
        FullJoin
    }
    
    public class SqlQueryBuilder
    {
        // ReSharper disable once MemberCanBePrivate.Global        
        public readonly string TableName;

        private string _query;
        public string Query => Trimmer(_query);

        /// <summary>
        /// Creates SqlQueryBuilder object
        /// </summary>
        [UsedImplicitly]
        public SqlQueryBuilder()
        {

        }

        /// <summary>
        /// Creates instance of query class with defined database table name
        /// </summary>
        /// <param name="tableName">Default table name in the database</param>
        public SqlQueryBuilder(string tableName)
        {
            TableName = tableName;
        }

        #region Extras

        /// <summary>
        /// Can add any text to query
        /// </summary>
        /// <param name="value">any string</param>
        /// <returns>SqlQueryBuilder object</returns>
        public SqlQueryBuilder Text(string value)
        {
            _query += $" {value} ";
            return this;
        }

        /// <summary>
        /// Adds '\n' New line (Break Line) to query
        /// </summary>
        /// <returns>SqlQueryBuilder object</returns>
        public SqlQueryBuilder Br()
        {
            _query += " \n ";
            return this;
        }

        #endregion        

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
                WhereClauseOperators.Between => " BETWEEN ",
                WhereClauseOperators.Like => " LIKE ",
                WhereClauseOperators.In => " IN ",
                _ => ""
            };
        }

        private static string JoinsTypesMapper(JoinsTypes joinType)
        {
            return joinType switch
            {
                JoinsTypes.InnerJoin => " INNER JOIN ",
                JoinsTypes.LeftJoin => " LEFT JOIN ",
                JoinsTypes.RightJoin => " RIGHT JOIN ",
                JoinsTypes.LeftOuterJoin => " LEFT OUTER JOIN ",
                JoinsTypes.RightOuterJoin => " RIGHT OUTER JOIN ",
                JoinsTypes.FullJoin => " FULL JOIN ",
                _ => ""
            };
        }

        private static object ValueTypeChecker(object value)
        {
            return value switch
            {
                int or bool => $"{value}",
                string str => str.Contains('(') && str.Contains(')') || str.Contains('@')? $"{str}" : $"'{str}'",
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
        /// Creates the select SQL query
        /// </summary>
        /// <returns>SqlQueryBuilder object</returns>
        public SqlQueryBuilder Select()
        {
            _query += $" SELECT * FROM {TableName} ";
            return this;
        }
        
        /// <summary>
        /// Creates the select SQL query
        /// </summary>
        /// <param name="tableName">Table name in database</param>
        /// <returns>SqlQueryBuilder object</returns>
        public SqlQueryBuilder Select(string tableName)
        {
            _query += $" SELECT * FROM {tableName} ";
            return this;
        }
        
        /// <summary>
        /// Creates the select SQL query
        /// </summary>
        /// <param name="columns">Columns in the table</param>
        /// <param name="tableName">Table name in database</param>
        /// <returns></returns>
        public SqlQueryBuilder Select( string[] columns, string tableName)
        {
            _query += $" SELECT {string.Join(',', columns)} FROM {tableName} ";
            return this;
        }

        /// <summary>
        /// Creates the insert SQL query
        /// </summary>
        /// <param name="columns">Columns in the table</param>
        /// <returns>SqlQueryBuilder object</returns>
        public SqlQueryBuilder Insert(string[] columns)
        {
            _query += $" INSERT INTO {TableName} ({string.Join(", ", columns)}) VALUES({StringerValues(columns)}) ";
            return this;
        }

        /// <summary>
        /// Creates the update SQL query
        /// </summary>
        /// <param name="columns">Columns in the table</param>
        /// <returns>SqlQueryBuilder object</returns>
        public SqlQueryBuilder Update(string[] columns)
        {
            _query += $" UPDATE {TableName} SET {StringerUpdateSets(columns)} ";
            return this;
        }

        /// <summary>
        /// Creates the update SQL query
        /// </summary>
        /// <returns>SqlQueryBuilder object</returns>
        public SqlQueryBuilder Delete()
        {
            _query += $" DELETE FROM {TableName} ";
            return this;
        }

        #endregion

        #region Others        
        /// <summary>
        /// Creates the where SQL query
        /// </summary>
        /// <param name="field">Column's name</param>
        /// <param name="value">Column's value</param>
        /// <param name="operatorType">Operator type for where clause.</param>
        /// <returns>SqlQueryBuilder object</returns>
        public SqlQueryBuilder Where(string field, object value, WhereClauseOperators operatorType = WhereClauseOperators.Equal)
        {
            _query += $" WHERE {field}{WhereClauseOperatorsMapper(operatorType)}{ValueTypeChecker(value)} ";
            return this;
        }

        /// <summary>
        /// Creates where SQL query without where word.
        /// </summary>
        /// <param name="field">Column's name</param>
        /// <param name="value">Column's value</param>
        /// <param name="operatorType">Operator type for where clause.</param>
        /// <returns>SqlQueryBuilder object</returns>
        public SqlQueryBuilder AndWhere(string field, object value, WhereClauseOperators operatorType = WhereClauseOperators.Equal)
        {
            _query += $" AND {field}{WhereClauseOperatorsMapper(operatorType)}{ValueTypeChecker(value)} ";
            return this;
        }

        
        /// <summary>
        /// The add 'and' operator before query
        /// </summary>
        /// <returns>SqlQueryBuilder object</returns>
        public SqlQueryBuilder And()
        {
            _query += " AND ";
            return this;
        }

        public SqlQueryBuilder As()
        {
            _query += " AS ";
            return this;
        }

        public SqlQueryBuilder GroupBy(string columns)
        {
            _query += $" GROUP BY {columns} ";
            return this;
        }
        

        #endregion

        #region Joins

        public SqlQueryBuilder JoinOn(JoinsTypes joinType, string tableName, string joinOn)
        {
            _query += $" {JoinsTypesMapper(joinType)} {tableName} ON {joinOn} ";
            return this;
        }

        public SqlQueryBuilder JoinOn(JoinsTypes joinType, SqlQueryBuilder subQuery, string alias, string joinOn)
        {
            _query += $" {JoinsTypesMapper(joinType)} ({subQuery._query}) {alias} ON {joinOn}";
            return this;
        }

        public override string ToString()
        {
            return Trimmer(_query);
        }

        #endregion
    }
}