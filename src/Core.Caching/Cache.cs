namespace Core.Caching
{
    /// <summary>
    /// Represents a token cache object.
    /// </summary>
    public sealed record Cache<T>
    {
        /// <summary>
        /// Gets or sets the token cache key.
        /// Typically, this is ClientId.
        /// </summary>
        public required string Key { get; init; }

        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        public required T Value { get; init; }

        /// <summary>
        /// Gets or sets the lifetime of when the cache should be expired.
        /// </summary>
        public TimeSpan ExpiresIn { get; init; }
    }
}
