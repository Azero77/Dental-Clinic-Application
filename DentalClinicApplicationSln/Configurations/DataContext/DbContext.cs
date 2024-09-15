using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Configurations.DataContext
{
    public class DbContext
    {
        private string CONNECTION_STRING;
        private IConfigurationRoot Configuration;

        public DbContext()
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appConfigurations.json")
                .Build();
            CONNECTION_STRING = Configuration["ConnectionStrings:ClientsConnectionString"];
        }

        private OleDbConnection GetConnection()
        {
            try
            {
                #pragma warning disable CA1416
                OleDbConnection connection = new OleDbConnection(CONNECTION_STRING);
                return connection;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public T Run<T>(Func<OleDbConnection,T> func)
        {
            OleDbConnection connection = GetConnection();
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

        public async Task<T> RunAsync<T>(Func<OleDbConnection,Task<T>> func)
        {
            OleDbConnection connection = GetConnection();
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

