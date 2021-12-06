// Created By abdulrahman elheyb
// 2021-11-28 1:57 PM

using System;
using System.Text.RegularExpressions;

namespace APICore
{
    public static class Validation
    {
        /// <summary>
        /// Validates the email is entered correctly or not 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool Email(string text)
        {
            var rgx = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            return rgx.IsMatch(text);
        }
        
        /// <summary>
        /// Validates the datetime is greater than default min value
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static bool Date(DateTime dateTime)
        {
            return dateTime > DateTime.MinValue;
        }
    }
}