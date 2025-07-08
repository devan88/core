namespace Core.Domain
{
    /// <summary>
    /// Contract for handling domain events.
    /// </summary>
    /// <typeparam name="T">domain event instance <see cref="DomainEvent"/>.</typeparam>
    public interface IDomainEventHandler<in T> where T : DomainEvent
    {
        /// <summary>
        /// Method 
        /// </summary>
        /// <param name="domainEvent"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task HandleAsync(T domainEvent, CancellationToken cancellationToken);
    }
}
