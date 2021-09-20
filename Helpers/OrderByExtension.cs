using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.Text;
using System.Linq.Dynamic.Core;

namespace WorldEvents.API.Helpers
{
    public static class OrderByExtension
    {
        public static IQueryable<T> ApplySort<T>(this IQueryable<T> source, string orderByQueryString)
        {
            if (!source.Any())
            {
                return source;
            }

            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return source;
            }

            var OrderParam = orderByQueryString.Trim().Split(',');
            var propretyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance); // Specify BindingFlags.Public to include public properties in the search. A property is considered public to reflection if it has at least one accessor that is public. | Specify BindingFlags.Instance to include instance methods
            var orderQueryBuilder = new StringBuilder();
            foreach (var param in OrderParam)
            {
                if (string.IsNullOrWhiteSpace(param))
                {
                    continue;
                }

                var proprtyFromQueryName = param.Trim().Split(" ")[0];
                var objectProprty = propretyInfos.FirstOrDefault(pi => pi.Name.Equals(proprtyFromQueryName, StringComparison.InvariantCultureIgnoreCase));
                if (objectProprty == null)
                {
                    continue;
                }

                var sortingOrder = param.EndsWith(" desc") ? "descending" : "ascending";

                orderQueryBuilder.Append($"{objectProprty.Name.ToString()} {sortingOrder},");
            }

            var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');

            return source.OrderBy(orderQuery); //using System.Linq.Dynamic.Core;
        }
    }
}
