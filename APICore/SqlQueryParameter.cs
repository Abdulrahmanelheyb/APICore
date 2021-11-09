// ReSharper disable All
// Created By abdulrahman elheyb
// 2021-11-09 4:31 PM

using System;
using JetBrains.Annotations;

namespace APICore
{
    /// <summary>
    /// Represents string replacing in the sql query
    /// </summary>
    public record SqlQueryParameter
    {
        [UsedImplicitly]
        public int Index { get; set; }
        public string Name { get; init; }
        [UsedImplicitly]
        public object Value { get; set; }

        public void SetValue(object value)
        {
            if (value is null) Value = "";
            
            Value = value switch
            {
                int => $"{value}",
                bool => $"{value.ToString()?.ToLower()}",
                string str => str.Contains('(') && str.Contains(')')? $"{str}" : $"'{str}'",
                DateTime => $"'{value}'",
                _ => $"'{value}'"
            };
        }
    }
}