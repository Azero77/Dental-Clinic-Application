using DentalClinicApp.Models;
using DentalClinicApplication.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Text;
using Configurations.DataContext;
using System.Threading.Tasks;
using AutoMapper;

namespace DentalClinicApplication.Services.DataProvider
{
    public abstract class Provider<T,DTO> : IProvider<T>
    {
        public DbContext DataContext { get; }
        public IMapper _mapper { get; }
        public abstract string Sql { get; set; }
        public Provider(DbContext dataContext, IMapper mapper)
        {
            DataContext = dataContext;
            _mapper = mapper;
        }
        public async Task<IEnumerable<T>> GetItems()
        {
            string sql = Sql;
            //await Task.Delay(3000);
            IEnumerable<DTO> ClientsDTO = await DataContext.RunAsync(async (conn) =>
            {
                return await conn.QueryAsync<DTO>(sql);
            });

            return ClientsDTO.Select(cDTo => _mapper.Map<DTO,T>(cDTo));
        }

        public abstract IProvider<T> ChangeProvider(string whereClause);
        
    }
}
