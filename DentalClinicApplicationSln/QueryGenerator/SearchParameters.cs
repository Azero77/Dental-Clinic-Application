using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryGenerator
{
    public class SearchParameter
    {
        readonly string _propertyName;
        readonly string _val;
        public Dictionary<SearchOperators, string> Operators;

        public SearchParameter(string propertyName, string val)
        {
            _propertyName = propertyName;
            _val = val;
            Operators = new()
            {
                {SearchOperators.Equal,$"{_propertyName} = '{_val}'" },
                {SearchOperators.GreaterThan, $"{_propertyName} > {_val}" },
                {SearchOperators.LessThan, $"{_propertyName} < {_val}" },
                {SearchOperators.Like, $"{_propertyName} LIKE '%{_val}'" },
                {SearchOperators.SortAscending, $"ORDER BY {_propertyName}" },
                {SearchOperators.SortDescinding, $"ORDER BY {_propertyName} DESC" }
            };
            
        }
        [Flags]
        public enum SearchOperators
        {
            Equal = 0,
            GreaterThan = 1,
            LessThan = 2,
            Like = 4,
            SortDescinding = 8,
            SortAscending = 16,
            Group = 32
        }
        
        private IEnumerable<string> GetListOperators(SearchOperators searchOperator)
        {
            //list contains all the predicate
            //{firstName = "anas"
            List<string> searchOperatorStrings = new();

            foreach (SearchOperators op in Operators.Keys)
            {
                if ((searchOperator & op) == op)
                {
                    searchOperatorStrings.Add(Operators[op]);
                }
            }
            return searchOperatorStrings;
        }

        /// <summary>
        /// Get a list of instructions and combine them in one sql
        /// </summary>
        /// <param name="searchOperatorStrings"></param>
        /// <returns></returns>
        private string CombineOperators(IEnumerable<string> searchOperatorStrings)
        {
            return string.Join(' ', searchOperatorStrings);
        }

        public string GetPredicate(SearchOperators operators)
        {
            IEnumerable<string> strings = GetListOperators(operators);
            return CombineOperators(strings);            
        }
    }
}
