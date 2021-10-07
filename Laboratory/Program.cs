using System;
using APICore.Models;

namespace Laboratory
{
    class ObjectA
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    internal static class Program
    {
        private static void Main()
        {
            ObjectA a = new ObjectA()
            {
                Id = 1,
                Name = "abdul"
            };

            var x = a;
            x.Name = "abdulrahman";
            Console.WriteLine(a.Name);
            Console.WriteLine(x.Name);
            Console.ReadKey();
        }
    }
}