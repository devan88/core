namespace Core.Logging
{
    /// <summary>
    /// Defines a service for generating correlation IDs.
    /// </summary>
    public interface ICorrelationIdService
    {
        /// <summary>
        /// Generates a new correlation ID.
        /// </summary>
        /// <returns>A unique correlation ID.</returns>
        string GetCorrelationId();
    }
}
