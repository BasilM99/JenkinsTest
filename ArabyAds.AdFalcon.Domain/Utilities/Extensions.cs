using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Domain.Utilities
{
    public static class Extensions
    {


        public static Expression<TDelegate> AndAlso<TDelegate>(this Expression<TDelegate> left, Expression<TDelegate> right)
        {
            return Expression.Lambda<TDelegate>(Expression.AndAlso(left, right), left.Parameters);
        }

        public static IEnumerable<T> Repeat<T>(this IEnumerable<T> source, int times)
        {
            source = source.ToArray();
            return Enumerable.Range(0, times).SelectMany(_ => source);
        }
    }
}
