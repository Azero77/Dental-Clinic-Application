using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApplication.Services.DataProvider
{
    public interface IParameterizedProvider<T> : IProvider<T>
    {
        string Predicate { get; }
    }
}
