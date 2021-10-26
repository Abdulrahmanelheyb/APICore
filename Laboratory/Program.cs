using System;
using APICore;

namespace Laboratory
{
    internal static class Program
    {
        private static void Main()
        {
            var query = new SqlQuery("users");
            var x = query.Select("city", "Id, Name")
                .LeftJoin("schools", "usr.Id = sc.UserId")
                .Where("Id", "(1,2,3)", WhereClauseOperators.In).AndWhere("Username", true);
            Console.WriteLine(x);
            Console.ReadKey();
        }
    }
}