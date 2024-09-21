using Configurations.DataContext;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApplication.Services.DataProvider
{
    public abstract class VirtualizedProvider<T>
        : IVirtualizationItemsProvider<T>,
        IParameterizedProvider<T>
    {
        public string Predicate { get; }
        public string TableName { get; }
        public DbContext DbContext { get; }

        Lazy<Task<int>> _initializeCount;
        public VirtualizedProvider(DbContext dbContext, string predicate, string tableName)
        {
            _initializeCount = new Lazy<Task<int>>(InitializeCount);
            DbContext = dbContext;
            Predicate = predicate;
            TableName = tableName;
        }

        private async Task<int> InitializeCount()
        {
            string sql = $"SELECT COUNT(*) FROM {TableName} {Predicate};";
            return await DbContext.RunAsync<int>(async conn =>
            {
                return await conn.ExecuteScalarAsync<int>(sql);
            });
        }

        public async Task<int> FetchCount()
        {
            return await _initializeCount.Value;
        }

        public async Task<IList<T>> FetchRange(int start, int size)
        {
            return (await GetItems(start, size)).ToList();
        }
        

        public async Task<IEnumerable<T>> GetItems()
        {
            return await FetchRange(0, 20);
        }

        public abstract Task<IEnumerable<T>> GetItems(int start, int size);
    }
}
