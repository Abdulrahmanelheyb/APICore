using System;
using System.Reflection;
using APICore;

namespace Laboratory
{
    internal static class Program
    {
        private static void Main()
        {
            var x = new SqlQuery("");
            x.Parameters[0].Value = true;
            var z = x.Query;
            Console.WriteLine(z);
            Console.ReadKey();
        }
    }
}