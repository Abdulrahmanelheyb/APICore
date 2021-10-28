using System;
using System.Reflection;
using APICore;

namespace Laboratory
{
    internal static class Program
    {
        private static void Main()
        {
            var x = new SqlQuery("users", "getlist");
            x.SetParameter("MyName", "ae");
            var z = x.Query;
            Console.WriteLine(z);
            Console.ReadKey();
        }
    }
}