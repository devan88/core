namespace Core.Cloud.Storage
{
    public record StorageItem
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string ContentType { get; set; }
        public Stream Content { get; set; }
    }
}
