using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WhiteMvvm.Bases;

namespace WhiteMvvm.Extensions
{
    public static class EnumerableExtension
    {
        public static IEnumerable<TModel> ToModel<TTransitional, TModel>(this IEnumerable<TTransitional> transionals , Func<TTransitional, TModel> mapping) 
            where TTransitional : BaseTransitional, new() where TModel : BaseModel, new()
        {
            return transionals.Select(transional => transional?.ToModel(mapping));
        }
    }
}
