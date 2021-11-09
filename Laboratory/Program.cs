using System;
using APICore;

namespace Laboratory
{
    internal static class Program
    {
        private static void Main()
        {
            SqlQueryParamizer.BaseFolder = "Queries";
            var x = new SqlQueryParamizer("users", "getlist");
            var param = x.GetParameter("My Name");
            param.SetValue(DateTime.Now.ToString("yyyy-MM-dd"));
            var z = x.Query;            
            Console.WriteLine(z);
            Console.ReadKey();
        }
    }
}