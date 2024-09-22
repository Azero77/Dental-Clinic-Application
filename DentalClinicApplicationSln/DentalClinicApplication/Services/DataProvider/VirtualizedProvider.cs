using AutoMapper;
using Configurations.DataContext;
using Dapper;
using DentalClinicApp.Models;
using DentalClinicApplication.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApplication.Services.DataProvider
{
    public class VirtualizedProvider<T, TDTO>
        : IVirtualizationItemsProvider<T>
    {
        public DbContext DbContext { get; }
        public readonly IMapper _mapper;
        protected string? whereClause = string.Empty;
        Lazy<Task<int>> _initializeCount;
        public VirtualizedProvider(DbContext dbContext,
                                    IMapper mapper,
                                    string? whereClause = "")
        {
            _initializeCount = new Lazy<Task<int>>(InitializeCount);
            DbContext = dbContext;
            _mapper = mapper;
            this.whereClause = whereClause;
            //if the sql was not provided then the provider will the table of T and get all items virtualized
        }


        
        protected virtual async Task<int> InitializeCount()
        {
            string tableName = typeof(T).Name + "s";

            string sql = $"SELECT COUNT(*) FROM {tableName} {whereClause};";
            return await RunSqlCount(sql);
        }

        private async Task<int> RunSqlCount(string sql)
        {
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

        public virtual async Task<IEnumerable<T>> GetItems(int start, int size)
        {
            string tableName = typeof(T).Name + "s";
            string sql = $"SELECT * FROM {tableName} {whereClause} LIMIT @size OFFSET @start;";
            return await RunSqlRange(start, size, sql);
        }

        protected async Task<IEnumerable<T>> RunSqlRange(int start, int size, string sql)
        {
            object param = new
            {
                start = start,
                size = size
            };
            IEnumerable<TDTO> typeDTOs = await DbContext.RunAsync<IEnumerable<TDTO>>(async conn =>
            {
                return await conn.QueryAsync<TDTO>(sql, param);
            });
            return typeDTOs.Select(cDTO => _mapper.Map<T>(cDTO));
        }

        public IProvider<T> GetNewVirtualizationItemsProvider
            (string newPredicate)
        {
            IProvider<T> newProvider = new VirtualizedProvider<T,TDTO>(
                this.DbContext,
                this._mapper
                );
            return newProvider;
        }
        public IProvider<T> ChangeProvider
            (string whereClause)
        {
            return new VirtualizedProvider<T, TDTO>(
                this.DbContext,
                this._mapper,
                whereClause
                );
        }
    }
}
