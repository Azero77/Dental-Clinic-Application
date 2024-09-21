using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QueryGenerator.SearchParameter;

namespace QueryGenerator
{
    public class QueryGenerator
    {

        public string GeneratePredicate(
            string sql
            ,string propertyName
            ,string val
            , Type entityType
            ,SearchOperators searchOperators
            
            )
            
        {
            SearchParameter searchParamters = new(propertyName, val);
            //The string to add to sql query
            string operation = GetOperation(searchParamters,searchOperators);
            return operation;
        }

        private string GetOperation(SearchParameter parameters,
            SearchOperators searchOperators)
        {
            return parameters.GetPredicate(searchOperators);
        }
    }
}
