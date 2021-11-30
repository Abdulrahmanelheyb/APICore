using System;
using APICore;

namespace Laboratory
{
    internal static class Program
    {
        private static void Main()
        {
            // SqlQueryParamizer.BaseFolder = "Queries";
            // var x = new SqlQueryParamizer("users", "getlist");
            // var param = x.GetParameter("My Name");
            // param.SetValue(DateTime.Now.ToString("yyyy-MM-dd"));
            // var z = x.Query;   

            var x = new
            {
                Name = "Abdulrahman",
                Age = 19,
                IsStudent = true,
                Degree = 89.0f
            };
            
            var z = ObjectPropertyMapper.GetObjectProperties(x);
            Console.WriteLine();
            Console.ReadKey();
        }
    }
}