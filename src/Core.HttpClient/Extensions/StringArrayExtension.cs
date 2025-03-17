using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Core.HttpClient.Extensions
{
    /// <summary>
    /// String Array extension class.
    /// </summary>
    public static class StringArrayExtension
    {
        /// <summary>
        /// Create a <see cref="QueryString"/> from the string array.
        /// </summary>
        /// <param name="stringArray">the string array instance.</param>
        /// <param name="parameterName">the query parameter name.</param>
        /// <returns></returns>
        public static QueryString ToQueryString(this string[] stringArray, string parameterName)
        {
            ArgumentNullException.ThrowIfNull(stringArray);

            List<KeyValuePair<string, StringValues>>? @params = [];

            if (stringArray?.Length > 0)
            {
                @params.Add(new(parameterName ?? nameof(stringArray), stringArray));
            }

            return QueryString.Create(@params);
        }
    }
}
