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
        protected string? _whereClause;
        protected string? _orderClause;
        public Provider(DbContext dataContext, IMapper mapper)
        {
            DataContext = dataContext;
            _mapper = mapper;
        }
        public abstract Task<IEnumerable<T>> GetItems();
        public abstract Task<T?> GetItem(string propertyName, object value);
        public Task<T?> GetItem(int id)
        {
            return GetItem("Id", id);
        }
        public abstract void ChangeProvider(string? whereClause, string? orderClause);
        public virtual void ResetProvider()
        {
            _whereClause = null;
            _orderClause = null;
        }

    }
}
