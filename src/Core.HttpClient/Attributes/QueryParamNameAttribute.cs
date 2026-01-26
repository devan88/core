namespace Core.HttpClient.Attributes
{
    /// <summary>
    /// Attribute to specify a custom name for a query parameter associated with a property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class QueryParamNameAttribute : Attribute
    {
        /// <summary>
        /// Gets the custom name for the query parameter.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryParamNameAttribute"/> class with the specified query parameter name.
        /// </summary>
        /// <param name="name">The custom name to be used for the query parameter.</param>
        public QueryParamNameAttribute(string name)
        {
            Name = name;
        }
    }
}
