using System;
using EmbedSerialDB;

namespace UsingTest
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var test = new ESDB(true))
            {
                var x = test.LoadDatabase("voitures", true);
                
                /// Make an insert
                var insertId = x.Insert(new { first_name = "clint", last_name = "mourlevat", age = 29 });

                /// Search one by tag
                var t = x.SearchOne(new { id = "5fd73f1a-ab0f-4112-bf7c-7549f6ed0aa0", lol = 30 });

                Console.WriteLine(t.first_name);

                /// Export all the db in zip file
                test.Export("lol.zip");
            }
        }
    }
}
