namespace Core.HttpClient.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class QueryParamNameAttribute : Attribute
    {
        public string Name { get; }

        public QueryParamNameAttribute(string name)
        {
            Name = name;
        }
    }
}
