﻿using DataAccessLayer;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assignment11
{
    class Program
    {
        static void Main(string[] args)
        {

            string connectionString =
            "Data Source=(local);Initial Catalog=school;"
            + "Integrated Security=true";

            DAL dal = new DAL
            {
                ConnectionString = connectionString
            };

            var a = dal.GetData<Users>("usp3", new KeyValuePair<string, object>[] { new KeyValuePair<string, object>("@ID", 2)});
            foreach(var b in a)
            {
                Console.WriteLine($"{b.ID}   {b.FirstName}   {b.LastName}");
            }
        }
    }
}
