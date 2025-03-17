namespace Core.HttpClient.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class IgnoreQueryParamAttribute : Attribute
    {
        public IgnoreQueryParamAttribute() { }
    }
}
