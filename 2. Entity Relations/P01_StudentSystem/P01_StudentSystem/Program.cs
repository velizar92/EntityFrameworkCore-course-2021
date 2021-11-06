using P01_StudentSystem.Data;
using System;

namespace P01_StudentSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            var dbContext = new StudentSystemContext();

            dbContext.Database.EnsureCreated();

            Console.WriteLine("DB was created! ");

            dbContext.Database.EnsureDeleted();

        }
    }
}
