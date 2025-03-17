using System.Reflection;
using Core.HttpClient.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Core.HttpClient.Extensions
{
    /// <summary>
    /// <see cref="QueryString"/> extension methods to support classes with query parameters.
    /// </summary>
    public static class QueryStringExtension
    {
        /// <summary>
        /// Create a <see cref="QueryString"/> from <typeparamref name="T"/> parameters.
        /// Gets the name from <see cref="QueryParamNameAttribute"/> or
        /// the property name in lowercase if not found.
        /// Property with <see cref="IgnoreQueryParamAttribute"/> will be ignored.
        /// </summary>
        /// <typeparam name="T">the query class containing the query parameters.</typeparam>
        /// <param name="instance">the instance to get the property value.</param>
        /// <returns><see cref="QueryString"/> with all the parameters.</returns>
        public static QueryString Create<T>(T instance)
            where T : class
        {
            List<KeyValuePair<string, StringValues>> parameters = [];

            IEnumerable<PropertyInfo> properties = typeof(T)
                .GetProperties()
                .Where(property => property.GetCustomAttributes(typeof(IgnoreQueryParamAttribute), false).Length == 0);

            foreach (PropertyInfo property in properties)
            {
                QueryParamNameAttribute? attribute = property.GetCustomAttribute<QueryParamNameAttribute>();

                object? value = property.GetValue(instance);

                if (value is not null)
                {
                    StringValues stringValues = value switch
                    {
                        Array array => new StringValues(array.Cast<object>().Select(v => v.ToString()).ToArray()),
                        _ => new StringValues(value.ToString())
                    };

                    if (stringValues.Count > 0)
                    {
                        parameters.Add(new(attribute?.Name ?? property.Name.ToLowerInvariant(), stringValues));
                    }
                }
            }

            return QueryString.Create(parameters);
        }
    }
}
