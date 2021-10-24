using System;
using APICore;

namespace Laboratory
{
    internal static class Program
    {
        private static void Main()
        {
            var query = new SqlQuery("users");
            var x = query.Select().And().Where("Id", DateTime.Now);
            var z = x.ToString();
            Console.WriteLine(z);
            Console.ReadKey();
        }
    }
}