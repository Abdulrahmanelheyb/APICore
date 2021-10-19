using System;
using APICore;

namespace Laboratory
{
    internal static class Program
    {
        private static void Main()
        {
            var query = new Queries("test");
            var x = query.Select(options: Queries.WhereQuery("Id", 1, $" and {Queries.WhereQuery("Name", "Abdulrahman")}"));
            Console.WriteLine(x);
            Console.ReadKey();
        }
    }
}