// Created By abdulrahman elheyb
// 2021-11-28 1:57 PM

using System.Text.RegularExpressions;

namespace APICore
{
    public static class Validation
    {
        public static bool Email(string text)
        {
            var rgx = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            return rgx.IsMatch(text);
        }
    }
}