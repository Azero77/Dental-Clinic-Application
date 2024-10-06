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
            DbConnection connection = GetConnection();
            connection.Open();
            T? result = default;
            try
            {
                result = func(connection);
            }
            catch
            {
                throw;
            }
            finally
            {
                connection.Close();
            }
            return result;
        }

        public async Task<T> RunAsync<T>(Func<DbConnection,Task<T>> func)
        {
            DbConnection connection = GetConnection();
            await connection.OpenAsync();
            T? result = default;
            try
            {
                result = await func(connection);
            }
            catch
            {
                throw;
            }
            finally
            {
                await connection.CloseAsync();
            }
            return result;
        }
    }

}
#pragma warning restore

