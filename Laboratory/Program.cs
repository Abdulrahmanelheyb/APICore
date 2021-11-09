using System;
using APICore;

namespace Laboratory
{
    internal static class Program
    {
        private static void Main()
        {
            SqlQueryParameterizer.BaseFolder = "Queries";
            var x = new SqlQueryParameterizer("users", "getlist");
            x.SetParameter("My Name", x);
            var z = x.Query;            
            Console.WriteLine(z);
            Console.ReadKey();
        }
    }
}