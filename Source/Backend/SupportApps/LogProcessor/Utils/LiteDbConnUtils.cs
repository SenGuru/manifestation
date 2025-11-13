using LiteDB;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace LogProcessor.Utils
{
    public static class LiteDbConnUtils
    {
        public static ConnectionString SharedConnection(string Path)
        {
            return new ConnectionString($"{Path}")
            {
                Connection = ConnectionType.Shared
            };
        }

        public static ILiteQueryableResult<T> ApplyPaging<T>(this ILiteQueryable<T> obj, int pageNumber, int pageSize)
        {
            if (pageSize == 0)
            {
                pageSize = 100;
            }

            if (pageNumber == 0)
            {
                return obj.Limit(pageSize);
            }

            pageNumber -= 1; //Skip records of previous pages only
            return obj.Limit(pageSize).Offset(pageNumber * pageSize);
        }

        public static ILiteQueryable<TSource> ApplySortingOrderBy<TSource>(this ILiteQueryable<TSource> query, string propertyName, bool isAsc)
        {
            try
            {
                if (propertyName == null || propertyName.Trim() == "")
                {
                    propertyName = "ModifiedDate";
                    isAsc = false;
                }

                PropertyInfo propertyInfo = null;
                var entityType = typeof(TSource);

                foreach (var prop in entityType.GetProperties())
                {
                    if (prop == null || prop.Name == null)
                        continue;

                    if (prop.Name.ToLower().Trim() == propertyName.ToLower().Trim())
                    {
                        propertyInfo = prop;
                        break;
                    }
                }

                if (propertyInfo == null)
                    return query;

                ParameterExpression parameterExpression = Expression.Parameter(entityType, "x");
                MemberExpression property = Expression.Property(parameterExpression, propertyName);
                var parameters = new ParameterExpression[] { parameterExpression };
                var selector = Expression.Lambda(property, parameters);
                var enumarableType = typeof(LiteQueryable<TSource>);
                var method = GetMethodInfo(isAsc, enumarableType).Single();

                MethodInfo genericMethod = method
                        .MakeGenericMethod(entityType, propertyInfo.PropertyType);

                var newQuery = (ILiteQueryable<TSource>)genericMethod
                        .Invoke(genericMethod, new object[] { query, selector });

                return newQuery;
            }
            catch
            {
                return query;
            }
        }

        private static IEnumerable<MethodInfo> GetMethodInfo(bool isAsc, Type enumarableType)
        {
            return enumarableType.GetMethods()
                                .Where(m => m.Name == (isAsc ? "OrderBy" : "OrderByDescending") && m.IsGenericMethodDefinition)
                                .Where(m =>
                                {
                                    var parameters = m.GetParameters().ToList();
                                    return parameters.Count == 2;
                                });
        }
    }
}
