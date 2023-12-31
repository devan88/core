namespace Core.Azure.Identity
{
    public record ServicePrincipalCredential
    {
        public string Name { get; set; }
        public string TenantId { get; set; }
        public string ClientId { get; set; }
        public virtual string ClientSecretKey => $"{Name}-Secret";
    }
}
