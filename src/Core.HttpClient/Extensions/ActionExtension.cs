using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Core.HttpClient.Extensions
{
    public static class ActionExtension
    {
        /// <summary>
        /// Extension method to perform validation on Required properties.
        /// </summary>
        /// <typeparam name="T">Option class with parameterless constructor.</typeparam>
        /// <param name="action"><see cref="Action"/>delegate</param>
        /// <returns></returns>
        public static T Invoke<T>(this Action<T> action)
            where T : new()
        {
            T options = new();
            action(options);

            ValidateRequiredProperties(options);

            return options;
        }

        private static void ValidateRequiredProperties<T>(T options)
        {
            IEnumerable<PropertyInfo> properties = typeof(T).GetProperties()
                .Where(prop => prop.GetCustomAttribute<RequiredAttribute>() is not null);

            List<Exception> errors = [];

            foreach (PropertyInfo property in properties.Where(property => property.GetValue(options) is null))
            {
                errors.Add(new InvalidOperationException($"The '{property.Name}' property must be set."));
            }

            if (errors.Count > 0)
            {
                throw new AggregateException(errors);
            }
        }
    }
}
