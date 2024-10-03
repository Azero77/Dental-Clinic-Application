using AutoMapper;
using Configurations.DataContext;
using Dapper;
using DentalClinicApp.Models;
using DentalClinicApplication.DTOs;
using DentalClinicApplication.Stores;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApplication.Services.DataProvider
{
    public class VirtualizedProvider<T, TDTO>
        : IVirtualizationItemsProvider<T>
    {
        public DbContext DataContext { get; }
        public MessageService MessageService { get; }

        public readonly IMapper _mapper;
        protected string? whereClause = string.Empty;
        protected string? orderByClause = string.Empty;
        Lazy<Task<int>> _initializeCount;
        public VirtualizedProvider(DbContext dbContext,
                                    IMapper mapper,
                                    MessageService messageService,
                                    string? whereClause = "",
                                    string? orderByClause = "")
        {
            _initializeCount = new Lazy<Task<int>>(InitializeCount);
            DataContext = dbContext;
            _mapper = mapper;
            MessageService = messageService;
            this.whereClause = whereClause;
            this.orderByClause = orderByClause;
            //if the sql was not provided then the provider will the table of T and get all items virtualized
        }



        protected virtual async Task<int> InitializeCount()
        {
            string tableName = typeof(T).Name + "s";

            string sql = $"SELECT COUNT(*) FROM {tableName} {whereClause};";
            return await RunSqlCount(sql);
        }

        protected async Task<int> RunSqlCount(string sql)
        {
            return await DataContext.RunAsync<int>(async conn =>
            {
                try
                {
                    return await conn.ExecuteScalarAsync<int>(sql);
                }
                catch (SQLiteException)
                {
                    MessageService.SetMessage("Search Schema is not availabe",
                        MessageType.Error);
                    return 0;
                }
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
            string sql = $"SELECT * FROM {tableName} {orderByClause} {whereClause} LIMIT @size OFFSET @start;";
            return await RunSqlRange(start, size, sql);
        }

        protected async Task<IEnumerable<T>> RunSqlRange(int start, int size, string sql)
        {
            object param = new
            {
                start = start,
                size = size
            };
            IEnumerable<TDTO> typeDTOs = await DataContext.RunAsync<IEnumerable<TDTO>>(async conn =>
            {
                try
                {
                    return await conn.QueryAsync<TDTO>(sql, param);
                }
                catch (SQLiteException)
                {
                    MessageService.SetMessage("Search Schema is not allowed",
                        MessageType.Error);
                    return Enumerable.Empty<TDTO>();
                }
            });
            return typeDTOs.Select(cDTO => _mapper.Map<T>(cDTO));
        }

        //change provider for both order by or where clause depending on what is null
        public IProvider<T> ChangeProvider
            (string? whereClause,
            string? orderByClause)
        {
            return new VirtualizedProvider<T, TDTO>(
                this.DataContext,
                this._mapper,
                this.MessageService,
                whereClause ?? this.whereClause,
                orderByClause ?? this.orderByClause
                );
        }

    }
}
