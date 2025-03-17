namespace Core.Domain.Core
{
    /// <summary>
    /// Defines the contract for a domain event dispatcher.
    /// A domain event dispatcher is responsible for handling/distributing events
    /// that have been raised during domain operations.
    /// </summary>
    public interface IDomainEventDispatcher
    {
        /// <summary>
        /// Dispatches a domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event to dispatch.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DispatchAsync(DomainEvent domainEvent, CancellationToken cancellationToken);
    }
}
