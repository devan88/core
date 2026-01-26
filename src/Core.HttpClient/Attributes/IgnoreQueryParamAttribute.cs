namespace Core.HttpClient.Attributes
{
    /// <summary>
    /// Attribute to indicate that a property should be ignored when generating query parameters.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class IgnoreQueryParamAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IgnoreQueryParamAttribute"/> class.
        /// </summary>
        public IgnoreQueryParamAttribute() { }
    }
}
