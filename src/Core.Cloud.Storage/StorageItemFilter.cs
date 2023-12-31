namespace Core.Cloud.Storage
{
    public record StorageItemFilter
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public IProgress<long> ProgressHandler { get; set; }
    }
}
