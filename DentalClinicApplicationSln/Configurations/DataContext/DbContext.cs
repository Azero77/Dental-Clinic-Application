using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Configurations.DataContext
{
    public class DbContext
    {
        private string CONNECTION_STRING;
        private IConfiguration Configuration;

        public DbContext(IConfiguration configuration)
        {
            Configuration = configuration;
            CONNECTION_STRING = Configuration["ConnectionStrings:ClientsConnectionString"];
        }


        private DbConnection GetConnection()
        {
            try
            {
                #pragma warning disable CA1416
                DbConnection connection = new SQLiteConnection(CONNECTION_STRING);
                return connection;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public T Run<T>(Func<DbConnection,T> func)
        {
            using (DbConnection connection = GetConnection())
            {
                connection.Open();
                try
                {
                    return func(connection);

                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public async Task<T> RunAsync<T>(Func<DbConnection,Task<T>> func)
        {
            await using (DbConnection connection = GetConnection())
            {
                await connection.OpenAsync();
                try
                {
                    return await (func(connection));
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
    }

}
#pragma warning restore

