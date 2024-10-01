using AutoMapper;
using Configurations.DataContext;
using DentalClinicApp.Models;
using DentalClinicApplication.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApplication.Services.DataProvider
{
    public class ClientsProvider
        : Provider<Client, ClientDTO>
    {
        string? _whereClause;
        string? _orderClause;
        public ClientsProvider(DbContext dataContext,
                                    IMapper mapper,
                                    string? whereClause = null,
                                    string? orderClause = null) : base(dataContext, mapper)
        {
            //if it was null then the values do not change
            _whereClause = whereClause;
            _orderClause = orderClause;
        }

        public override IProvider<Client> ChangeProvider(string? whereClause, string? orderClause)
        {
            return new ClientsProvider(this.DataContext, this._mapper, whereClause ?? this._whereClause,
               orderClause ?? this._orderClause);
        }


        public override Task<IEnumerable<Client>> GetItems()
        {
            throw new NotImplementedException();
        }
    }
}
