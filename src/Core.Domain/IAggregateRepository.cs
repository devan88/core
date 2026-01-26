namespace Core.Domain
{
    /// <summary>
    /// Defines a contract for a repository that manages aggregates of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the aggregate root.</typeparam>
    /// <typeparam name="TId">The type of the identifier for the aggregate root.</typeparam>
    public interface IAggregateRepository<T, TId> where T : AggregateRoot<TId>
    {
        /// <summary>
        /// Asynchronously adds a new aggregate to the repository.
        /// </summary>
        /// <param name="aggregate">The aggregate root to be added.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the identifier of the added aggregate.
        /// </returns>
        Task<TId> AddAggregateAsync(T aggregate, CancellationToken cancellationToken);
    }
}
