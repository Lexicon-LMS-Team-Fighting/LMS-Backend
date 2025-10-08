using System.Linq.Expressions;
using System.Reflection;

namespace LMS.Shared.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> OrderByField<T>(this IQueryable<T> source, string fieldName, string direction = "asc")
        {
            if (string.IsNullOrWhiteSpace(fieldName))
                return source;

            var propertyInfo = typeof(T).GetProperty(fieldName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (propertyInfo == null)
            {
                var fieldInfo = typeof(T).GetField(fieldName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                
                if (fieldInfo == null)
                    return source;

                var paramF = Expression.Parameter(typeof(T), "x");
                var field = Expression.Field(paramF, fieldInfo);
                var lambdaF = Expression.Lambda(field, paramF);
                string methodNameF = direction.ToLower() == "desc" ? "OrderByDescending" : "OrderBy";

                var methodF = typeof(Queryable).GetMethods()
                    .First(m => m.Name == methodNameF && m.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(T), fieldInfo.FieldType);

                var resultF = methodF.Invoke(null, new object[] { source, lambdaF });

                return resultF as IQueryable<T> ?? source;
            }

            // Если свойство найдено
            var param = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(param, propertyInfo);
            var lambda = Expression.Lambda(property, param);
            string methodName = direction.ToLower() == "desc" ? "OrderByDescending" : "OrderBy";

            var method = typeof(Queryable).GetMethods()
                .First(m => m.Name == methodName && m.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), propertyInfo.PropertyType);

            var result = method.Invoke(null, new object[] { source, lambda });

            return result as IQueryable<T> ?? source;
        }

        public static IQueryable<T> WhereContains<T>(this IQueryable<T> source, string fieldName, string? value)
        {
            if (string.IsNullOrWhiteSpace(fieldName) || string.IsNullOrWhiteSpace(value))
                return source;

            var propertyInfo = typeof(T).GetProperty(fieldName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (propertyInfo == null)
            {
                var fieldInfo = typeof(T).GetField(fieldName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (fieldInfo == null || fieldInfo.FieldType != typeof(string))
                    return source;

                var paramF = Expression.Parameter(typeof(T), "x");
                var field = Expression.Field(paramF, fieldInfo);
                var methodF = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                var containsCallF = Expression.Call(field, methodF!, Expression.Constant(value));
                var lambdaF = Expression.Lambda<Func<T, bool>>(containsCallF, paramF);

                return source.Where(lambdaF);
            }

            if (propertyInfo.PropertyType != typeof(string))
                return source;

            var param = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(param, propertyInfo);
            var method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            var containsCall = Expression.Call(property, method!, Expression.Constant(value));
            var lambda = Expression.Lambda<Func<T, bool>>(containsCall, param);

            return source.Where(lambda);
        }
    }
}
