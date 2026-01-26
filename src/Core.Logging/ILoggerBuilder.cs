namespace Core.Logging
{
    /// <summary>
    /// Defines the contract for a logger builder that configures and creates a logging instance.
    /// </summary>
    public interface ILoggerBuilder
    {
        /// <summary>
        /// Builds the logger instance based on the configured settings.
        /// </summary>
        void Build();

        /// <summary>
        /// Shuts down the logger, flushing any remaining log entries and releasing resources.
        /// </summary>
        void Shutdown();
    }
}
