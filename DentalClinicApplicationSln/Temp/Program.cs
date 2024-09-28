using Configurations.DataContext;
using Dapper;
using System;
using System.Collections.Generic;

namespace Temp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DbContext context = new();
            IEnumerable<DateTime> dateTimes = GetStartDates(context);
            Random random = new();
            
            int id = 1;
            foreach (DateTime dateTime in dateTimes)
            {
                string sql = "UPDATE Appointments " +
                    $"SET EndDate = {dateTime + TimeSpan.FromHours(random.Next(1,3))} " +
                    $"WHERE Id = {id}";
                Console.WriteLine(id);
                context.Run(conn => conn.Execute(sql));
                id++;
            }
        }

        private static IEnumerable<DateTime> GetStartDates(DbContext context)
        {
            return
                context.Run(
                    conn =>
                    {
                        return conn.Query<DateTime>(
                            "SELECT StartDate FROM Appointments;"
                            );
                    });
        }
    }
}
